using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cerberus_red : MonoBehaviour
{
    public static cerberus_red CERBERUSRED_INSTANCE;

    public float HP;
    public float EXP;
    public float damage;
    public float speed;

    private void Awake()
    {
        CERBERUSRED_INSTANCE = this;
    }

    private void Reset()
    {
        HP = 2000;
        EXP = 200;
        damage = 20;
        speed = 0.5f;
    }
}
