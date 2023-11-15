using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    GameObject player;
    Player m_Player;

    private void Awake()
    {
        player = GameObject.Find("boy");
        m_Player = player.GetComponent<Player>();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            m_Player.g_PlayerHP += 30;
        }

    }


}
