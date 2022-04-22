using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ooze_red : MonoBehaviour
{
    public static ooze_red OOZERED_INSTANCE;

    public float HP;
    public float EXP;
    public float damage;
    public float speed;

    private void Awake()
    {
        OOZERED_INSTANCE = this;
    }

    private void Reset()
    {
        HP = 2500;
        EXP = 200;
        damage = 25f;
        speed = 0.3f;
    }
}
