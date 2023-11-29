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

    public int enemyHp = 2;

    public int atackPower = 10;

    BattleManager m_BattleManager = null;

    [SerializeField]
    public BoxCollider m_BoxCollider = null;


    [SerializeField]
     BoxCollider BoxCollider;

    private NavMeshAgent navMeshAgent = null;

    [SerializeField] private DestinationController destinationController;

    void Awake()
    {
        m_bear = GetComponent<Animator>();
        player = GameObject.Find("player1");
        m_player = player.GetComponent<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_Item = GetComponent<ItemManager>();
        destinationController = GetComponent<DestinationController>();
        navMeshAgent.SetDestination(destinationController.GetDestination());
        m_BattleManager = GetComponent<BattleManager>();
    }

    private void Update()
    {
        UpdateAI();
        Debug.Log(enemyHp);
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
        }
        else
        {
            m_bear.SetBool("Idle", true);
        }
        //else
        //{
        //    destinationController.CreateDetination();
        //    navMeshAgent.SetDestination(destinationController.GetDestination());
        //}
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
        if (collider.CompareTag("Player")&& enemyHp > 0)
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

    void OnTriggerExit(Collider other)
    {
        //m_BattleManager.Damage(other/*, enemyHp, m_player.PlayerPower*/);

        IDamageable damageable = GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damagee(other, enemyHp, m_player.PlayerPower);
            damageable.Death(other, enemyHp);
        }
        //if (other.CompareTag("weapon"))
        //{
        //    Debug.Log("åFÉ_ÉÅÅ[ÉW");
        //}
            //m_BattleManager.Atack(0, enemyHp);

        if (/*m_BattleManager.EnemyDamage == true &&*/ enemyHp != 0)
        {
            m_player.WeaponColOff();
            navMeshAgent.isStopped = true;
            BearRunAnimStop();
            BearAtackAnimStop();
            //m_BattleManager.HpDown(enemyHp, m_player.PlayerPower);
            //enemyHp--;
            //m_player.WeaponColOff();
            navMeshAgent.destination = player.transform.position;

                m_bear.SetTrigger("Get Hit Front");
                Invoke("BearGo", 1.2f);
                Invoke("BearRunAnimGo", 1.2f);
            //m_player.isAtack = false;
            Invoke("IsBearHitFalse", 1.2f);

        }

        //if (m_player.isAtack == true && enemyHp != 0)
        //{
        //    if (other.CompareTag("weapon"))
        //    {
        //        isBearHit = true;
        //        //navMeshAgent.isStopped = true;
        //        //BearRunAnimStop();
        //        //BearAtackAnimStop();
        //        //enemyHp = m_BattleManager.HpDown(enemyHp, 1);
        //        //enemyHp--;
        //        //if (isSee == false)
        //        //{
        //        //    navMeshAgent.destination = player.transform.position;
        //        //}
        //        if (enemyHp != 0)
        //        {
        //            m_bear.SetTrigger("Get Hit Front");
        //            if (isSee == true)
        //            {
        //                Invoke("BearGo", 1.2f);
        //                Invoke("BearRunAnimGo", 1.2f);
        //            }
        //        }
        //        else {
        //            Deth();
        //        }
        //    }
        //    m_player.isAtack = false;
        //    Invoke("IsBearHitFalse", 1.2f);
        //}
    }
    public void Deth()
    {
        navMeshAgent.isStopped = true;
        m_bear.SetBool("Death", true);
        Destroy(gameObject, 3.2f);
        m_Item.ItemDrop();
    }
}
