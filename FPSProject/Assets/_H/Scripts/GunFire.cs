using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    public Camera cam;
    public GameObject flames;
    public GameObject bombFactory;
    public GameObject firePoint;

    float power = 15.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        RayFire();
        //Fire();
    }

    private void RayFire()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 50f,
           Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 1)));

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f))
            {
                print(hit.collider.name);

                Instantiate(flames, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }


        //마우스 우측 버튼 클릭시 수류탄 던지기

        if (Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePoint.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            //rb.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);

            //45도 각도로 발사
            Vector3 dir = Camera.main.transform.forward + (Camera.main.transform.up*2f);
            dir.Normalize();
            rb.AddForce(dir * power, ForceMode.Impulse);
            
        }

    }
    //private void Fire()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
    //        RaycastHit hitInfo;

    //        if (Physics.Raycast(ray, out hitInfo))
    //        {
    //            print("충돌 오브젝트 : " + hitInfo.collider.name);
    //            GameObject bulletFx;
    //            bulletFx = Instantiate(flames);
    //            bulletFx.transform.forward = hitInfo.normal;

    //            //최적화
    //            //1.오브젝트 풀링
    //            //2.정점수 줄이기 LOD
    //            //3.파티클에서 3D모델보다는 가급적 스프라이트 사용하기
    //            //4.비어있는 함수 제거하기
    //            //5.레이어 마스크 사용 충돌처리
    //            //유니티 내부적으로 속도향상을 위해 비트연산 처리가 된다
    //            //총 32비트를 사용하기 때문에 레이어도 32개까지 추가 가능함

    //            int layer = gameObject.layer + object.layer + bossObject.layer;
    //            //layer = 1 << 8;
    //            // 0000 0000 0000 0001 => 0000 0001 0000 0000
    //            layer = 1 << 8 | 1 << 9 | 1 << 12;
    //            //0000 0001 0000 0000 =>player
    //            //0000 0010 0000 0000 =>enemy
    //            //0001 0000 0000 0000 =>boss

    //            //0001 0011 0000 0000 =>Player , Enemy , boss 모두다 충돌처리 가능

    //            //if (Physics.Raycast(ray, out hitInfo, 100, layer)) //layer만 충돌
    //            //if (Physics.Raycast(ray, out hitInfo, 100,~layer)) //layer만 충돌 제외
    //            //{
    //            //   
    //            //}

    //            //   //if(플레이어와 충돌했다면)
    //            //   //if(에너미와 충돌했다면)
    //            //   //if(보스와 충돌했다면)
    //            //   //이런식이면 if문이 많이 들어가게 되고
    //            //   //당연히 성능이 조금이라도 떨어질 수밖에 없다
    //            //   //비트연산은 성능 최적화에 도움이된다.

    //        }

    //    }


    //}



}
