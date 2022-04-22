using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Controller : MonoBehaviour
{
    public GameObject hitCharged;
    public GameObject hitCrossed;
    public GameObject hitPulse;

    private GameObject currentWeapon;
    private GameObject currentWeaponEffect;
    private float playerDamage;


    private void Start()
    {
        startSetUp();
    }

    // �߻�ü�� 5�ʵ� �������.
    private void startSetUp()
    {
        Destroy(gameObject, 5);
    }

    // �÷��̾��� �������� ���� ���⿡ ���� ������ �޴´�.
    public void damageSetUP(float playerDamage, GameObject currentWeapon)
    {
        this.playerDamage = playerDamage;
        this.currentWeapon = currentWeapon;
    }

    // Attack_Controller�� ���� ������ ������ �����Ѵ�.
    public float attackDamage()
    {
        return playerDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "NormalEnemy" || tag == "StrongEnemy" || tag == "BossEnemy" || tag == "Stop")
        {
            isHit();
        }
    }

    // ���� ���� ������ ���� �ǰ�ȿ���� �ٸ��� �Ѵ�.
    private void isHit()
    {
        Destroy(gameObject);

        switch (currentWeapon.name)
        {
            case "pulse":
                currentWeaponEffect = hitPulse;
                break;

            case "charged":
                currentWeaponEffect = hitCharged;
                break;

            case "crossed":
                currentWeaponEffect = hitCrossed;
                break;
        }

        GameObject hitEffect = Instantiate(currentWeaponEffect, transform.position, transform.rotation);
        Destroy(hitEffect, 1);
    }
}
