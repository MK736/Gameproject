using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("�ړ��̑���"), SerializeField]
    private float _speed = 3;

    [Header("�W�����v����u�Ԃ̑���"), SerializeField]
    private float _jumpSpeed = 7;

    [Header("�d�͉����x"), SerializeField]
    private float _gravity = 15;

    [Header("�������̑��������iInfinity�Ŗ������j"), SerializeField]
    private float _fallSpeed = 10;

    [Header("�����̏���"), SerializeField]
    private float _initFallSpeed = 2;

    //[Header("�J����"), SerializeField]
    private Camera _targetCamera;
    private Transform _cameratransform;

    private Transform _transform;
    private CharacterController _characterController;

    private Vector2 _inputMove;
    private float _verticalVelocity;
    private float _turnVelocity;
    private bool _isGroundedPrev;

    public bool isAtack = false;
    [SerializeField]
    Enemy m_enemy;
    private GameObject m_MainManager;
    private MainManager m_Mainmanager;
    public BoxCollider m_boxCollider = null;
    public Animator m_PlayerAnimmator = null;

    Vector3 moveVelocity = Vector3.zero;

    /// <summary>
    /// �ړ�Action(PlayerInput������Ă΂��)
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        _inputMove = context.ReadValue<Vector2>();

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

    /// <summary>
    /// �W�����vAction(PlayerInput������Ă΂��)
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // �{�^���������ꂽ�u�Ԃ����n���Ă��鎞��������
        if (!context.performed || !_characterController.isGrounded) return;

        // ����������ɑ��x��^����
        _verticalVelocity = _jumpSpeed;
    }

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();

        if (_targetCamera == null)
            _targetCamera = Camera.main;

        _cameratransform = _targetCamera.transform;

        m_PlayerAnimmator = GetComponent<Animator>();
        m_enemy = GetComponent<Enemy>();

        m_MainManager = GameObject.FindWithTag("MainManager");
        m_Mainmanager = m_MainManager.GetComponent<MainManager>();
    }

    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    public void WeaponColOn()
    {
        m_boxCollider.enabled = true;
    }

    public void WeaponColOff()
    {
        m_boxCollider.enabled = false;
    }

    public void OnAtack(InputAction.CallbackContext context)
    {
        // �����ŕ���̓����蔻���ON�ɂ���B
        isAtack = true;
        //WeaponColOn();
        WeaponColOn();
        //m_boxCollider.enabled = true;
        m_PlayerAnimmator.SetTrigger("atack");

    }

    private void FixedUpdate()
    {
        if (m_PlayerAnimmator.GetBool("atack") == true) return;
        {
            Rotate();
            Move();
        }
    }

    private void Move()
    {
        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;

        // CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        _characterController.Move(moveDelta);
    }

    void Rotate()
    {
        // �J�����̌����i�p�x[deg]�j�擾
        var cameraAngleY = _cameratransform.transform.eulerAngles.y;

        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        moveVelocity = new Vector3(
            _inputMove.x * _speed,
            _verticalVelocity,
            _inputMove.y * _speed
        );
        // �J�����̊p�x�������ړ��ʂ���]
        moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;
        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;

        if (_inputMove != Vector2.zero)
        {
            // �ړ����͂�����ꍇ�́A�U�����������s��

            // ������͂���y������̖ڕW�p�x[deg]���v�Z
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x)
                * Mathf.Rad2Deg + 90;
            // �J�����̊p�x�������U������p�x��␳
            targetAngleY += cameraAngleY;

            // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
            var angleY = Mathf.SmoothDampAngle(
                _transform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );

            // �I�u�W�F�N�g�̉�]���X�V
            _transform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }

    private void Update()
    {
        var isGrounded = _characterController.isGrounded;

        if (isGrounded && !_isGroundedPrev)
        {
            // ���n����u�Ԃɗ����̏������w�肵�Ă���
            _verticalVelocity = -_initFallSpeed;
        }
        else if (!isGrounded)
        {
            // �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
            _verticalVelocity -= _gravity * Time.deltaTime;

            // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
            if (_verticalVelocity < -_fallSpeed)
                _verticalVelocity = -_fallSpeed;
        }

        _isGroundedPrev = isGrounded;

        //// �J�����̌����i�p�x[deg]�j�擾
        //var cameraAngleY = _cameratransform.transform.eulerAngles.y;

        //// ������͂Ɖ����������x����A���ݑ��x���v�Z
        //var moveVelocity = new Vector3(
        //    _inputMove.x * _speed,
        //    _verticalVelocity,
        //    _inputMove.y * _speed
        //);
        //// �J�����̊p�x�������ړ��ʂ���]
        //moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;

        //// ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        //var moveDelta = moveVelocity * Time.deltaTime;

        //// CharacterController�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        //_characterController.Move(moveDelta);

        //if (_inputMove != Vector2.zero)
        //{
        //    // �ړ����͂�����ꍇ�́A�U�����������s��

        //    // ������͂���y������̖ڕW�p�x[deg]���v�Z
        //    var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x)
        //        * Mathf.Rad2Deg + 90;
        //    // �J�����̊p�x�������U������p�x��␳
        //    targetAngleY += cameraAngleY;

        //    // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
        //    var angleY = Mathf.SmoothDampAngle(
        //        _transform.eulerAngles.y,
        //        targetAngleY,
        //        ref _turnVelocity,
        //        0.1f
        //    );

        //    // �I�u�W�F�N�g�̉�]���X�V
        //    _transform.rotation = Quaternion.Euler(0, angleY, 0);
        //}

        if (m_Mainmanager.isDead == true)
        {
            Death();
        }
    }
    public void Death()
    {
        m_PlayerAnimmator.SetBool("Death", true);
        Destroy(gameObject, 1.5f);
        Debug.Log("���S");
    }
}
