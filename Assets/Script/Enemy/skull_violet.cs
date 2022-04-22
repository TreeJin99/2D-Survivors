using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skull_violet : MonoBehaviour
{
    public static skull_violet SKULLVIOLET_INSTANCE;

    public float HP;
    public float EXP;
    public float damage;
    public float speed;

    private void Awake()
    {
        SKULLVIOLET_INSTANCE = this;
    }

    private void Reset()
    {
        HP = 200;
        EXP = 40;
        damage = 15;
        speed = 0.4f;
    }
}
