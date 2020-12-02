using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int hp = 10;

    public void Damaged(int value)
    {
        hp -= value;
    }
}
