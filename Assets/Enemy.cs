using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    Animator m_bear = null;

    GameObject player;

    Player m_player;

    private ItemManager m_Item;

    bool isBearHit = false;

    bool isSee = false;


    //bool hasAnim = false;

    //AnimatorClipInfo clipPlayerinfo;
    //Animator playerAnim = null;
    //string playeranimname;

    byte bearhp = 5;

    NavMeshAgent navMeshAgent;



    void Awake()
    {
        m_bear = GetComponent<Animator>();
        player = GameObject.Find("boy");
        m_player = player.GetComponent<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_Item = GetComponent<ItemManager>();
    }

    //void Update()
    //{
    //    //if (bearhp > 0 && isBearHit == false) { Target(); }

    //    //if (hasAnim == false)
    //    //{
    //    //    BearRunAnimStop();
    //    //}
    //}

    //void FixedUpdate()
    //{
    //    if (bearhp > 0 && isBearHit == false) { Target(); }
    //}

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
        //m_bear.SetBool("Run Forward", true);
        if (isBearHit == false && bearhp > 0)
        {
            //BearRunAnimGo();
            m_bear.SetBool("Detection", true);
            navMeshAgent.destination = player.transform.position;
        }
        //BearRunAnimGo();
    }

    public void OnDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            //if (bearhp > 0 && isBearHit == false) { Target(); }
            isSee = true;
            Target();
            //hasAnim = true;
        }
        //else
        //{
        //    hasAnim = false;
        //}

    }


    public void OutDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isSee = false;
            //if (bearhp > 0 && isBearHit == false) { Target(); }
            BearRunAnimStop();
            //hasAnim = true;
        }
        //if (hasAnim)
        //{
        //m_bear.SetBool("Run Forward", false);
        //}
    }


    void IsBearHitTrue()
    {
        isBearHit = true;
    }

    void IsBearHitFalse()
    {
        isBearHit = false;
    }

    void BearRunAnimGo()
    {
        m_bear.SetBool("Detection", true);
    }

    public void BearRunAnimStop()
    {
        m_bear.SetBool("Detection", false);
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
        if (m_player.isAtack == true && bearhp != 0)
        {
            if (other.CompareTag("weapon"))
            {
                isBearHit = true;
                BearStop();
                //m_bear.SetBool("Run Forward", false);
                BearRunAnimStop();
                bearhp--;
                if (bearhp != 0)
                {
                    //BearStop();
                    //m_bear.SetBool("Death", true);
                    //Destroy(gameObject, 3.2f);

                    m_bear.SetTrigger("Get Hit Front");
                    if (isSee == true)
                    {
                        Invoke("BearGo", 1.2f);
                        Invoke("BearRunAnimGo", 1.2f);
                    }
                }
                else {
                    //m_bear.SetTrigger("Get Hit Front");
                    //Invoke("BearGo", 1.0f);
                    BearDeth();
                }
            }
            m_player.isAtack = false;
            Invoke("IsBearHitFalse", 1.2f);
        }

    }

    void BearDeth()
    {
        BearStop();
        m_bear.SetBool("Death", true);
        Destroy(gameObject, 3.2f);
        m_Item.ItemDrop();
    }


}
