using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    GameObject player;
    Player m_Player;

    private PlayerGage playerGage;

    private void Awake()
    {
        player = GameObject.Find("boy");
        m_Player = player.GetComponent<Player>();

        playerGage = GameObject.FindObjectOfType<PlayerGage>();
        playerGage.SetPlayer(m_Player);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            if (m_Player.g_PlayerHP > 50)
            {
                playerGage.GaugeUp(1.0f);
                m_Player.g_PlayerHP = 100;
            }
            else
            {
                playerGage.GaugeUp(0.5f);
                m_Player.g_PlayerHP += 50;
            }
        }

    }


}
