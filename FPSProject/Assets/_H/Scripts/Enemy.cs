using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int hp = 10;
    public Vector3 returnPoint;

    //플레이어를 따라갈 수 있는 최대 거리
    public float findRange = 20.0f;
    //플레이어를 공격할 수 있는 최대 거리
    public float atkRange = 5f;
    // Start is called before the first frame update
    void Start()
    {
        returnPoint = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //OnDrawGizmos를 사용해야 Gizmos를 Draw할 수 있다.
    private void OnDrawGizmos()
    {
        //Gizmos 색상 조절
        Gizmos.color = Color.red;
        //returnPoint(vector3) 위치에서 findRange(float)를 반지름으로 하는 원 그리기
        Gizmos.DrawWireSphere(returnPoint, findRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }

    public void Damaged(int value)
    {
        //죽었을 때 밑에있는 Die나 setDamaged로 인한 애니메이션 실행을 막기 위해서 사용한다.
        if (hp <= 0) return;

        hp -= value;

        if (hp<=0)
        {
            //같은 오브젝트에 있는 스크립트 EnemyFSM의 Die 함수를 실행한다.
            GetComponent<EnemyFSM>().Die();
        }

        else
        {
            //같은 오브젝트에 있는 스크립트 EnemyFSM의 setDamaged 함수를 실행한다.
            GetComponent<EnemyFSM>().setDamaged();
        }
    }
    
}
