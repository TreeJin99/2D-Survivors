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

    public float gracePeriod;           // 피격시 무적 지속시간
    public float playerHP;
    public float playerDamage;
    public float playerMovementSpeed;
    public float coolTime;
    public int nextStageTime;           // 다음 스테이지로 넘어가는 시간
    public Text dieStageTime;
    public Text dieStageLevel;


    private GameObject currentWeapon;   // 현재 무기 종류
    private Spawn_Controller spawn;     // Spawn_Controller의 INSTANCE
    private Vector3 direction;          // 플레이어가 바라보는 방향
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
    private int stageLevel;             // 현재 스테이지 


    private void Awake()
    {
        PLAYER_INSTANCE = this;

        // Player 오브젝트 선언
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        // Canvas 선언
        myHP = GameObject.Find("MyHP").GetComponent<Text>();
        myDamage = GameObject.Find("MyDamage").GetComponent<Text>();
        myLevel = GameObject.Find("MyLevel").GetComponent<Text>();
        stageTimeTxt = GameObject.Find("Survive Time").GetComponent<Text>();
        killCountText = GameObject.Find("MyKill").GetComponent<Text>();
        StageText = GameObject.Find("StageLevel").GetComponent<Text>();
    }

    private void Start()
    {
        startSetUp();       // 초기 설정
        UpdateUI();         // Canvas 초기화
    }

    private void Update()
    {
        updateStageTime();  // 현재 스테이지의 시간
        playerMovement();   // 플레이어 이동 메소드
        playerAttack();     // 플레이어 공격 메소드
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

    // 현재 게임의 stage를 관여한다.
    // stage를 실시간으로 Spawn_Controller에 넘겨줘 스테이지가 올라갈수록 난이도를 올린다.
    private void updateStageTime()
    {
        stageTime += Time.deltaTime;
        stageLevel = ((int)(stageTime / nextStageTime)) + 1;
        spawn.UpgradeStage(stageLevel);

        stageTimeTxt.text = stageTime.ToString("N2");
        StageText.text = "Stage   " + stageLevel.ToString();
    }


    // 플레이어가 적을 사냥했을 때 다른 스크립트로부터 호출되어 경험치를 주입한다.
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

    // 플레이어가 적을 사냥했을 때 다른 스크립트로부터 호출되어 킬수를 올린다.
    public void kill()
    {
        killCount += 1;
        killCountText.text = killCount.ToString();
    }

    // 플레이어가 적으로부터 데미지를 받을 경우 호출된다.
    // 적마다 다른 데미지를 주입한다.
    public void GetDamaged(float damage)
    {
        currentHP -= damage;

        if (currentHP > 0)
        {
            StartCoroutine("activeGracePeriod");    // 코루틴을 사용해 무적시간을 발동한다.
        }
        else
        {
            playerDie();
        }
    }

    /**
     * Coroutine이란, Update()문 밖에서도 업데이트가 필요한 상황에서 쓰인다.
     * 아래의 코드를 통해 Update()문 처럼 당장 실행하는 것이 아닌,
     * 지정된 일정 시간동안 멈추고 다시 작동할 수 있는 특정 조건을 부여할 수 있다.
     * 코루틴은 IEnumerator이라는 반환형으로 시작한다.
     * yeild return이 함수 내부에 필수로 존재해야한다.
     */
    IEnumerator activeGracePeriod()
    {
        gameObject.layer = 11;
        myHP.text = currentHP.ToString();

        int countTime = 0;

        // 플레이어 피격시 색깔을 깜빡여 피격당했음을 동적으로 표현해준다.
        while (countTime < gracePeriod * 10)
        {
            if (countTime % 2 == 0)
                playerSpriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                playerSpriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.1f);  // 0.1초가 지나면 아래의 코드가 실행된다.
            countTime++;
        }

        playerSpriteRenderer.color = new Color32(255, 255, 255, 255);

        // 무적시간 종료
        gameObject.layer = 10;
        yield return null;      // 다음 프레임에 실행된다.
    }

    private void playerDie()
    {
        dieStageTime.text = stageTime.ToString("N2");
        dieStageLevel.text = "Stage   "+ stageLevel.ToString();
        Time.timeScale = 0;             // 게임 일시정지
        RestartPanel.SetActive(true);
        gameObject.SetActive(false);
    }



    // 레벨업이 되었을 경우 levelupPanel을 활성화해 플레이어 스탯을 올릴 수 있도록 한다.
    private void levelUpPanel()
    {
        Time.timeScale = 0;             // 게임 일시정지
        levelupPanel.SetActive(true);
    }

    // 데미지가 상승한다.
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
        // direction에 플레이어의 방향을 Vector3로 입력한다.
        direction.x = Input.acceleration.x;
        direction.y = Input.acceleration.y;
        direction.Normalize();  // 벡터의 모든 길이가 1이어야 이동 속도가 일정하기에 정규화 과정 진행

        // 플레이어가 바라보는 방향에 맞춰 회전시킨다.
        // 또한, 플레이어 크기를 1.5배 키웠다.
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
            direction.Normalize();  // 위와 마찬가지로 한번 더 정규화 과정 진행
            // 플레이어가 바라보는 z값 각도를 바라보는 방향의 아크탄젠트(atan)를 이용해 구한다.
            float z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // 발사체 생성 위치 조정을 위한 선언
            Vector3 playerPos = new Vector3(transform.position.x , (transform.position.y - 0.3f), transform.position.z);

            // Instantitate(생성될 물체, 생성될 위치, 물체의 방향)
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
        Time.timeScale = 1;             // 게임 재게
    }

    public void QuitGame()
    {
        /*
         * 프로세스가 완전히 종료되지 않아 Application.Quit()가 적용이 안된다.
         * 따라서, UnityEditorApplication의 플레이 상태를 종료로 강제전환 하여 종료를 활성화시켜준다.
         */
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}