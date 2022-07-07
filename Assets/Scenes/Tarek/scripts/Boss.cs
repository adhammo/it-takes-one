using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;


[RequireComponent(typeof(NavMeshAgent))]
public class Boss : MonoBehaviour
{
    [SerializeField] float projectileSpawnOffset = 5;
    [SerializeField] GameObject projectile;

    [Header("Horizontal Attack Parameters")]
    [SerializeField] float horizontalAttackAngle = 45;
    [SerializeField] int numHorizontalProjectiles = 5;

    [Header("Vertical Attack Parameters")]
    [SerializeField] float verticalAttackAngle = 30;
    [SerializeField] int numVerticalProjectiles = 3;

    [Header("General Attack Parameters")]
    [SerializeField] float projectileSpeed = 10;
    [SerializeField] float projectileDamage = 5;

    [Header("AI Settings")]
    [SerializeField][Tooltip("Min and max times the AI waits between states/actions")] Vector2 stateCooldowns = new Vector2(3, 6);
    [SerializeField] float moveDist = 5;

    [SerializeField] Slider bossSlider;
    [SerializeField] TextMeshProUGUI _healthText;

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
    BotStatus _bossHeatlh;

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
            yield return new WaitForSeconds(UnityEngine.Random.Range(stateCooldowns.x, stateCooldowns.y));

            switch (_state)
            {
                case BossState.Move:
                    {
                        NavMeshHit navHit = GetRandomPointOnNavmesh();

                        Debug.DrawLine(transform.position, navHit.position, Color.green, 1000);
                        _agent.SetDestination(navHit.position);
                    }
                    break;

                case BossState.HorizontalAttack:
                    {
                        HorizontalArcAttack();
                    }
                    break;
                case BossState.VerticalAttack:
                    {
                        VerticalArcAttack();
                    }
                    break;
            }

            _state = GetRandomState();
        }
    }

    private NavMeshHit GetRandomPointOnNavmesh()
    {
        bool pointOnMesh = false;
        NavMeshHit navHit;

        do
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * moveDist;

            randomDirection += transform.position;

            pointOnMesh = NavMesh.SamplePosition(randomDirection, out navHit, moveDist, NavMesh.AllAreas);

        } while (!pointOnMesh);
        return navHit;
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _bossHeatlh = GetComponent<BotStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetCorrected = new Vector3(_target.position.x, transform.position.y, _target.position.z);
        transform.LookAt(targetCorrected);

        if (bossSlider)
            bossSlider.value = _bossHeatlh.currentHealth / _bossHeatlh.maxHealth;

        _healthText.text = $"{_bossHeatlh.currentHealth}/{_bossHeatlh.maxHealth}";
    }

    private void ArcAttack(Vector3[] projectileDirections)
    {
        foreach (var direction in projectileDirections)
        {
            Vector3 spawnPos = transform.position + direction * projectileSpawnOffset;

            Debug.DrawRay(transform.position, direction * 100, Color.blue, 1000);

            var projectile = GameObject.Instantiate(this.projectile, spawnPos, Quaternion.LookRotation(direction));
            var projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.AddForce(direction * projectileSpeed, ForceMode.Impulse);

            var projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.damage = projectileDamage;
        }
    }

    void HorizontalArcAttack()
    {
        Vector3[] projectileDirections = rotateForwardVector(horizontalAttackAngle, numHorizontalProjectiles, new Vector3(0, 1, 0));

        ArcAttack(projectileDirections);
    }


    void VerticalArcAttack()
    {
        Vector3[] projectileDirections = rotateForwardVector(verticalAttackAngle, numVerticalProjectiles, new Vector3(1, 0, 0));

        ArcAttack(projectileDirections);
    }

    Vector3[] rotateForwardVector(float rotationAngle, int numSegments, Vector3 rotationAxis)
    {
        float angleFraction = rotationAngle / numSegments;
        float angle = -rotationAngle / 2 + angleFraction / 2;
        Vector3[] directions = new Vector3[numHorizontalProjectiles];

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
