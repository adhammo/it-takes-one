using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Boss : MonoBehaviour
{
    [SerializeField] float offsetDistance = 5;
    [SerializeField] GameObject horizontalProjectile;
    [SerializeField] float horizontalAttackAngle = 45;
    [SerializeField] int numHorizontalProjectiles = 5;
    [SerializeField] Vector2 attackCooldowns = new Vector2(3, 6);
    [SerializeField] float moveDist = 5;

    enum BossState
    {
        Move,
        HorizontalAttack,
        VerticalAttack
    };

    NavMeshAgent _agent;
    bool _activated = false;
    Transform _target;
    BossState _state;

    public void Activate()
    {
        _activated = true;

        _state = GetRandomState();
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(attackCooldowns.x, attackCooldowns.y));

            switch (_state)
            {
                case BossState.Move:
                    {
                        bool pointOnMesh = false;
                        NavMeshHit navHit;

                        do
                        {
                            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * moveDist;

                            randomDirection += transform.position;

                            pointOnMesh = NavMesh.SamplePosition(randomDirection, out navHit, moveDist, NavMesh.AllAreas);

                        } while (!pointOnMesh);

                        Debug.DrawLine(transform.position, navHit.position, Color.green, 1000);
                        _agent.SetDestination(navHit.position);
                    }
                    break;

                case BossState.HorizontalAttack:
                    break;
                case BossState.VerticalAttack:
                    break;
            }

            _state = GetRandomState();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        Activate();
        //HorizontalArcAttack();
    }

    // Update is called once per frame
    void Update()
    {
        Transform targetCorrected = _target;
        targetCorrected.position = new Vector3(_target.position.x, transform.position.y, _target.position.z);

        transform.LookAt(targetCorrected);
    }

    void HorizontalArcAttack()
    {
        Vector3[] projectileDirections = rotateForward(horizontalAttackAngle, numHorizontalProjectiles, new Vector3(1, 0, 0));

        foreach (var direction in projectileDirections)
        {
            Vector3 spawnPos = transform.position + direction * offsetDistance;

            Debug.DrawRay(transform.position, direction * 100, Color.blue, 1000);

            var go = GameObject.Instantiate(horizontalProjectile, spawnPos, Quaternion.LookRotation(direction));
        }
    }

    Vector3[] rotateForward(float rotationAngle, int numSegments, Vector3 rotationAxis)
    {
        float angleFraction = rotationAngle / numSegments;
        float angle = -rotationAngle / 2 + angleFraction / 2;
        Vector3[] directions = new Vector3[numSegments];

        for (int i = 0; i < numHorizontalProjectiles; i++)
        {
            Vector3 angleVec = rotationAxis * angle;

            Quaternion attackOrientation = Quaternion.Euler(angleVec.x, angleVec.y, angleVec.z);
            directions[i] = (attackOrientation * transform.forward).normalized;

            angle += angleFraction;
        }

        return directions;
    }

    BossState GetRandomState()
    {
        var states = Enum.GetValues(typeof(BossState));
        return (BossState)states.GetValue(UnityEngine.Random.Range(0, states.Length));
    }
}
