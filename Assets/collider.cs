using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class collider : MonoBehaviour
{
    Animator m_bear = null;

    GameObject player;

    Player m_player;

    //AnimatorClipInfo clipPlayerinfo;

    //Animator playerAnim = null;

    //string playeranimname;

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


        if (bearhp > 0)
        {
            Target();
        }

    }

    void Target()
    {

        //if (bearhp == 0)
        //{
        //    BearStop();
        //}
        //else
        //{
        //    navMeshAgent.destination = player.transform.position;
        //}
        m_bear.SetBool("Run Forward", true);
        navMeshAgent.destination = player.transform.position;
    }

    void BearStop()
    {
        navMeshAgent.isStopped = true;
    }

    void BearGo()
    {
        navMeshAgent.isStopped = false;
    }


    void OnTriggerEnter(Collider other)
    {
        //playerAnim = m_player.m_PlayerAnimmator;

        //clipPlayerinfo = playerAnim.GetCurrentAnimatorClipInfo(0)[0];

        //playeranimname = clipPlayerinfo.clip.name;

        if (m_player.isAtack == true)
        {
            if (other.CompareTag("weapon"))
            {
                m_bear.SetBool("Run Forward", false);
                bearhp--;
                BearStop();
                if (bearhp != 0)
                {
                    //BearStop();
                    //m_bear.SetBool("Death", true);
                    //Destroy(gameObject, 3.2f);

                    m_bear.SetTrigger("Get Hit Front");
                    Invoke("BearGo", 0.5f);
                }
                else {
                    //m_bear.SetTrigger("Get Hit Front");
                    //Invoke("BearGo", 1.0f);

                    BearDeth();
                }
            }
            m_player.isAtack = false;
        }

    }

    void BearDeth()
    {
        BearStop();
        m_bear.SetBool("Death", true);
        Destroy(gameObject, 3.2f);
    }


}
