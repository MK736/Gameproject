using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform directionTransform;
    [SerializeField] Transform groundCheckTransform;

    [Header("PlayerMove")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float rotationSpeed = 180;
    [SerializeField] float groundDrag = 10;

    [Header("PlayerJump")]
    [SerializeField] float jumpForce = 10;
    [SerializeField] float jumpFreezeTime = 1;
    [SerializeField] float jumpMultiple = 0.2f;
    bool ReadyJump;

    [Header("GroundCheck")]
    [SerializeField] LayerMask whatGround;
    float groundCheckRayLength = 0.3f;
    bool isGround = true;
    bool wasGround = true;

    public bool isAtack = false;

    [Header("Slope")]
    [SerializeField] float maxAngle = 45;
    float slopeCheckRayLength = 0.5f;
    private RaycastHit slopehit;
    private bool exitSlope;

    //public int g_MaxPlayerHP = 0;
    //public int g_PlayerHP = 0;
    //protected PlayerGage playerGage;
    //public int PlayerPower = 1;

    //[SerializeField]
    //private BoxCollider m_boxCollider;

    //[SerializeField]
    //public CapsuleCollider BodyCollider = null;

    //public bool isDead = false;

    Rigidbody m_Rigidbody;
    Transform m_CameraTransform;
    Vector3 m_MoveDirection;
    Quaternion targetRotation;
    [SerializeField]
    Enemy m_enemy;

    public Animator m_PlayerAnimmator = null;

    //private BattleManager m_BattleManager = null;

    Vector2 m_MoveInput;

    Vector2 moveInput; // 移動入力
    bool jumpInput;

    readonly float GROUND_DRAG = 5;
    readonly float GRAVITY = 9.81f;
    readonly Vector2 VECTOR2_ZERO = new Vector2(0, 0);

    private GameObject m_MainManager;
    private MainManager m_Mainmanager;

    public string Stagename = "stage3";

    public BoxCollider m_boxCollider = null;

    // シングルトンs
    //static public Player instance;

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
        //ゲームマネージャーを作成しプレイヤーの位置情報をシングルトンで管理する
        //if(Stagename =="stage2")
        //{
        //    transform.position = new Vector3(20, 122, -19);
        //}
        //if (Stagename == "stage3")
        //{
        //    transform.position = new Vector3(855, 0, 710);
        //}

        //m_Camera = Camera.main;
        //m_CameraTransform = m_Camera.transform;

        //m_Rigidbody = GetComponent<Rigidbody>();
        //m_Rigidbody.drag = groundDrag;
        m_CameraTransform = Camera.main.transform;

        m_Rigidbody = playerTransform.GetComponent<Rigidbody>();
        m_Rigidbody.drag = groundDrag;
        m_PlayerAnimmator = GetComponent<Animator>();

        m_enemy = GetComponent<Enemy>();

        //playerGage = GameObject.FindObjectOfType<PlayerGage>();
        //playerGage.SetPlayer(this);

        //m_BattleManager = GetComponent<BattleManager>();

        //ReadyJump = true;
        ReadyJump = true;
        //g_PlayerHP = 500;
        //g_MaxPlayerHP = 500;
        //WeaponColOff();
        m_MainManager = GameObject.FindWithTag("MainManager");
        m_Mainmanager = m_MainManager.GetComponent<MainManager>();
    }

    private void Start()
    {
        //cameraTrn = Camera.main.transform;

        //rigidBody = playerTrn.GetComponent<Rigidbody>();
        //rigidBody.drag = groundDrag;

        //readyToJump = true;

        transform.position = new Vector3(0,0,0);
    }
    void Update()
    {
        //CheckGround();
        //SpeedControl();
        //Debug.Log(g_PlayerHP);

        CheckGround();
        //Rotate();
        SpeedControl();

        //if (jumpInput)
        //{
        //    Jump();
        //    jumpInput = false;
        //}

        if (m_Mainmanager.isDead == true)
        {
            Death();
        }
    }

    void FixedUpdate()
    {
        if (m_PlayerAnimmator.GetBool("atack") == true) return;
        {
            Rotate();
            Move();
        }
        //Move();
    }

    void CheckGround()
    {
        isGround = Physics.Raycast(groundCheckTransform.position, Vector3.down, groundCheckRayLength, whatGround);
        if (wasGround != isGround)
        {
            if (isGround)
            {
                m_Rigidbody.drag = groundDrag;
            }
            else
            {
                m_Rigidbody.drag = 0;
            }
            wasGround = isGround;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_MoveInput = context.ReadValue<Vector2>();

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    m_PlayerAnimmator.SetBool("is_running", true);
                    break;

                case InputActionPhase.Canceled:
                    m_PlayerAnimmator.SetBool("is_running", false);
                    break;
                default:
                    break;
            }
    }

    public void OnAtack(InputAction.CallbackContext context)
    {
        // ここで武器の当たり判定をONにする。
        isAtack = true;
        //WeaponColOn();
        WeaponColOn();
        //m_boxCollider.enabled = true;
        m_PlayerAnimmator.SetTrigger("atack");

    }

    public void WeaponColOn()
    {
        m_boxCollider.enabled = true;
    }

    public void WeaponColOff()
    {
        m_boxCollider.enabled = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //m_Rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //jumpInput = value.isPressed;
        //m_PlayerAnimmator.SetTrigger("jump");
        switch (context.phase)
        {
            case InputActionPhase.Started:
                m_PlayerAnimmator.SetBool("jump", true);
                break;

            case InputActionPhase.Canceled:
                m_PlayerAnimmator.SetBool("jump", false);
                break;
            default:
                break;
        }
        Jump();
    }

    //public void TakeDamage(Enemy m_Enemy)
    //{
    //    //m_PlayerAnimmator.SetTrigger("Hit");
    //    PlayerGage.instance.GaugeReduction(MainManager.instance.m_EAtackPower);


    //    MainManager.instance.m_Php -= MainManager.instance.m_EAtackPower;//m_BattleManager.HpDown(g_PlayerHP, m_Enemy.atackPower);


    //    m_Enemy.AtackEnd();
    //    if (MainManager.instance.m_Php == 0)
    //    {
    //        MainManager.instance.isDead = true;
    //        Death();

    //    }
    //}

    public void Death()
    {
        m_PlayerAnimmator.SetBool("Death", true);
        Destroy(gameObject, 1.5f);
        Debug.Log("死亡");
    }
    void Move()
    {
        m_MoveDirection = directionTransform.forward * m_MoveInput.y + directionTransform.right * m_MoveInput.x;

        // スロープ
        if (OnSlope() && !exitSlope)
        {
            m_Rigidbody.AddForce(GetSlopeMoveDirection() * moveSpeed * 20.0f, ForceMode.Force);
            if (m_Rigidbody.velocity.y > 0)
                m_Rigidbody.AddForce(Vector3.down * 80.0f, ForceMode.Force);
        }

        // 地上
        else if (isGround)
        {
            m_Rigidbody.AddForce(m_MoveDirection.normalized * moveSpeed * 10.0f, ForceMode.Force);
        }

        // 空中
        else if (!isGround)
        {
            m_Rigidbody.AddForce(m_MoveDirection.normalized * moveSpeed * 10.0f * jumpMultiple, ForceMode.Force);
        }

        m_Rigidbody.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        if (OnSlope() && !exitSlope)
        {
            if (m_Rigidbody.velocity.magnitude > moveSpeed)
                m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(m_Rigidbody.velocity.x, 0.0f, m_Rigidbody.velocity.z);
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitVel = flatVel.normalized * moveSpeed;
                m_Rigidbody.velocity = new Vector3(limitVel.x, m_Rigidbody.velocity.y, limitVel.z);
            }
        }
    }

    void Rotate()
    {

        var dir = playerTransform.position - new Vector3(m_CameraTransform.position.x, playerTransform.position.y, m_CameraTransform.position.z);
        directionTransform.forward = dir.normalized;

        m_MoveDirection = directionTransform.forward * m_MoveInput.y + directionTransform.right * m_MoveInput.x;

        if (m_MoveDirection != Vector3.zero)
        {
            playerTransform.forward = Vector3.Slerp(playerTransform.forward, m_MoveDirection.normalized, rotationSpeed * Time.deltaTime);
        }

    }
    private void Jump()
    {
        if (ReadyJump && isGround)
        {
            ReadyJump = false;

            exitSlope = true;
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, 0.0f, m_Rigidbody.velocity.z);
            m_Rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpFreezeTime);
        }
    }

    private void ResetJump()
    {
        ReadyJump = true;
        exitSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(groundCheckTransform.position, Vector3.down, out slopehit, slopeCheckRayLength))
        {
            float angle = Vector3.Angle(Vector3.up, slopehit.normal);
            return angle < maxAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(m_MoveDirection, slopehit.normal).normalized;
    }
}
