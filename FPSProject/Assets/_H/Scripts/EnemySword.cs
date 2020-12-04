using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //충돌한 콜라이더의 태그가 "Player" 일 때
        if(other.tag == "Player")
        {
            //플레이어의 PlayerStat script를 pStat에 저장하고
            PlayerStat pStat = other.GetComponent<PlayerStat>();

            //랜덤 데미지를 뽑은 후
            int damage = Random.Range(1, 3);
            //pStat의 Damaged 함수를 실행
            pStat.Damaged(damage);

           // print(damage + "의 피해를 입었습니다!");
            
            //이제 충돌을 마쳤으므로 다단 히트를 방지하기 위해
            //오브젝트를 비활성화 한다.
            gameObject.SetActive(false);
        }
    }
}
