using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skull_red : MonoBehaviour
{
    public static skull_red SKILLRED_INSTANCE;

    public float HP;
    public float EXP;
    public float damage;
    public float speed;

    private void Awake()
    {
        SKILLRED_INSTANCE = this;
    }

    private void Reset()
    {
        HP = 2000;
        EXP = 200;
        damage = 25;
        speed = 0.4f;
    }
}
