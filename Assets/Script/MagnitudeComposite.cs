using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

internal class MagnitudeComposite : InputBindingComposite<float>
{
    // ����
    [InputControl] public int input = 0;

    /// <summary>
    /// ������
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    private static void Initialize()
    {
        // �����CompositeBinding��o�^����K�v������
        InputSystem.RegisterBindingComposite(typeof(MagnitudeComposite), "Magnitude");
    }

    /// <summary>
    /// ���͒l�̑傫���ɕϊ����ĕԂ�
    /// </summary>
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        return context.EvaluateMagnitude(input);
    }

    /// <summary>
    /// �l�̑傫����Ԃ�
    /// </summary>
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context);
    }
}
