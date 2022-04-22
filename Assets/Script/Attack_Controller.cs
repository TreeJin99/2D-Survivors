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

    // 발사체는 5초뒤 사라진다.
    private void startSetUp()
    {
        Destroy(gameObject, 5);
    }

    // 플레이어의 데미지와 현재 무기에 대한 정보를 받는다.
    public void damageSetUP(float playerDamage, GameObject currentWeapon)
    {
        this.playerDamage = playerDamage;
        this.currentWeapon = currentWeapon;
    }

    // Attack_Controller가 가진 데미지 정보를 전달한다.
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

    // 현재 무기 종류에 따라 피격효과를 다르게 한다.
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
