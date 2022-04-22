using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public float bonusExp;

    private Player_Controller player;
    private Animator open;

    private void Awake()
    {
        open = GetComponent<Animator>();
    }

    private void Start()
    {
        startSetUp();
    }

    private void startSetUp()
    {
        player = Player_Controller.PLAYER_INSTANCE;
        bonusExp = 500;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isOpen();
        }
    }
    
    /* 플레이어와 충돌할 경우 열린다.
     * 플레이어와 재충돌을 방지하기 위해 layer를 11로 이동한다.
     */
    private void isOpen()
    {
        gameObject.layer = 11;
        player.GetExp(bonusExp * (player.getStageLevel() * 0.5f));
        open.SetTrigger("isOpen");
        Destroy(gameObject, 3);
    }
}
