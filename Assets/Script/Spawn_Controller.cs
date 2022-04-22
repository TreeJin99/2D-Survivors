using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Controller : MonoBehaviour
{
    public static Spawn_Controller SPAWN_INSTANCE;

    public GameObject boneworm_black;
    public GameObject cerberus_black;
    public GameObject ooze_black;
    public GameObject skull_violet;
    public GameObject slime_black;
    public GameObject worm_black;
    public GameObject cerberus_red;
    public GameObject ooze_red;
    public GameObject skull_red;

    private Player_Controller player;
    private Transform target;
    private GameObject[] normalEnemies;
    private GameObject[] strongEnemies;
    private GameObject[] bossEnemies;

    public float spawnTime;
    public float spawnEnemiesCount;
    private int stageLevel;
    private int nextStageTime;

    private void Awake()
    {
        SPAWN_INSTANCE = this;
    }

    private void Start()
    {
        getBaseInfo();
        getPlayerInstance();    // 플레이어의 nextStageTime 값을 받아오기 위해 Awake()보다 느린 Start()에서 사용한다.
        spawnEnemies();
    }

    private void getBaseInfo()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        normalEnemies = new GameObject[] { boneworm_black, cerberus_black, slime_black, worm_black };
        strongEnemies = new GameObject[] { ooze_black, skull_violet };
        bossEnemies = new GameObject[] { cerberus_red, ooze_red, skull_red };
    }

    private void getPlayerInstance()
    {
        player = Player_Controller.PLAYER_INSTANCE;
        nextStageTime = player.nextStageTime;
    }

    public void UpgradeStage(int stageLevel)
    {
        this.stageLevel = stageLevel;
    }

    // 지정된 시간만큼 지정된 함수를 호출한다.
    // 즉, 지정된 시간만큼 적을 소환한다.
    private void spawnEnemies()
    {
        InvokeRepeating("SpawnEnemy", 1, nextStageTime/2);
        InvokeRepeating("SpawnBoss", 1, nextStageTime);
    }


    private void SpawnEnemy()
    {
        for(int i = 0; i < spawnEnemiesCount; i++)
        {
            int r1 = Random.Range(0, 4);

            GameObject spawnEnemyLeft = Instantiate(normalEnemies[r1], getRandomLeftLocate(), transform.rotation);
            GameObject spawnEnemyRight = Instantiate(normalEnemies[r1], getRandomRightLocate(), transform.rotation);
            spawnEnemyLeft.GetComponent<Enemy_Controller>().UpgradeStage(stageLevel);
            spawnEnemyRight.GetComponent<Enemy_Controller>().UpgradeStage(stageLevel);
        }

        int r2 = Random.Range(0, 2);
        GameObject spawnStrongEnemyLeft = Instantiate(strongEnemies[r2], getRandomLeftLocate(), transform.rotation);
        GameObject spawnStrongEnemyRight = Instantiate(strongEnemies[r2], getRandomRightLocate(), transform.rotation);
        spawnStrongEnemyLeft.GetComponent<Enemy_Controller>().UpgradeStage(stageLevel);
        spawnStrongEnemyRight.GetComponent<Enemy_Controller>().UpgradeStage(stageLevel);
    }

    private void SpawnBoss()
    {
        int randomLocate = Random.Range(0, 2);
        int randomSpawn = Random.Range(0, 3);

        switch (randomLocate)
        {
            case 0:
                GameObject spawnEnemyLeft = Instantiate(bossEnemies[randomSpawn], getRandomLeftLocate(), transform.rotation);
                spawnEnemyLeft.GetComponent<Enemy_Controller>().UpgradeStage(stageLevel);
                break;

            case 1:
                GameObject spawnEnemyRight = Instantiate(bossEnemies[randomSpawn], getRandomRightLocate(), transform.rotation);
                spawnEnemyRight.GetComponent<Enemy_Controller>().UpgradeStage(stageLevel);
                break;
        }
    }

    private Vector2 getRandomLeftLocate()
    {
        Vector2 spawnNormalLeft = new Vector2(target.position.x - 3.5f, target.position.y + Random.Range(-2.5f, 2.5f));
        return spawnNormalLeft;
    }

    private Vector2 getRandomRightLocate()
    {
        Vector2 spawnNormalRight = new Vector2(target.position.x + 3.5f, target.position.y + Random.Range(-2.5f, 2.5f));
        return spawnNormalRight;
    }
}
