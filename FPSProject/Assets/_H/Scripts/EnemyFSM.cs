using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    GameObject Target;
    CharacterController controller;

    float distance;
    //Enemy state

    enum EnemyState
    {
        Idle, Move, Attack, Damaged, Die
    }

    EnemyState state; // Enemy state 변수

    Enemy enem;
    Animator anim;

    //유니티 길찾기 알고리즘이 적용된 네비게이션을 사용하려면
    //반드시 UnityEngine.AI를 추가해줘야 한다
    //네비게이션이 2D에서 했던 길찾기 알고리즘보다 성능이 좋다
    //2D기반 A*같은 경우는 본인 위치에서 실시간으로 계산을 해야 하는 반면
    //유니티 네비게이션은 맵전체를 베이크 해서 에이전트가 어느 위치에 있던
    //미리 계산된 정보를 사용한다

    NavMeshAgent agent;

    //FSM기반으로 코드를 짜는 경우 주의해야 할 사항 
    //충돌은 Collider로 하고,
    //이동만 NevMeshAgent를 사용해야
    //EnemyFSM을 제대로 사용할 수 있다
    //충돌이 제대로 작동하지 않을 수도 있다.

    //따라서 시작할 대 네비메시에이전트는꺼줘야 한다
    //게임오브젝트 -> 활성, 비활성 SetActive(true or false)

    //컴포넌트->활성, 비활성 enable = ture or false;


    //attack을 할 때 object를 활성화, 비활성화 하기 위함
    public GameObject swordObj;

    // Start is called before the first frame update
    void Start()
    {
        enem = GetComponent<Enemy>();

        state = EnemyState.Idle;

        Target = GameObject.Find("player");
        controller = GetComponent<CharacterController>();

        //object, transform 
        //anim = transform.Find("DogPolyart").GetComponent<Animator>();
        anim = GetComponentInChildren<Animator>();
        anim.SetInteger("state", 0);

        //처음은 공격 콜라이더가 비활성화 상태
        swordObj.SetActive(false);


        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(Target.transform.position, gameObject.transform.position);

        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damaged:
                //이 상태일 때는 아무것도 안함 (맞았을 때 move를 방지하기 위함)
                Damaged();
                break;
            case EnemyState.Die:
                agent.velocity = Vector3.zero;
                //이 상태일 때는 아무것도 안함 (죽었을 때 move를 방지하기 위함)
                break;
        }
        //print(state);
    }


    private void Idle()
    {
        //findRange보다 작을 때 (범위안에 들었을 때) move 상태로 변경
        if (distance < enem.findRange)
        {
            anim.SetInteger("state", 1);
            state = EnemyState.Move;
        }

    }
    //이동상태
    private void Move()
    {
        //이동은 네비게이션을 사용한다
        if (!agent.enabled) agent.enabled = true;


        //여기다가 쓰면 move가 돌때 계속 실행이됨.
        //anim.SetInteger("state", 1);

        //어떨때 플레이어한테 이동? 범위에 들어왔을 때
        //언제 리턴? 범위에 벗어났을 때

        float distance = Vector3.Distance(enem.returnPoint, Target.transform.position);

        //move to Target (tracking)
        if (distance < enem.findRange)
        {
            agent.SetDestination(Target.transform.position);
            ////어디를 봐야함?
            //transform.LookAt(Target.transform);

            //Vector3 dir = Target.transform.position - gameObject.transform.position;
            //dir.Normalize();
            //controller.Move(dir * 5.0f * Time.deltaTime);

            ////enem2Player
            float atkDistance = Vector3.Distance(transform.position, Target.transform.position);

            ////enemy와 player의 거리가 atkRange보다 작을 때(범위안에 들어왔을 때) state를 attack으로 변경
            if (atkDistance < enem.atkRange)
            {
                anim.SetInteger("state", 2);
                state = EnemyState.Attack;
            }
        }

        //move to returnPoint (return)
        else
        {
            //어디를 봐야함?
            // transform.LookAt(enem.returnPoint);

            Vector3 dir = enem.returnPoint - gameObject.transform.position;
            dir.Normalize();
            // controller.Move(dir * 3.0f * Time.deltaTime);

            agent.SetDestination(enem.returnPoint);

            float rtnDistance = Vector3.Distance(transform.position, enem.returnPoint);
            if (rtnDistance < 1.0f)
            {
                transform.position = enem.returnPoint;
                //reset -> identity는 같은 의미라고 생각하면 됨.
                transform.rotation = Quaternion.identity;

                anim.SetInteger("state", 0);
                state = EnemyState.Idle;

               
            }
        }
    }
    //공격상태
    private void Attack()
    {
        agent.velocity = Vector3.zero;

        float curFrame = anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
        // 소수점만 가져옴
        // % 1을 하는 이유는 최대를 1로 지정하기 위함이다. ( 0 < curFrame < 1 )

        if (curFrame <= 0.1f)
        {
            //플레이어 방향으로 LookAt하고
            transform.LookAt(Target.transform);

            //검의 콜라이더를 켜준다.
            swordObj.SetActive(true);
        }

        //현재 프레임이 1과 거의 가까워 졌을 때 (종료 직전일 때)
        if (curFrame >= 0.9f)
        {

            //검의 콜라이더를 비활성화 해준다. (아직 활성화 상태이면)
            if (swordObj.activeSelf) swordObj.SetActive(false);

            //거리를 비교하여 enemy의 state를 변경한다.
            if (distance > enem.atkRange)
            {
                //0 : idle, 1 : move, 2 : attack
                anim.SetInteger("state", 1);
                state = EnemyState.Move;
            }
        }
        agent.enabled = false;
    }


    public void setDamaged()
    {
        //공격도중 데미지를 입었을 때는 칼의 충돌 오브젝트를 비활성화 한다.
        if (swordObj.activeSelf) swordObj.SetActive(false);

        anim.SetTrigger("hit");
        state = EnemyState.Damaged;
        //아무리 animation 방식이 trigger라고 해도, 데미지를 입으면서 move가 되면 안되니까.
        //Damaged로 state를 옮겨둔다. (move를 못하게)
    }

    //피격상태 (Any State)
    private void Damaged()
    {
        agent.velocity = Vector3.zero;
        //animation frame이 거의 막바지에 도달했을 때
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            //state를 idle로 바꾼다!
            state = EnemyState.Idle;
        }
    }

    //죽음상태 (Any State)
    public void Die()
    {
        //이 경우도... 죽었는데 칼의 충돌체가 남아있으면 시체를 밟다가 데미지를 입는다
        if (swordObj.activeSelf) swordObj.SetActive(false);

        //죽은 상태(animation)가 실행될 때, 이전 state가 Move 상태이면, Die animation실행하며 플레이어를 향해
        //움직이는  state를 Die로 변경만 해준다( 사실상 변경해도 실행되는 update가 없다)
        state = EnemyState.Die;
        anim.SetTrigger("die");

        //뒤에 시간초를 넣어준 이유는? 잘 알거라고 생각합니다.
        Destroy(gameObject, 3f);

    }
}
