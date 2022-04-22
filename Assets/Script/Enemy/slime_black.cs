using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime_black : MonoBehaviour
{
    public static slime_black SLIMEBLACK_INSTANCE;

    public float HP;
    public float EXP;
    public float damage;
    public float speed;

    private void Awake()
    {
        SLIMEBLACK_INSTANCE = this;
    }

    private void Reset()
    {
        HP = 100;
        EXP = 15;
        damage = 5;
        speed = 0.3f;
    }
}
