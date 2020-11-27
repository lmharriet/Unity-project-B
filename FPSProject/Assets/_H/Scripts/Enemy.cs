using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int hp = 10;
    public Vector3 returnPoint;
    // Start is called before the first frame update
    void Start()
    {
        returnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Damaged(int value)
    {
        hp -= value;
        if (hp<=0)
        {
            Destroy(gameObject, 3.0f);
        }
    }
    
}
