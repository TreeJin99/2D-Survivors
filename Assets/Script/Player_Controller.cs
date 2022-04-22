using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    public static Player_Controller PLAYER_INSTANCE;
    public Rigidbody2D playerRigidBody;
    public SpriteRenderer playerSpriteRenderer;
    public GameObject pulse;
    public GameObject charged;
    public GameObject crossed;
    public GameObject levelupPanel;
    public GameObject RestartPanel;
    public Text CoolTimeText;
    public AudioSource hitSound;

    public float gracePeriod;           // �ǰݽ� ���� ���ӽð�
    public float playerHP;
    public float playerDamage;
    public float playerMovementSpeed;
    public float coolTime;
    public int nextStageTime;           // ���� ���������� �Ѿ�� �ð�
    public Text dieStageTime;
    public Text dieStageLevel;


    private GameObject currentWeapon;   // ���� ���� ����
    private Spawn_Controller spawn;     // Spawn_Controller�� INSTANCE
    private Vector3 direction;          // �÷��̾ �ٶ󺸴� ����
    private Text myHP;
    private Text myDamage;
    private Text myLevel;
    private Text stageTimeTxt;
    private Text killCountText;
    private Text StageText;

    private float currentHP;
    private float currentEXP;
    private float currentTime;
    private float stageTime;
    private float levelUp;
    private int killCount;
    private float attackSpeed;
    private bool secondaryAttack;
    private int stageLevel;             // ���� �������� 


    private void Awake()
    {
        PLAYER_INSTANCE = this;

        // Player ������Ʈ ����
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        // Canvas ����
        myHP = GameObject.Find("MyHP").GetComponent<Text>();
        myDamage = GameObject.Find("MyDamage").GetComponent<Text>();
        myLevel = GameObject.Find("MyLevel").GetComponent<Text>();
        stageTimeTxt = GameObject.Find("Survive Time").GetComponent<Text>();
        killCountText = GameObject.Find("MyKill").GetComponent<Text>();
        StageText = GameObject.Find("StageLevel").GetComponent<Text>();
    }

    private void Start()
    {
        startSetUp();       // �ʱ� ����
        UpdateUI();         // Canvas �ʱ�ȭ
    }

    private void Update()
    {
        updateStageTime();  // ���� ���������� �ð�
        playerMovement();   // �÷��̾� �̵� �޼ҵ�
        playerAttack();     // �÷��̾� ���� �޼ҵ�
    }

    private void startSetUp()
    {
        spawn = Spawn_Controller.SPAWN_INSTANCE;
        currentHP = playerHP;
        currentWeapon = pulse;
        levelUp = 300;
        killCount = 0;
        attackSpeed = 200;
        secondaryAttack = false;
    }

    public int getStageLevel()
    {
        return stageLevel;
    }

    // ���� ������ stage�� �����Ѵ�.
    // stage�� �ǽð����� Spawn_Controller�� �Ѱ��� ���������� �ö󰥼��� ���̵��� �ø���.
    private void updateStageTime()
    {
        stageTime += Time.deltaTime;
        stageLevel = ((int)(stageTime / nextStageTime)) + 1;
        spawn.UpgradeStage(stageLevel);

        stageTimeTxt.text = stageTime.ToString("N2");
        StageText.text = "Stage   " + stageLevel.ToString();
    }


    // �÷��̾ ���� ������� �� �ٸ� ��ũ��Ʈ�κ��� ȣ��Ǿ� ����ġ�� �����Ѵ�.
    public void GetExp(float exp)
    {
        currentEXP += exp;
        if (currentEXP >= levelUp)
        {
            levelUpPanel();
            levelUp *= 1.5f;
        }
        UpdateUI();
    }

    // �÷��̾ ���� ������� �� �ٸ� ��ũ��Ʈ�κ��� ȣ��Ǿ� ų���� �ø���.
    public void kill()
    {
        killCount += 1;
        killCountText.text = killCount.ToString();
    }

    // �÷��̾ �����κ��� �������� ���� ��� ȣ��ȴ�.
    // ������ �ٸ� �������� �����Ѵ�.
    public void GetDamaged(float damage)
    {
        currentHP -= damage;

        if (currentHP > 0)
        {
            StartCoroutine("activeGracePeriod");    // �ڷ�ƾ�� ����� �����ð��� �ߵ��Ѵ�.
        }
        else
        {
            playerDie();
        }
    }

    /**
     * Coroutine�̶�, Update()�� �ۿ����� ������Ʈ�� �ʿ��� ��Ȳ���� ���δ�.
     * �Ʒ��� �ڵ带 ���� Update()�� ó�� ���� �����ϴ� ���� �ƴ�,
     * ������ ���� �ð����� ���߰� �ٽ� �۵��� �� �ִ� Ư�� ������ �ο��� �� �ִ�.
     * �ڷ�ƾ�� IEnumerator�̶�� ��ȯ������ �����Ѵ�.
     * yeild return�� �Լ� ���ο� �ʼ��� �����ؾ��Ѵ�.
     */
    IEnumerator activeGracePeriod()
    {
        gameObject.layer = 11;
        myHP.text = currentHP.ToString();

        int countTime = 0;

        // �÷��̾� �ǰݽ� ������ ������ �ǰݴ������� �������� ǥ�����ش�.
        while (countTime < gracePeriod * 10)
        {
            if (countTime % 2 == 0)
                playerSpriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                playerSpriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.1f);  // 0.1�ʰ� ������ �Ʒ��� �ڵ尡 ����ȴ�.
            countTime++;
        }

        playerSpriteRenderer.color = new Color32(255, 255, 255, 255);

        // �����ð� ����
        gameObject.layer = 10;
        yield return null;      // ���� �����ӿ� ����ȴ�.
    }

    private void playerDie()
    {
        dieStageTime.text = stageTime.ToString("N2");
        dieStageLevel.text = "Stage   "+ stageLevel.ToString();
        Time.timeScale = 0;             // ���� �Ͻ�����
        RestartPanel.SetActive(true);
        gameObject.SetActive(false);
    }



    // �������� �Ǿ��� ��� levelupPanel�� Ȱ��ȭ�� �÷��̾� ������ �ø� �� �ֵ��� �Ѵ�.
    private void levelUpPanel()
    {
        Time.timeScale = 0;             // ���� �Ͻ�����
        levelupPanel.SetActive(true);
    }

    // �������� ����Ѵ�.
    public void LevelUpDamage()
    {
        playerDamage += 20f;

        if (playerDamage >= 240)
        {
            currentWeapon = crossed;
            attackSpeed = 350;
        }
        else if (playerDamage >= 160)
        {
            currentWeapon = charged;
            attackSpeed = 300;
        }
        levelupPanel.SetActive(false);
        UpdateUI();
        Time.timeScale = 1;
    }

    public void LevelUpHP()
    {
        currentHP += 50;
        levelupPanel.SetActive(false);
        UpdateUI();
        Time.timeScale = 1;
    }

    public void LevelUpCoolTime()
    {
        if (coolTime <= 0.95f)
        {
            coolTime += 0.05f;
            levelupPanel.SetActive(false);
            Time.timeScale = 1;

            if (coolTime >= 0.95f)
                CoolTimeText.text = "MAX!";
        }
    }

    private void UpdateUI()
    {
        myHP.text = currentHP.ToString("N0");
        myDamage.text = (playerDamage.ToString("N0") + "%");
        myLevel.text = ((currentEXP / levelUp) * 100).ToString("N0") + "%";
    }

    private void playerMovement()
    {
        direction = Vector3.zero;
        // direction�� �÷��̾��� ������ Vector3�� �Է��Ѵ�.
        direction.x = Input.acceleration.x;
        direction.y = Input.acceleration.y;
        direction.Normalize();  // ������ ��� ���̰� 1�̾�� �̵� �ӵ��� �����ϱ⿡ ����ȭ ���� ����

        // �÷��̾ �ٶ󺸴� ���⿡ ���� ȸ����Ų��.
        // ����, �÷��̾� ũ�⸦ 1.5�� Ű����.
        if (direction.x < 0)
            transform.localScale = new Vector2(1, 1) / 1.5f;
        else if (direction.x >= 0)
            transform.localScale = new Vector2(-1, 1) / 1.5f;

        playerRigidBody.velocity = new Vector3(direction.x * playerMovementSpeed, direction.y * playerMovementSpeed, 0) * Time.deltaTime;
    }


    private void playerAttack()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 1)
        {
            direction.Normalize();  // ���� ���������� �ѹ� �� ����ȭ ���� ����
            // �÷��̾ �ٶ󺸴� z�� ������ �ٶ󺸴� ������ ��ũź��Ʈ(atan)�� �̿��� ���Ѵ�.
            float z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // �߻�ü ���� ��ġ ������ ���� ����
            Vector3 playerPos = new Vector3(transform.position.x , (transform.position.y - 0.3f), transform.position.z);

            // Instantitate(������ ��ü, ������ ��ġ, ��ü�� ����)
            GameObject attack = Instantiate(currentWeapon, playerPos, transform.rotation);
            attack.transform.rotation = Quaternion.Euler(0, 0, z);
            attack.GetComponent<Attack_Controller>().damageSetUP(playerDamage, currentWeapon);
            Rigidbody2D attackRigid = attack.GetComponent<Rigidbody2D>();
            attackRigid.AddForce(new Vector2(direction.x * attackSpeed, direction.y * attackSpeed));

            currentTime = coolTime;
        }
    }

    public void ActiveHitSound()
    {
        hitSound.Play();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;             // ���� ���
    }

    public void QuitGame()
    {
        /*
         * ���μ����� ������ ������� �ʾ� Application.Quit()�� ������ �ȵȴ�.
         * ����, UnityEditorApplication�� �÷��� ���¸� ����� ������ȯ �Ͽ� ���Ḧ Ȱ��ȭ�����ش�.
         */
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}