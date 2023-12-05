using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(DestinationController))]

public class Enemy : MonoBehaviour
{
    Animator m_bear = null;

    GameObject player;

    Player m_player;

    private ItemManager m_Item;

    bool isBearHit = false;

    bool isSee = false;

    public bool isAttack = false;

    //public Vector3[] wayPoints = new Vector3[3];
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
    public Transform center;

    Vector3 pos;
    [SerializeField] float radius = 490;

    [SerializeField] float waitTime = 2;

    [SerializeField] float time = 0;

    public int enemyHp = 2;

    public int atackPower = 10;

    BattleManager m_BattleManager = null;

    public BoxCollider m_BoxCollider = null;


     public BoxCollider AtackBoxCollider;

    private NavMeshAgent navMeshAgent = null;

    //[SerializeField] private DestinationController destinationController;

    void Awake()
    {
        m_bear = GetComponent<Animator>();
        player = GameObject.Find("player1");
        m_player = player.GetComponent<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        m_Item = GetComponent<ItemManager>();
        //destinationController = GetComponent<DestinationController>();
        //navMeshAgent.SetDestination(destinationController.GetDestination());
        m_BattleManager = GetComponent<BattleManager>();
        //navMeshAgent.autoBraking = false;
        //navMeshAgent.updateRotation = false;
        AtackEnd();
        GotoNextPoint();
    }

    private void Update()
    {
        UpdateAI();
        //Debug.Log(enemyHp);
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
            //Debug.Log("çUåÇ");
        }
        else if (isSee)
        {
            nextState = EnemyAiState.MOVEANDATACK;
            //Debug.Log("î≠å©");
        }
        else
        {
            nextState = EnemyAiState.MOVE;
            //Debug.Log("ì¶ÇµÇΩ");
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
        //BearRunAnimGo();
        //BearGo();

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            m_bear.SetBool("Detection", false);
            StopHere();
        }


        //if (Vector3.Distance(transform.position, destinationController.GetDestination()) < 10.0f)
        //{
        //    destinationController.CreateDetination();
        //    navMeshAgent.SetDestination(destinationController.GetDestination());

        //}

    }
    void MoveAndAtack()
    {
        if (isBearHit == false && enemyHp > 0)
        {
            m_bear.SetBool("Detection", true);
            navMeshAgent.destination = player.transform.position;
        }
        //if(Vector3.Distance(transform.position, m_player.transform.position) < 20.0f)
        //{
        //    Debug.Log("Attack");
        //    nextState = EnemyAiState.ATTACK;
        //}
        //Debug.Log("î≠å©");
    }

    void Attack()
    {
        if (m_player.isDead == false)
        {
            navMeshAgent.isStopped = true;
            BearRunAnimStop();
            BearAtackAnim();
            AtackStart();
            //m_player.TakeDamage();
        }
        else
        {
            m_bear.SetBool("Idle", true);
        }
    }

    void GotoNextPoint()
    {
        navMeshAgent.isStopped = false;
        //BearRunAnimGo();
        m_bear.SetBool("Detection", true);


        float posX = Random.Range(-1 * radius, radius);
        float posZ = Random.Range(-1 * radius, radius);

        pos = center.position;
        pos.x += posX;
        pos.z += posZ;

        Vector3 direction = new Vector3(pos.x, transform.position.y, pos.z);

        Quaternion rotation = Quaternion.LookRotation(direction - transform.position, Vector3.up);

        transform.rotation = rotation;

        navMeshAgent.destination = pos;
    }

    void StopHere()
    {
        navMeshAgent.isStopped = true;
        m_bear.SetBool("Detection", false);

        time += Time.deltaTime;

        if (time > waitTime)
        {
            m_bear.SetBool("Detection", true);
            GotoNextPoint();
            time = 0;
        }
    }

    void AtackStart()
    {
        AtackBoxCollider.enabled = true;
    }

    public void AtackEnd()
    {
        AtackBoxCollider.enabled = false;
    }

    public void OnAttack(Collider collider)
    {
        if (collider.CompareTag("Player")&& enemyHp > 0)
        {
            isAttack = true;

        }
    }
    public void OutAttack(Collider collider)
    {
        AtackEnd();
        if (collider.CompareTag("Player"))
        {
            BearAtackAnimStop();
            //AtackEnd();
            isAttack = false;
        }
    }
    public void OnDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            //navMeshAgent.destination = collider.transform.position;
            navMeshAgent.destination = player.transform.position;
        }
    }
    public void OutDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            //destinationController.CreateDetination();
            //navMeshAgent.SetDestination(destinationController.GetDestination());
            GotoNextPoint();
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

    void OnTriggerExit(Collider other)
    {
        IDamageable damageable = GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damagee(other, m_player.PlayerPower);
            damageable.Death(other, enemyHp);
        }

        if (enemyHp != 0)
        {
            m_player.WeaponColOff();
            navMeshAgent.isStopped = true;
            BearRunAnimStop();
            BearAtackAnimStop();
            navMeshAgent.destination = player.transform.position;

                m_bear.SetTrigger("Get Hit Front");
                Invoke("BearGo", 1.2f);
                Invoke("BearRunAnimGo", 1.2f);
            Invoke("IsBearHitFalse", 1.2f);

        }
    }
    public void Deth()
    {
        AtackEnd();
        navMeshAgent.isStopped = true;
        m_bear.SetBool("Death", true);
        Destroy(gameObject, 3.2f);
        m_Item.ItemDrop();
    }
}
