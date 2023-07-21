using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class collider : MonoBehaviour
{


    Animator m_bear = null;

    GameObject player;

    Player m_player;

    AnimatorClipInfo clipPlayerinfo;

    Animator playerAnim = null;

    string playeranimname;

    string nowplayeranim = "RightHand@Attack01";

    byte bearhp = 5;

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Awake()
    {
        m_bear = GetComponent<Animator>();
        player = GameObject.Find("Women1");
        m_player = player.GetComponent<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Target();

    }

    void Target()
    {

        if (bearhp <= 0)
        {
            navMeshAgent.isStopped = true;
        }
        else
        {
            navMeshAgent.destination = player.transform.position;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        playerAnim = m_player.m_PlayerAnimmator;

        clipPlayerinfo = playerAnim.GetCurrentAnimatorClipInfo(0)[0];

        playeranimname = clipPlayerinfo.clip.name;

        if (playeranimname == nowplayeranim)
        {
            if (other.CompareTag("weapon"))
            {
                bearhp--;
                //Debug.Log("hit");
                navMeshAgent.isStopped = true;

                if (bearhp <= 0)
                {
                    m_bear.SetBool("Death", true);
                    Destroy(gameObject, 3.3f);
                }
                else {
                    m_bear.SetTrigger("Get Hit Front");
                    navMeshAgent.isStopped = false;
                }
            }
        }

    }


}
