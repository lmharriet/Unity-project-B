using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    GameObject Target;
    CharacterController controller;
    Vector3 returnPoint;
    float distance;
    //Enemy state

    enum EnemyState
    {
        Idle, Move, Attack, Return, Damaged, Die
    }

    EnemyState state; // Enemy state 변수

    //유용한 기능
    #region "Idle 상태에 필요한 변수들"

    #endregion
    #region "Move 상태에 필요한 변수들"

    #endregion
    #region "Attack 상태에 필요한 변수들"

    #endregion
    #region "Return 상태에 필요한 변수들"

    #endregion
    #region "Damaged 상태에 필요한 변수들"

    #endregion
    #region "Die 상태에 필요한 변수들"
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState.Idle;

        Target = GameObject.Find("player");
        returnPoint = new Vector3(-35f, 3.5f, -45f);
        gameObject.transform.position = returnPoint;
        controller = GetComponent<CharacterController>();
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
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Dagamed();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
        print(state);
    }


    private void Idle()
    {
        //1.플레이어와 일정범위가 되면 이동상태로 변경(탐지범위)
        //- 플레이어 찾기(GameObject.Find("Player")); ->타겟찾기
        //- 일정범위 30미터 (거리비교 : Distance, Magnitude 아무거나 사용)
        //- 상태변경 ->이동
        //- 상태전환 출력


        if (distance < 20.0f)
        {
            state = EnemyState.Move;
        }

    }
    //이동상태
    private void Move()
    {
        //1. 플레이어를 향해서 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이어를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경
        //- 플레이어처럼 캐릭터컨트롤러를 이용하기
        //- 공격범위 1미터
        //- 상태변경-> 공격상태 or 리턴상태
        //- 상태전환 출력 
        Vector3 dir = Target.transform.position - gameObject.transform.position;
        dir.Normalize();
        controller.Move(dir * 5.0f * Time.deltaTime);

        if (distance < 5.0)
        {
            state = EnemyState.Attack;
        }
        if (Vector3.Distance(gameObject.transform.position, returnPoint) > 20.0f)
        {
            state = EnemyState.Return;
        }


    }
    //공격상태
    private void Attack()
    {
        //1. 플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어를 공격
        //2. 플레이어가 공격범위를 벗어나면 상태변경 -> 이동상태
        //-공격범위 1미터
        //-상태변경 ->이동
        //-상태전환 출력
        if (distance > 5.0f)
        {
            state = EnemyState.Move;
        }

    }
    private void Return()
    {
        //1.에너미가 플레이어를 추격하더라도 처음 위치에서 일정범위를 벗어나면 다시 돌아옴
        //-처음위치에서 일정범위 30미터
        //-상태변경
        //-상태전환 출력
        Vector3 dir = returnPoint - gameObject.transform.position;
        dir.Normalize();
        controller.Move(dir * 3.0f * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, returnPoint);
        if (distance < 1.0f)
        {
            transform.position = returnPoint;
            state = EnemyState.Idle;
        }
    }

    //피격상태 (Any State)
    private void Dagamed()
    {
        //코루틴 사용
        //1. 에너미 체력이 1이상일때만 피격받을 수 있다.
        //2. 다시 이전상태로 변경
        //-상태변경
        //-상태전환 출력

    }

    //죽음상태 (Any State)
    private void Die()
    {
        //코루틴 사용
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //-상태변경
        //-상태전환 출력
    }

}
