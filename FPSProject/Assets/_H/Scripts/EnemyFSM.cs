using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Animator anim;

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
        anim = transform.Find("DogPolyart").GetComponent<Animator>();
   
        anim.SetInteger("state", 0);

        //처음은 공격 콜라이더가 비활성화 상태
        swordObj.SetActive(false);
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
        //여기다가 쓰면 move가 돌때 계속 실행이됨.
        //anim.SetInteger("state", 1);

        //어떨때 플레이어한테 이동? 범위에 들어왔을 때
        //언제 리턴? 범위에 벗어났을 때

        float distance = Vector3.Distance(enem.returnPoint, Target.transform.position);

        //move to Target (tracking)
        if (distance < enem.findRange)
        {
            //어디를 봐야함?
            transform.LookAt(Target.transform);

            Vector3 dir = Target.transform.position - gameObject.transform.position;
            dir.Normalize();
            controller.Move(dir * 5.0f * Time.deltaTime);

            //enem2Player
            float atkDistance = Vector3.Distance(transform.position, Target.transform.position);

            //enemy와 player의 거리가 atkRange보다 작을 때(범위안에 들어왔을 때) state를 attack으로 변경
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
            transform.LookAt(enem.returnPoint);

            Vector3 dir = enem.returnPoint - gameObject.transform.position;
            dir.Normalize();
            controller.Move(dir * 3.0f * Time.deltaTime);

            float rtnDistance = Vector3.Distance(transform.position, enem.returnPoint);
            if (rtnDistance < 1.0f)
            {
                anim.SetInteger("state", 0);
                transform.position = enem.returnPoint;
                state = EnemyState.Idle;
            }
        }
    }
    //공격상태
    private void Attack()
    {
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
        //animation frame이 거의 막바지에 도달했을 때
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
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

        //오늘의 결론. 

        //코루틴 안썻음 ㅅㄱ
    }
}
