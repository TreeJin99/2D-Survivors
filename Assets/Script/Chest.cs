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
    
    /* �÷��̾�� �浹�� ��� ������.
     * �÷��̾�� ���浹�� �����ϱ� ���� layer�� 11�� �̵��Ѵ�.
     */
    private void isOpen()
    {
        gameObject.layer = 11;
        player.GetExp(bonusExp * (player.getStageLevel() * 0.5f));
        open.SetTrigger("isOpen");
        Destroy(gameObject, 3);
    }
}
