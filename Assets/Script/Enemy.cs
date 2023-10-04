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

    private float speed = 5.0f;
    private float rotationSmooth = 1f;

    private Vector3 targetPosition;

    private float changeTargetSqrDistance = 10.0f;
    [SerializeField]
    private Transform leftup;
    [SerializeField]
    private Transform rightdown;

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
        targetPosition = GetRandomPositionOnLevel1();
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
        targetPosition = GetRandomPositionOnLevel1();
        nextState = EnemyAiState.MOVE;
    }


    void AiMainRoutine()
    {
        if(isSee == true)
        {
            nextState = EnemyAiState.MOVEANDATACK;
        }
        else if(isSee == false)
        {
            nextState=EnemyAiState.MOVE;
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
    }
    void Move()
    {
        float sqrDistanceToTarget = Vector3.SqrMagnitude(transform.position - targetPosition);
        if (sqrDistanceToTarget < changeTargetSqrDistance)
        {
            targetPosition = GetRandomPositionOnLevel1();
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

        BearRunAnimGo();
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Debug.Log("Serch");
    }

    void MoveAndAtack()
    {
        if (isBearHit == false && bearhp > 0)
        {
            //BearRunAnimGo();
            m_bear.SetBool("Detection", true);
            navMeshAgent.destination = player.transform.position;
        }
        Debug.Log("”­Œ©");
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
