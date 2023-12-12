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

    //public int enemyHp = 2;

    //public int atackPower = 10;

    //BattleManager m_BattleManager = null;

    public BoxCollider m_BoxCollider = null;


     public BoxCollider AtackBoxCollider;

    private NavMeshAgent navMeshAgent = null;

    //[SerializeField] private DestinationController destinationController;

    public Transform center;

    Vector3 pos;

    [SerializeField] float radius = 490;
    [SerializeField] float waitTime = 2;
    [SerializeField] float time = 0;

    //static public Enemy instance;


    void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this.gameObject);
        //}
        //else
        //{
        //    Destroy(this.gameObject);
        //}

        m_bear = GetComponent<Animator>();
        player = GameObject.Find("player1");
        m_player = player.GetComponent<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_Item = GetComponent<ItemManager>();
        //destinationController = GetComponent<DestinationController>();
        //navMeshAgent.SetDestination(destinationController.GetDestination());
        //m_BattleManager = GetComponent<BattleManager>();
        AtackEnd();
        GotoNextPoint();

    }

    private void Update()
    {
        UpdateAI();
        //Debug.Log(enemyHp);
        //Debug.Log(Player.instance.g_PlayerHP);
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
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            m_bear.SetBool("Detection", false);
            StopHere();
        }

        //BearRunAnimGo();
        //BearGo();

        //if (Vector3.Distance(transform.position, destinationController.GetDestination()) < 10.0f)
        //{
        //    destinationController.CreateDetination();
        //    navMeshAgent.SetDestination(destinationController.GetDestination());
        //}

    }
    void MoveAndAtack()
    {
        if (isBearHit == false && MainManager.instance.m_Ehp > 0)
        {
            m_bear.SetBool("Detection", true);
            navMeshAgent.destination = m_player.transform.position;
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
        if (MainManager.instance.isDead == false)
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
        //else
        //{
        //    destinationController.CreateDetination();
        //    navMeshAgent.SetDestination(destinationController.GetDestination());
        //}
    }


    void GotoNextPoint()
    {
        navMeshAgent.isStopped = false;
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
        if (collider.CompareTag("Player")&& MainManager.instance.m_Ehp > 0)
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
            isAttack = false;
        }
    }
    public void OnDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            navMeshAgent.destination = m_player.transform.position;
        }
    }
    public void OutDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            GotoNextPoint();
            //destinationController.CreateDetination();
            //navMeshAgent.SetDestination(destinationController.GetDestination());
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
        //IDamageable damageable = GetComponent<IDamageable>();
        //if (damageable != null)
        //{
        //    damageable.Damagee(other, Player.instance.PlayerPower);
        //    damageable.Death(other, enemyHp);
        //}
        if (other.CompareTag("weapon") && MainManager.instance.m_Ehp == 0)
        {
            Deth();
            m_BoxCollider.enabled = false;
            AtackBoxCollider.enabled = false;
        }
        else if (other.CompareTag("weapon") && MainManager.instance.m_Ehp != 0)
        {
            MainManager.instance.m_Ehp -= MainManager.instance.m_AtackPower;
            m_player.WeaponColOff();
            navMeshAgent.isStopped = true;
            BearRunAnimStop();
            BearAtackAnimStop();
            navMeshAgent.destination = m_player.transform.position;

            m_bear.SetTrigger("Get Hit Front");
            Invoke("BearGo", 1.2f);
            Invoke("BearRunAnimGo", 1.2f);
            Invoke("IsBearHitFalse", 1.2f);
        }

        //if (enemyHp != 0)
        //{
        //    Player.instance.WeaponColOff();
        //    navMeshAgent.isStopped = true;
        //    BearRunAnimStop();
        //    BearAtackAnimStop();
        //    navMeshAgent.destination = Player.instance.transform.position;

        //        m_bear.SetTrigger("Get Hit Front");
        //        Invoke("BearGo", 1.2f);
        //        Invoke("BearRunAnimGo", 1.2f);
        //    Invoke("IsBearHitFalse", 1.2f);

        //}
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
