using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    public static Enemy_Controller ENEMY_INSTANCE;
    public GameObject BonusChest;

    private Player_Controller player;
    private Transform playerLocation;

    private string enemyTag;
    private string enemyName;
    private float HP;
    private float EXP;
    private float damage;
    private float speed;
    private float normalEnemySize;
    private float strongEnemySize;
    private float BossEnemySize;
    private int stageLevel;

    private void Awake()
    {
        ENEMY_INSTANCE = this;
    }

    private void Start()
    {
        startSetUp();
        currentEnemyIs();
        updateStatus();
    }

    private void Update()
    {
        EnemyDirection();
        FollowPlayer();
    }

    private void startSetUp()
    {
        player = Player_Controller.PLAYER_INSTANCE;
        playerLocation = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // 몬스터 tag 종류에 따라 사이즈를 다르게 한다.
        normalEnemySize = 1.25f;
        strongEnemySize = 1.5f;
        BossEnemySize = 2.5f;
    }

    // 이 스크립트를 소유한 오브젝트의 이름을 판별하여 능력치를 다르게 부여한다.
    private void currentEnemyIs()
    {
        enemyTag = gameObject.tag;
        enemyName = gameObject.name;

        if (enemyTag == "NormalEnemy")
        {
            switch (enemyName)
            {
                case "boneworm-black(Clone)":
                    boneworm_black boneworm_black = boneworm_black.BONEWORMBLACK_INSTANCE;
                    getEnemyStatus(boneworm_black.HP, boneworm_black.EXP, boneworm_black.damage, boneworm_black.speed);
                    break;

                case "cerberus-black(Clone)":
                    cerberus_black cerberus_black = cerberus_black.CERBERUSBLACK_INSTANCE;
                    getEnemyStatus(cerberus_black.HP, cerberus_black.EXP, cerberus_black.damage, cerberus_black.speed);
                    break;

                case "slime-black(Clone)":
                    slime_black slime_black = slime_black.SLIMEBLACK_INSTANCE;
                    getEnemyStatus(slime_black.HP, slime_black.EXP, slime_black.damage, slime_black.speed);
                    break;

                case "worm-black(Clone)":
                    worm_black worm_black = worm_black.WORMBLACK_INSTANCE;
                    getEnemyStatus(worm_black.HP, worm_black.EXP, worm_black.damage, worm_black.speed);
                    break;
            }

        }
        else if (enemyTag == "StrongEnemy")
        {
            switch (enemyName)
            {
                case "ooze-black(Clone)":
                    ooze_black ooze_black = ooze_black.OOZEBLACK_INSTANCE;
                    getEnemyStatus(ooze_black.HP, ooze_black.EXP, ooze_black.damage, ooze_black.speed);
                    break;

                case "skull-violet(Clone)":
                    skull_violet skull_violet = skull_violet.SKULLVIOLET_INSTANCE;
                    getEnemyStatus(skull_violet.HP, skull_violet.EXP, skull_violet.damage, skull_violet.speed);
                    break;
            }
        }
        else if (enemyTag == "BossEnemy")
        {
            switch (enemyName)
            {
                case "cerberus-red(Clone)":
                    cerberus_red cerberus_red = cerberus_red.CERBERUSRED_INSTANCE;
                    getEnemyStatus(cerberus_red.HP, cerberus_red.EXP, cerberus_red.damage, cerberus_red.speed);
                    break;

                case "skull-red(Clone)":
                    skull_red skull_red = skull_red.SKILLRED_INSTANCE;
                    getEnemyStatus(skull_red.HP, skull_red.EXP, skull_red.damage, skull_red.speed);
                    break;

                case "ooze-red(Clone)":
                    ooze_red ooze_red = ooze_red.OOZERED_INSTANCE;
                    getEnemyStatus(ooze_red.HP, ooze_red.EXP, ooze_red.damage, ooze_red.speed);
                    break;
            }
        }
    }

    // 현재 오브젝트의 체력, 경험치, 데미지, 속도를 주입한다.
    // 반복작업을 방지하기 위해 함수화했다.
    private void getEnemyStatus(float HP, float EXP, float damage, float speed)
    {
        this.HP = HP;
        this.EXP = EXP;
        this.damage = damage;
        this.speed = speed;
    }

    // 현재 스테이지 레벨을 받아온다.
    public void UpgradeStage(int stageLevel)
    {
        this.stageLevel = stageLevel;
    }

    // 현재 스테이지 레벨에 맞춰 몬스터의 능력을 강화한다.
    private void updateStatus()
    {
        HP *= (stageLevel * 0.5f);
        EXP += (stageLevel * 0.25f);
        damage += (stageLevel * 0.05f);
        speed += (stageLevel * 0.05f);
    }

    // 오브젝트가 보는 방향 바꾸되 enemyTag에 따라 크기를 다르게 분류하기 위해 switch문을 사용한다.
    private void EnemyDirection()
    {
        switch (enemyTag)
        {
            case "NormalEnemy":
                changeDirection(normalEnemySize);
                break;

            case "StrongEnemy":
                changeDirection(strongEnemySize);
                break;

            case "BossEnemy":
                changeDirection(BossEnemySize);
                break;
        }
    }

    // 받은 사이즈에 맞춰 크기를 지정하여 시선 방향을 바꾼다.
    private void changeDirection(float size)
    {
        Vector2 pos = playerLocation.position - transform.position;

        if (pos.x < 0)
            transform.localScale = new Vector2(size, size);
        else if (pos.x >= 0)
            transform.localScale = new Vector2(-size, size);
    }

    // 플레이어를 따라 간다.
    private void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerLocation.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 공격을 받았을 경우 내 체력을 감소시킨다.
        if (collision.gameObject.tag == "PlayerAttack")
        {
            Attack_Controller attack = collision.GetComponent<Attack_Controller>();
            GetDamage(attack.attackDamage());
        }
        // 플레이어를 공격했을 경우 플레이어의 체력을 감소시킨다.
        else if (collision.gameObject.tag == "Player")
        {
            player.GetDamaged(damage);
        }
    }

    private void GetDamage(float playerDamage)
    {
        HP -= playerDamage;
        player.ActiveHitSound();
        if (HP <= 0)
        {
            player.GetExp(EXP); // 플레이어 경험치 증가
            player.kill();      // 플레이어 킬 카운트 증가

            // 현재 오브젝트가 보스일 경우 상자드랍
            if (enemyTag == "BossEnemy")
            {
                DropChest();
            }

            Destroy(gameObject);
        }
    }

    private void DropChest()
    {
        GameObject chest = Instantiate(BonusChest, transform.position, Quaternion.identity);
    }
}
