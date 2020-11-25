using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    //카메라가 플레이어 따라다니기
    //플레이어 자식으로 붙여서 이동해도 상관없다
    //하지만 게임에 따라서 드라마틱한 연출이 필요한 경우 처럼
    //게임 기획에 따라 1인칭 또는 3인칭등 변경이 필요할 수 있다.

    public Transform target;    //카메라가 따라다닐 타겟
    public float speed = 10.0f; //카메라 이동속도
    public Transform target1st; //1인칭 시점
    public bool isFPS = false;         //1인칭이냐?


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    private void LateUpdate()
    {
        //카메라가 타겟 따라다니기
        //FollowTarget();

        //1인칭 to 3인칭, 3인칭 to 1인칭으로 카메라 변경
        ChangeView();
    }

    private void ChangeView()
    {
        if (Input.GetKeyDown("1"))
        {
            isFPS = true;
        }
        if (Input.GetKeyDown("3"))
        {
            isFPS = false;
        }


        if (isFPS)
        {
            //카메라의 위치를 강제로 타겟에 고정해둔다
            transform.position = target1st.position;
        }
        else
        {
            //카메라의 위치를 강제로 타겟에 고정해둔다
            transform.position = target.position;
        }


    }

    private void FollowTarget()
    {
        //타겟 방향 구하기(벡터의 뺄셈)
        //방향 = 타겟 - 자기자신
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        transform.Translate(dir * speed * Time.deltaTime);

        //문제점 : 타겟에 도착하면 지진일어나는 것 처럼 보인다
        //거리를 구해서 고정시키면 된다
        //최적화 관련 면접 질문으로 종종 나온다


        //2D에서 사용했던 거리 구하는 공식
        //float x = x2 - x1;
        //floay y = y2 - y1;
        //return sqrtf(x * x + y * y);

        //1.벡터안의 Distance()함수 사용  Vector3.Distance(); ->실수를 리턴
        //2.벡터안의 magnitude 속성 사용  ->실수를 리턴
        //3.벡터안의 sqrMagnitude속성 사용  ->실수를 리턴
        

        //l.Distance()
        if(Vector3.Distance(target.position,transform.position) <1.0f)
        {
            transform.position = target.position;
        }

        //2.magnitude
        //float distance = (target.position - transform.position).magnitude;
        //if (distance < 1.0f) transform.position = target.position;


        //3.sqrMagnitude (정확한 값은 아니고 크기 비교만 할 때 사용한다)
        //성능상 유리하다 왜냐하면 루트연산을 하지 않기 때문에
        //float distance = (target.position - transform.position).sqrMagnitude;
        //if (distance < 1.0f) transform.position = target.position;

        //위와 같이 하고도 지진나는 경우
        //FollowTarget을 LateUpdate()에 넣으면 해결된다.

    }
}
