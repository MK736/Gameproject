using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    GameObject player;
    Player m_Player;
    BattleManager m_BattleManager;

    private PlayerGage playerGage;

    private void Awake()
    {
        player = GameObject.Find("player1");
        m_Player = player.GetComponent<Player>();

        m_BattleManager = player.GetComponent<BattleManager>();

        playerGage = GameObject.FindObjectOfType<PlayerGage>();
        playerGage.SetPlayer(m_Player);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            //m_Player.g_PlayerHP = m_BattleManager.HpUp(m_Player.g_PlayerHP, 50);
        }

    }


}
