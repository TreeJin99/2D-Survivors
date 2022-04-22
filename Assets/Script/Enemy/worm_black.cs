using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worm_black : MonoBehaviour
{
    public static worm_black WORMBLACK_INSTANCE;

    public float HP;
    public float EXP;
    public float damage;
    public float speed;

    private void Awake()
    { 
        WORMBLACK_INSTANCE = this;
    }

    private void Reset()
    {
        HP = 100;
        EXP = 15;
        damage = 7.5f;
        speed = 0.3f;
    }
}
