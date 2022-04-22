using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ooze_black : MonoBehaviour
{
    public static ooze_black OOZEBLACK_INSTANCE;

    public float HP;
    public float EXP;
    public float damage;
    public float speed;

    private void Awake()
    {
        OOZEBLACK_INSTANCE = this;
    }

    private void Reset()
    {
        HP = 150;
        EXP = 40;
        damage = 10f;
        speed = 0.3f;
    }
}
