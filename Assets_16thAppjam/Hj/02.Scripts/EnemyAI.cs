using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    NavMeshAgent agent;
    public enum State { Normal, Angry, Random }
    public State nowState;

    Animator animator;

    public float Speed = 5.0f;
    public int hp = 20;

    bool isDead = false;

    public int index;
    
    public Transform Player;
    public Vector3 originPos;
    Vector3 targetPos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        Player = GameObject.Find("[CameraRig]").transform;
        targetPos = Player.position;
    }

    private void Start()
    {
        agent.speed = Speed;
        InvokeRepeating("PathFind", 0, 0.1f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Damage(1);   
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        //플레이어가 쏜 불꽃에 맞았을때
        if (other.CompareTag("Fire"))
        {
            //수정해줘 동미니니니니니ㅣ니나
            BulletCtrl bullet = other.GetComponent<BulletCtrl>();
            bullet.Boom();
            Damage(bullet.Damage);
            other.gameObject.GetComponent<BulletCtrl>().OffBullet();
        }
    }

    public void ResetAI()
    {
        //인공지능 초기화
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("[CameraRig]").transform;
        targetPos = Player.position;
        nowState = State.Normal;
    }

    //길찾기 0.1초마다 호출됨
    void PathFind()
    {
        Debug.Log("!!!!");
        if (!GameManager.instance.Gameovered)
        {
            if (Vector3.Distance(Player.position, transform.position) > 25)
                EnemyManager.instance.EnemyDie(gameObject);

            if (nowState == State.Normal)
                agent.SetDestination(Quaternion.Euler(0, 20 * index, 0) * Vector3.forward * 2 + originPos);
            else if(nowState == State.Angry)
                agent.SetDestination(targetPos);
            else
            {
                targetPos = new Vector3(0, 0, 40);
                agent.SetDestination(targetPos);
            }
        }
    }

    //데미지를 입음
    public void Damage(int dmg)
    {
        animator.SetBool("IsRunning", true);
        StartCoroutine("DamageAnim");

        if (nowState == State.Normal)
        {
            if (Random.Range(0,2) == 0)
            {
                nowState = State.Angry;
                agent.speed = Speed * 1.25f;
            }
            else
            {
                nowState = State.Random;
                //targetPos = transform.position + (transform.position - targetPos);
                targetPos = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward * 50;
            }
        }

        hp -= dmg;
        if(hp <= 0 && !isDead)
        {
            isDead = true;

            switch (nowState)
            {
                case State.Angry:
                    GameManager.instance.ScoreAdd(5);
                    break;
                case State.Random:
                    GameManager.instance.ScoreAdd(3);
                    break;
                case State.Normal:
                    GameManager.instance.ScoreAdd(10);
                    break;
            }
            
            StartCoroutine("DeadAnim");
        }
    }

    IEnumerator DamageAnim()
    {
        animator.SetBool("IsDamage", true);
        agent.speed = 0;
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsDamage", false);
        agent.speed = Speed;
    }

    IEnumerator DeadAnim()
    {
        animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(0.2f);

        animator.SetBool("IsRunning", false);
        animator.SetBool("IsDead", false);
        hp = 20;
        EnemyManager.instance.EnemyDie(gameObject);
        
        targetPos = Player.position;
        nowState = State.Normal;

        isDead = false;
        EnemyManager.instance.EnemyAdd();
    }
}
