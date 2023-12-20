using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

internal class MagnitudeComposite : InputBindingComposite<float>
{
    // 入力
    [InputControl] public int input = 0;

    /// <summary>
    /// 初期化
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    private static void Initialize()
    {
        // 初回にCompositeBindingを登録する必要がある
        InputSystem.RegisterBindingComposite(typeof(MagnitudeComposite), "Magnitude");
    }

    /// <summary>
    /// 入力値の大きさに変換して返す
    /// </summary>
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        return context.EvaluateMagnitude(input);
    }

    /// <summary>
    /// 値の大きさを返す
    /// </summary>
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context);
    }
}
