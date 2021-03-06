using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour
{
    public Animator anim;
    public Transform Player;
    public float TowerRadius = 30f;
    public BotStatus Status;
    public GameObject MachineGuns;

    public float TimeToFire = 4f;
    private bool AttackFlag = true;
    private bool CanAttack = false;

    public GameObject BulletPrefab;
           GameObject CurrentBullet;
    public GameObject ShootingPoint;
    public GameObject ShootingPoint2;

    public ParticleSystem SmokeEffect;   
    public float angleDiff;

    public GameObject StandObject;

    void Start()
    {
        //eulerAngle_y = transform.localEulerAngles.y;            
    }

    void Update()
    {
        CheckDistance();
    }

    IEnumerator TimeToShoot (float time)
    {
        yield return new WaitForSeconds(time);
        Attack();
        AttackFlag=true;
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(Player.position, transform.position); 

        if ( distance <= TowerRadius && !Status.BotisDied  ) 
        {
             FaceTarget();
             if(AttackFlag && CanAttack)
             {
                StartCoroutine(TimeToShoot(TimeToFire));
                AttackFlag=false;
                CanAttack = false;
             }

        }
        else
        {
           anim.SetBool("Attack",false);
        }
    }

     void FaceTarget()
    {
        Vector3 direction = Player.position - transform.position;
        if(direction.x !=0 && direction.z !=0)
        {
            angleDiff = Vector3.Dot(StandObject.transform.forward * -1 , direction );

            if( angleDiff > 10f)
            {
                CanAttack = true;
              Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x * -1, 0, direction.z * -1));
              Quaternion lookRotation_MG = Quaternion.LookRotation(new Vector3(direction.x * -1, direction.y * -0.1f, direction.z * -1));

              transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation , Time.deltaTime * 5f);
              MachineGuns.transform.rotation = Quaternion.Slerp(MachineGuns.transform.rotation, lookRotation_MG , Time.deltaTime * 5f);
            }

           

        }
       
    }

    void Attack()
    {       
                    if(!Status.BotisDied)
                    {
                        anim.SetBool("Attack",true);
                    }

    }
    public void MachineGunSpy()
    {
                        GameObject CurrentBullet2;
                        Vector3 direction = Player.position - ShootingPoint.transform.position;
                        CurrentBullet2 = Instantiate (BulletPrefab,ShootingPoint.transform.position , ShootingPoint.transform.rotation);
                        CurrentBullet = Instantiate (BulletPrefab,ShootingPoint2.transform.position , ShootingPoint.transform.rotation);
                        Rigidbody rb = CurrentBullet.GetComponent<Rigidbody>();
                        Rigidbody rb2 = CurrentBullet2.GetComponent<Rigidbody>();
                        rb.AddForce(direction * 40f );
                        rb2.AddForce(direction * 40f );
                        Destroy(CurrentBullet,3f);
                        Destroy(CurrentBullet2,3f);
                        anim.SetBool("Attack",false);
    }

    public void CanonSpy()
    {
            Vector3 direction =   Player.position - ShootingPoint.transform.position;
            CurrentBullet = Instantiate (BulletPrefab,ShootingPoint.transform.position , ShootingPoint.transform.rotation);
            Rigidbody rb = CurrentBullet.GetComponent<Rigidbody>();
            rb.AddForce(direction * 40f );
            SmokeEffect.Play();
            Destroy(CurrentBullet,3f);
            anim.SetBool("Attack",false);
    }

}
