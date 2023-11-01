using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform directionTransform;
    [SerializeField] Transform groundCheckTransform;

    [Header("PlayerMove")]
    [SerializeField] float moveSpeed        = 10;
    [SerializeField] float rotationSpeed    = 180;
    [SerializeField] float groundDrag       = 10;

    [Header("PlayerJump")]
    [SerializeField] float jumpForce        = 10;
    [SerializeField] float jumpFreezeTime   = 1;
    [SerializeField] float jumpMultiple     = 0.2f;
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

    Rigidbody m_Rigidbody;
    Transform m_CameraTransform;
    Vector3 m_MoveDirection;
    Quaternion targetRotation;

    public Animator m_PlayerAnimmator = null;

    Vector2 m_MoveInput;

    readonly Vector2 VECTOR2_ZERO = new Vector2(0, 0);

    void Awake()
    {
        m_CameraTransform = Camera.main.transform;

        m_Rigidbody = playerTransform.GetComponent<Rigidbody>();
        m_Rigidbody.drag = groundDrag;
        m_PlayerAnimmator = GetComponent<Animator>();

        ReadyJump = true;
    }

    void Update()
    {
        CheckGround();
        SpeedControl();
    }

    void FixedUpdate()
    {
        if (m_PlayerAnimmator.GetBool("atack") == true) return;
        {
            Rotate();
            Move();
        }
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
        isAtack = true;
        m_PlayerAnimmator.SetTrigger("atack");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
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

        if (m_MoveDirection != Vector3.zero) {
            playerTransform.forward = Vector3.Slerp(playerTransform.forward, m_MoveDirection.normalized, rotationSpeed * Time.deltaTime);
        }

    }
    private void Jump()
    {
        if (ReadyJump && isGround) {
            ReadyJump = false;

            exitSlope = true;
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, 0.0f, m_Rigidbody.velocity.z);
            m_Rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(RestJump), jumpFreezeTime);
        }
    }

    private void RestJump()
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
