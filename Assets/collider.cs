using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider : MonoBehaviour
{


    Animator m_bear = null;

    GameObject player;

    Player m_player;

    AnimatorClipInfo clipPlayerinfo;

    Animator playerAnim = null;


    // Start is called before the first frame update
    void Awake()
    {
        m_bear = GetComponent<Animator>();
        player = GameObject.Find("Women1");
        m_player = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {


    }


    void OnTriggerEnter(Collider other)
    {
        playerAnim = m_player.m_PlayerAnimmator;

        clipPlayerinfo = playerAnim.GetCurrentAnimatorClipInfo(0)[0];

        if (other.CompareTag("weapon"))
        {
            Debug.Log("hit");
            m_bear.SetBool("Death", true);
        }

    }


}
