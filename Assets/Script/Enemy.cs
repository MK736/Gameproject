using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DestinationController))]

public class Enemy : MonoBehaviour
{
    Animator m_bear = null;

    GameObject player;

    Player m_player;

    private ItemManager m_Item;

    bool isBearHit = false;

    bool isSee = false;

    public bool isAttack = false;

    public Vector3[] wayPoints = new Vector3[3];
    public enum EnemyAiState
    {
        WAIT,
        MOVE,
        ATTACK,
        MOVEANDATACK,
        IDLE
    }
    public EnemyAiState aiState = EnemyAiState.WAIT;
    public EnemyAiState nextState;

    byte bearhp = 5;

    [SerializeField]
    private BoxCollider BoxCollider;

    private NavMeshAgent navMeshAgent = null;

    [SerializeField] private DestinationController destinationController;

    void Awake()
    {
        m_bear = GetComponent<Animator>();
        player = GameObject.Find("boy");
        m_player = player.GetComponent<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_Item = GetComponent<ItemManager>();
        destinationController = GetComponent<DestinationController>();
        navMeshAgent.SetDestination(destinationController.GetDestination());
    }

    private void Update()
    {
        UpdateAI();
    }

    public void SetAi()
    {
        InitAi();
        AiMainRoutine();
        aiState = nextState;
    }
    void InitAi()
    {
        nextState = EnemyAiState.MOVE;
    }

    void AiMainRoutine()
    {
        if (isAttack)
        {
            nextState = EnemyAiState.ATTACK;
            //Debug.Log("�U��");
        }
        else if (isSee)
        {
            nextState = EnemyAiState.MOVEANDATACK;
            //Debug.Log("����");
        }
        else
        {
            nextState = EnemyAiState.MOVE;
            //Debug.Log("������");
        }
    }
    void UpdateAI()
    {
        SetAi();
        switch (aiState)
        {
            case EnemyAiState.WAIT:
                Wait();
            break;
            case EnemyAiState.MOVEANDATACK:
                MoveAndAtack();
            break;
            case EnemyAiState.ATTACK:
                Attack();
                break;
            case EnemyAiState.MOVE:
                Move();
            break;
        }
    }

    void Wait()
    {
        //BearRunAnimStop();
        navMeshAgent.isStopped = true;

    }
    void Move()
    {
        BearRunAnimGo();
        BearGo();

        if (Vector3.Distance(transform.position, destinationController.GetDestination()) < 10.0f)
        {
            destinationController.CreateDetination();
            navMeshAgent.SetDestination(destinationController.GetDestination());
        }

    }
    void MoveAndAtack()
    {
        if (isBearHit == false && bearhp > 0)
        {
            m_bear.SetBool("Detection", true);
            navMeshAgent.destination = player.transform.position;
        }
        //if(Vector3.Distance(transform.position, m_player.transform.position) < 20.0f)
        //{
        //    Debug.Log("Attack");
        //    nextState = EnemyAiState.ATTACK;
        //}
        //Debug.Log("����");
    }

    void Attack()
    {
        //Debug.Log("�U��");
        navMeshAgent.isStopped = true;
        BearRunAnimStop();
        BearAtackAnim();
        AtackStart();
    }

    void AtackStart()
    {
        BoxCollider.enabled = true;
    }

    public void AtackEnd()
    {
        BoxCollider.enabled = false;
    }

    public void OnAttack(Collider collider)
    {
        if (collider.CompareTag("Player")&& bearhp > 0)
        {
            isAttack = true;
        }
    }
    public void OutAttack(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            BearAtackAnimStop();
            isAttack = false;
        }
    }
    public void OnDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            navMeshAgent.destination = collider.transform.position;
        }
    }
    public void OutDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            destinationController.CreateDetination();
            navMeshAgent.SetDestination(destinationController.GetDestination());
        }
    }
    void IsBearHitTrue()
    {
        isBearHit = true;
    }

    void IsBearHitFalse()
    {
        isBearHit = false;
    }

    void BearAtackAnim()
    {
        m_bear.SetBool("Atack1", true);
    }
    void BearAtackAnimStop()
    {
        m_bear.SetBool("Atack1", false);
    }

    void BearRunAnimGo()
    {
        m_bear.SetBool("Detection", true);
    }

    public void BearRunAnimStop()
    {
        m_bear.SetBool("Detection", false);
    }
    void BearGo()
    {
        navMeshAgent.isStopped = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (m_player.isAtack == true && bearhp != 0)
        {
            if (other.CompareTag("weapon"))
            {
                isBearHit = true;
                navMeshAgent.isStopped = true;
                BearRunAnimStop();
                BearAtackAnimStop();
                bearhp--;
                if (isSee == false)
                {
                    navMeshAgent.destination = player.transform.position;
                }
                if (bearhp != 0)
                {
                    m_bear.SetTrigger("Get Hit Front");
                    if (isSee == true)
                    {
                        Invoke("BearGo", 1.2f);
                        Invoke("BearRunAnimGo", 1.2f);
                    }
                }
                else {
                    BearDeth();
                }
            }
            m_player.isAtack = false;
            Invoke("IsBearHitFalse", 1.2f);
        }
    }
    void BearDeth()
    {
        navMeshAgent.isStopped = true;
        m_bear.SetBool("Death", true);
        Destroy(gameObject, 3.2f);
        m_Item.ItemDrop();
    }
}
