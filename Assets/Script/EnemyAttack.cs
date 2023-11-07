using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Enemy m_enemy;
    Player m_Player;

    void Awake()
    {
        m_enemy = GetComponent<Enemy>();
        m_Player = GetComponent<Player>();
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit");
            m_Player.TakeDamage();
        }
    }


    }
