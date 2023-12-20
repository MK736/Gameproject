using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

internal class QuickPressInteraction : IInputInteraction
{
    // �X�e�B�b�N�|���n�߂̓��͒l�̑傫��
    public float startPressPoint = 0.2f;

    // �X�e�B�b�N�|���I���̓��͒l�̑傫��
    public float endPressPoint = 0.9f;

    // �X�e�B�b�N�����S�ɓ|���I���܂ł̍ő勖�e����[s]
    public float maxDelay = 0.1f;

    // �X�e�B�b�N�𗣂����Ɣ��f����臒l
    public float releasePoint = 0.375f;

    private double _startPressTime;
    private bool _isFree = true;

    /// <summary>
    /// ������
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void Initialize()
    {
        // �����Interaction��o�^����K�v������
        InputSystem.RegisterInteraction<QuickPressInteraction>();
    }

    public void Process(ref InputInteractionContext context)
    {
        // �^�C���A�E�g����
        if (context.timerHasExpired)
        {
            // �ő勖�e���Ԃ𒴂��ă^�C���A�E�g�ɂȂ����ꍇ�̓L�����Z��
            context.Canceled();
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                // ���͑ҋ@���

                if (!_isFree)
                {
                    // �j���[�g�����ʒu�`�F�b�N
                    if (!context.ControlIsActuated(startPressPoint))
                    {
                        _isFree = true;
                    }

                    break;
                }

                // �X�e�B�b�N���j���[�g�����ʒu�Ȃ�A���̓`�F�b�N
                if (context.ControlIsActuated(endPressPoint))
                {
                    // ��C�ɃX�e�B�b�N���|���ꂽ�ꍇ
                    _isFree = false;
                    _startPressTime = context.time;

                    // Started�APerformed�R�[���o�b�N����C�ɔ���
                    context.Started();
                    context.PerformedAndStayPerformed();
                }
                else if (context.ControlIsActuated(startPressPoint))
                {
                    // �X�e�B�b�N���|����n�߂��ꍇ
                    _isFree = false;
                    _startPressTime = context.time;

                    // Started�R�[���o�b�N����
                    context.Started();
                    context.SetTimeout(maxDelay);
                }

                break;

            case InputActionPhase.Started:
                // �X�e�B�b�N���|����n�߂Ă�����

                if (context.time - _startPressTime <= maxDelay)
                {
                    // �ő勖�e���ԓ��ɃX�e�B�b�N�����S�ɓ|���ꂽ���`�F�b�N
                    if (context.ControlIsActuated(endPressPoint))
                    {
                        // �|���ꂽ��Performed�R�[���o�b�N�𔭉�
                        context.PerformedAndStayPerformed();
                    }
                }
                else
                {
                    // �ő勖�e���ԓ��ɃX�e�B�b�N�����S�ɓ|����Ȃ���Β��f�Ƃ݂Ȃ�
                    context.Canceled();
                }

                break;

            case InputActionPhase.Performed:
                // �X�e�B�b�N���|����

                // �X�e�B�b�N���߂���Ă��邩�ǂ����̃`�F�b�N
                if (!context.ControlIsActuated(releasePoint))
                {
                    context.Canceled();
                }
                else if (context.ControlIsActuated())
                {
                    context.PerformedAndStayPerformed();
                }

                break;
        }
    }

    public void Reset()
    {
        _startPressTime = 0;
    }
}
