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

        // ���� tag ������ ���� ����� �ٸ��� �Ѵ�.
        normalEnemySize = 1.25f;
        strongEnemySize = 1.5f;
        BossEnemySize = 2.5f;
    }

    // �� ��ũ��Ʈ�� ������ ������Ʈ�� �̸��� �Ǻ��Ͽ� �ɷ�ġ�� �ٸ��� �ο��Ѵ�.
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

    // ���� ������Ʈ�� ü��, ����ġ, ������, �ӵ��� �����Ѵ�.
    // �ݺ��۾��� �����ϱ� ���� �Լ�ȭ�ߴ�.
    private void getEnemyStatus(float HP, float EXP, float damage, float speed)
    {
        this.HP = HP;
        this.EXP = EXP;
        this.damage = damage;
        this.speed = speed;
    }

    // ���� �������� ������ �޾ƿ´�.
    public void UpgradeStage(int stageLevel)
    {
        this.stageLevel = stageLevel;
    }

    // ���� �������� ������ ���� ������ �ɷ��� ��ȭ�Ѵ�.
    private void updateStatus()
    {
        HP *= (stageLevel * 0.5f);
        EXP += (stageLevel * 0.25f);
        damage += (stageLevel * 0.05f);
        speed += (stageLevel * 0.05f);
    }

    // ������Ʈ�� ���� ���� �ٲٵ� enemyTag�� ���� ũ�⸦ �ٸ��� �з��ϱ� ���� switch���� ����Ѵ�.
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

    // ���� ����� ���� ũ�⸦ �����Ͽ� �ü� ������ �ٲ۴�.
    private void changeDirection(float size)
    {
        Vector2 pos = playerLocation.position - transform.position;

        if (pos.x < 0)
            transform.localScale = new Vector2(size, size);
        else if (pos.x >= 0)
            transform.localScale = new Vector2(-size, size);
    }

    // �÷��̾ ���� ����.
    private void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerLocation.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾� ������ �޾��� ��� �� ü���� ���ҽ�Ų��.
        if (collision.gameObject.tag == "PlayerAttack")
        {
            Attack_Controller attack = collision.GetComponent<Attack_Controller>();
            GetDamage(attack.attackDamage());
        }
        // �÷��̾ �������� ��� �÷��̾��� ü���� ���ҽ�Ų��.
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
            player.GetExp(EXP); // �÷��̾� ����ġ ����
            player.kill();      // �÷��̾� ų ī��Ʈ ����

            // ���� ������Ʈ�� ������ ��� ���ڵ��
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
