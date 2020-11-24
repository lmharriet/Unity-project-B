using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float speed = 5.0f; // 이동속도
    CharacterController controller;

    public float gravity = 9.1f;
    public float jumpSpeed = 8.0f;

    Vector3 dir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어 이동
        Move();
    }

    private void Move()
    {


        if (controller.isGrounded)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            dir = new Vector3(h, 0, v);

            dir.Normalize();

            dir = Camera.main.transform.TransformDirection(dir);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                dir.y = jumpSpeed;
            }
        }
        
            dir.y -= gravity * Time.deltaTime;
        

        //대각선 이동 속도를 상하좌우와 동일하게 만들기
        //게임에 따라 일부러 대각선은 빠르게 이동하도록 하는 경우도 있다
        //이럴때는 벡터의 정규화(Normalize)를 하면 안된다
        //transform.Translate(dir * speed * Time.deltaTime);



        //카메라가 보는 방향으로 이동해야한다.
        //dir = Camera.main.transform.TransformDirection(dir); // 회전처리할 때 필수

        // transform.Translate(dir * speed * Time.deltaTime);

     
        //문제점 : 충돌처리 안됨, 공중부양, 땅파고 들기 
        //캐릭터 컨트롤러 컴포넌트를 사용해서 문제점 해결하기
        //캐릭터컨트롤러는 충돌감지만 하고 물리가 적용안된다
        //따라서 충돌감지를 하기 위해서는 반드시
        // 캐릭터 컨트롤러 컴포넌트가 제공해주는 함수로 이동처리해야 한다.

        controller.Move(dir * speed * Time.deltaTime);

        //controller.SimpleMove(dir * speed * Time.deltaTime);


    }

}
