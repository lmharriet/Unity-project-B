using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    public Camera cam;
    public GameObject flames;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 50f, Color.Lerp(Color.black, Color.blue, 0.1f));

        if(Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f))
            {
                print(hit.collider.name);

                Instantiate(flames, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        
    }
}
