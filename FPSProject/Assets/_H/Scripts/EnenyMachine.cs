using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnenyMachine : MonoBehaviour
{
    //Enemy state

    enum EnemyState
    {
        Idle, Move, Attack, Return, Damaged, Die
    }

    EnemyState state; // Enemy state 변수

    Enemy enem;
    Animator anim;
    //Coroutine coru;

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

    //필요한 변수들

    public float findRange = 15f;   //플레이어를 찾는 범위
    public float moveRange = 30f;   //시작지점에서 최대 이동가능한 범위
    public float attackRange = 2f;  //공격 가능 범위
    Vector3 startPoint;             //몬스터 시작위치
    Transform player; //플레이어를 찾기 위해서
    CharacterController cc; //에너미 이동을 위해서

    //에너미 일반변수
    int hp = 100; // 체력
    int att = 5;  //공격력
    float speed = 5.0f; // 이동속도

    //공격 딜레이

    float attTime = 2.0f;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {


      

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
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
        //print(state);
    }


    private void Idle()
    {
        //1.플레이어와 일정범위가 되면 이동상태로 변경(탐지범위)
        //- 플레이어 찾기(GameObject.Find("Player")); ->타겟찾기
        //- 일정범위 30미터 (거리비교 : Distance, Magnitude 아무거나 사용)
        //- 상태변경 ->이동
        //- 상태전환 출력

        if(Vector3.Distance(transform.position,player.position)<findRange)
        {
            state = EnemyState.Move;
            print("상태전환 : Idle -> Move");
        }

    }
    //이동상태
    private void Move()
    {
        //여기다가 쓰면 move가 돌때 계속 실행이됨.
        //anim.SetInteger("state", 0);

        //1. 플레이어를 향해서 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이어를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경
        //- 플레이어처럼 캐릭터컨트롤러를 이용하기
        //- 공격범위 1미터
        //- 상태변경-> 공격상태 or 리턴상태


        //이동 할 수 있는 최대범위를 벗어나면 돌아와야한다.
        if(Vector3.Distance(transform.position,startPoint)>moveRange)
        {
            state = EnemyState.Return;
            print("상태전환 : Move -> Return");
        }
        else if(Vector3.Distance(transform.position,player.position)>attackRange)
        {
            //플레이어를 추격
            //이동방향 (벡터의 뺄셈)
            //방향 = 타겟 - 자기자신
            Vector3 dir = (player.position - transform.position).normalized;
            //dir.Normalize();

            //에너미가 백스텝으로 쫒아온다
            //에너미가 타겟을 바라보게 하고 싶다
            //방법1
            //transform.forward =dir;
            //방법2
            //transform.LookAt(player);

            //순간이동이 아닌 좀더 자연스럽게 회전처리를 하고 싶다
            //선형보간

            //transform.forward = Vector3.Lerp(transform.forward, dir , 10 * Time.deltaTime);
            //여기도 문제가 있는데 플레이어와 에너미가 일직선상에 있으면 왼쪽으로 회전해야 할지 오른쪽으로 회전해야 할지 
            //알 수가 없어서 그냥 백덤블링을 해버린다.

            //유니티 내부적으로 회전은 전부 쿼터니온으로 처리되고 있기 때문에
            //자연스러운 회전처리를 하려면 결국 쿼터니온으로 사용해야 한다
            //하지만 이걸 몰라도 크게 상관은 없다
            //결국 우리는 네비메시에이전트를 사용하면 이딴거 다 자동으로 적용된다

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);


            //에너미 이동 
            //cc.Move(dir * speed * Time.deltaTime);
            //중력이 적용안되는 문제가 있다

            //중력문제를 해결하기 위해서 simpleMove를 사용한다.
            //심플무브는 최소한의 물리가 적용되어 중력문제를 해결할 수 있다
            //단 내부적으로 시간처리를 
            cc.SimpleMove(dir * speed);


        }
        //공격범위 안에 들어온 상태
        else
        {
            state = EnemyState.Attack;
            print("상태전환 : Move -> Attack");
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
      if(Vector3.Distance(transform.position, player.position)>attackRange)
        {
            timer += Time.deltaTime;
           if(timer>attTime)
            {
                print("공격");
                //플레이어의 필요한 스크립트 컴포넌트 가져와서 데미지를 주면 된다
                //player.GetComponent<PlayerMove>();

                //타이머 초기화
                timer = 0.0f;

            }
        }
      else //현재상태를 무브를 전환(재추격)
        {
            state = EnemyState.Move;
            print("상태전환 : Attack -> Move");
            timer = 0.0f;
        }
    }

    private void Return()
    {
        //1.에너미가 플레이어를 추격하더라도 처음 위치에서 일정범위를 벗어나면 다시 돌아옴
        //-처음위치에서 일정범위 30미터
        //-상태변경
        //-상태전환 출력
      
        //시작위치까지 도달하지 않을때는 이동
        //도착하면 대기상태로 변경
        if(Vector3.Distance(transform.position,startPoint)>0.1f)
        {
            Vector3 dir = (startPoint - transform.position).normalized;
            cc.SimpleMove(dir * speed);
        }
        else
        {
            //위치값을 강제로 고정
            transform.position = startPoint;
            state = EnemyState.Idle;
            print("상태전환 : Return -> Idle");
        }
    }
    //플레이어쪽에서 충돌감지를 할 수 있으니 이 함수는 퍼블릭으로 만들자 
   public void hitDamage(int value)
    {
        //체력깍기
        hp -= value;

        if(hp>0)
        {
            state = EnemyState.Damaged;
            Damaged();
        }

        else
        {
            state = EnemyState.Die;
            hp = 0;
            Die();
        }
    }

    //피격상태 (Any State)
    private void Damaged()
    {
        //코루틴 사용
        //1. 에너미 체력이 1이상일때만 피격받을 수 있다.
        //2. 다시 이전상태로 변경
        //-상태변경
        //-상태전환 출력

        StartCoroutine(DamageProc());
    }

    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1.0f);
        //현재상태를 이동으로 전환
        state = EnemyState.Move;
        print("상태전환 : Damaged->Move");
    
    }

    //죽음상태 (Any State)
    private void Die()
    {
        //코루틴 사용
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //-상태변경
        //-상태전환 출력

        //진행중인 모든 코루틴은 정지한다
        StopAllCoroutines();
    }

    IEnumerator DieProc()
    {
        //캐릭터 컨트롤러 비활성화
        //cc.enabled;
        //2초후에 자기자신을 제거한다

        yield return new WaitForSeconds(2.0f);
        print("죽었다");
        Destroy(gameObject);
    }

    //여기에서 정의 해둔 Gizmo들은 씬뷰에서만 보인다
    private void OnDrawGizmos()
    {
        //공격가능 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        //플레이어 찾을 수 있는 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);
       
        //시작지점으로 부터 이동가능한 최대 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPoint, moveRange);
    }


}
