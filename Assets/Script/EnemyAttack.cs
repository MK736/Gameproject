using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //GameObject player;
    //Player m_Player;

    GameObject enemy;
    Enemy m_Enemy;

    void Start()
    {
        //player = GameObject.Find("player1");
        //m_Player = player.GetComponent<Player>();
        enemy = GameObject.Find("Enemy_Bear");
        m_Enemy = enemy.GetComponent<Enemy>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MainManager.instance.m_Php > 0)
            {
                MainManager.instance.TakeDamage(m_Enemy);
                Debug.Log("HpDown");
            }
            else
            {
                MainManager.instance.isDead = true;
            }
            //Debug.Log("Hit");
        }
    }


    }
