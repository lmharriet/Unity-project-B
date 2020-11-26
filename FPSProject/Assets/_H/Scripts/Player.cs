using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float CamV, CamH;
    
    CharacterController controller;
    public Camera PlayerCam;
    public float hp = 20;

    float h, v;
    float speed = 3f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    //float TestClamp(float value, float min, float max)
    //{
    //    if (CamH < min) return min;
    //    else if (CamH > max) return max;

    //    else return value;
    //}

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        CamV += Input.GetAxis("Mouse X");
        CamH -= Input.GetAxis("Mouse Y");

        CamH = Mathf.Clamp(CamH, -90, 90);

        //clamp
       // camh = TestClamp(CamH, -90, 90);




        //Rotate -> += value;
        //Rotation  -> = value;
        transform.rotation = Quaternion.Euler(0f, CamV, 0f);        
        PlayerCam.transform.localRotation = Quaternion.Euler(CamH, 0f, 0f);
     

        Vector3 dirX = transform.right * h;
        Vector3 dirY = transform.forward * v;
        Vector3 moveDir = (dirX + dirY).normalized * speed;
        
        controller.Move(moveDir*Time.deltaTime);
        
    }
}
