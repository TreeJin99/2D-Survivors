using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cerberus_black : MonoBehaviour
{
    public static cerberus_black CERBERUSBLACK_INSTANCE;

    public float HP;
    public float EXP;
    public float damage;
    public float speed;

    private void Awake()
    {
        CERBERUSBLACK_INSTANCE = this;
    }

    private void Reset()
    {
        HP = 100;
        EXP = 30;
        damage = 12.5f;
        speed = 0.5f;
    }
}
