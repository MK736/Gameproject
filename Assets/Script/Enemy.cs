using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    Animator m_bear = null;

    GameObject player;

    Player m_player;

    private ItemManager m_Item;

    bool isBearHit = false;

    bool isSee = false;

    bool isWait = true;

    private float speed = 5.0f;
    private float rotationSmooth = 1f;

    private Vector3 targetPosition;

    private float changeTargetSqrDistance = 10.0f;
    [SerializeField]
    private Transform leftup;
    [SerializeField]
    private Transform rightdown;

    public Vector3[] wayPoints = new Vector3[3];
    private int currentRoot;
    private int Mode;
    public Transform g_Player;
    public Transform g_EnemyPos;


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

    NavMeshAgent navMeshAgent;

    void Awake()
    {
        m_bear = GetComponent<Animator>();
        player = GameObject.Find("boy");
        m_player = player.GetComponent<Player>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_Item = GetComponent<ItemManager>();

    }

    private void Start()
    {
        navMeshAgent.SetDestination(GetRandomPositionOnLevel1());
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
        //navMeshAgent.SetDestination(GetRandomPositionOnLevel1());
        nextState = EnemyAiState.MOVE;
    }
    //ì¶Ç∞êÿÇ¡ÇΩÇ†Ç∆èÍèäÇÕåàÇ‹Ç¡ÇƒÇ¢ÇÈÇ™ìÆÇ©Ç»Ç¢
    //èÍèäÇàÍâÒÇæÇØåàÇﬂÇÈñ⁄ìIínÇ…Ç¬ÇØÇŒçƒìxèÍèäÇåàÇﬂÇÈ
    //èÍèäåàÇﬂÇÕç≈èâÇ∆ì¶Ç∞êÿÇ¡ÇΩå„ÇæÇØ

    void AiMainRoutine()
    {
        //if(isWait)
        //{
        //    nextState = EnemyAiState.WAIT;
        //    isWait = false;
        //    return;
        //}
        if(isSee)
        {
            nextState = EnemyAiState.MOVEANDATACK;
            Debug.Log("î≠å©");
        }
        else
        {
            targetPosition = GetRandomPositionOnLevel1();
            nextState = EnemyAiState.MOVE;
            Debug.Log("ì¶ÇµÇΩ");
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

            case EnemyAiState.MOVE:
                Move();
            break;

            case EnemyAiState.MOVEANDATACK:
                MoveAndAtack();
            break;
        }
    }

    void Wait()
    {
        //BearRunAnimStop();
        //BearStop();

    }
    void Move()
    {
        //float sqrDistanceToTarget = Vector3.SqrMagnitude(transform.position - targetPosition);
        //if (sqrDistanceToTarget < changeTargetSqrDistance)
        //{
        //    navMeshAgent.SetDestination(GetRandomPositionOnLevel1());
        //}

        //Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

        BearRunAnimGo();
        BearGo();
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //Debug.Log("Serch");

        Vector3 pos = wayPoints[currentRoot];
        float distance = Vector3.Distance(g_EnemyPos.position, g_Player.transform.position);

        if (distance > 25) {
            Mode = 0;
        }

        if (distance < 25)
        {
            Mode = 1;
        }

        switch(Mode)
        {
            case 0:
                if (Vector3.Distance (transform.position, pos) < 1f)
                {
                    currentRoot += 1;
                    if (currentRoot > wayPoints.Length - 1)
                    {
                        currentRoot = 0;
                    }
                }
                GetComponent<NavMeshAgent>().SetDestination(pos);
                break;

                case 1:
                navMeshAgent.destination = player.transform.position;
                break;
        }

    }

    void MoveAndAtack()
    {
        if (isBearHit == false && bearhp > 0)
        {
            //BearRunAnimGo();
            m_bear.SetBool("Detection", true);
            navMeshAgent.destination = player.transform.position;
        }
        Debug.Log("î≠å©");
    }
    public Vector3 GetRandomPositionOnLevel1()
    {
        return new Vector3(Random.Range(leftup.position.x,rightdown.position.x), 0, Random.Range(rightdown.position.z, leftup.position.z));
    }

    public void OnDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isSee = true;
        }
    }

    public void OutDetectObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isSee = false;
            //navMeshAgent.destination = new Vector3(Random.Range(leftup.position.x, rightdown.position.x), 0, Random.Range(rightdown.position.z, leftup.position.z));
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
        if (m_player.isAtack == true && bearhp != 0)
        {
            if (other.CompareTag("weapon"))
            {
                isBearHit = true;
                BearStop();
                BearRunAnimStop();
                bearhp--;
                if (isSee == false)
                {
                    transform.Rotate(new Vector3(180, 0, 0));
                    BearGo();
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
        BearStop();
        m_bear.SetBool("Death", true);
        Destroy(gameObject, 3.2f);
        m_Item.ItemDrop();
    }
}
