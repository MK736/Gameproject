using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

internal class QuickPressInteraction : IInputInteraction
{
    // スティック倒し始めの入力値の大きさ
    public float startPressPoint = 0.2f;

    // スティック倒し終わりの入力値の大きさ
    public float endPressPoint = 0.9f;

    // スティックを完全に倒し終わるまでの最大許容時間[s]
    public float maxDelay = 0.1f;

    // スティックを離したと判断する閾値
    public float releasePoint = 0.375f;

    private double _startPressTime;
    private bool _isFree = true;

    /// <summary>
    /// 初期化
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void Initialize()
    {
        // 初回にInteractionを登録する必要がある
        InputSystem.RegisterInteraction<QuickPressInteraction>();
    }

    public void Process(ref InputInteractionContext context)
    {
        // タイムアウト判定
        if (context.timerHasExpired)
        {
            // 最大許容時間を超えてタイムアウトになった場合はキャンセル
            context.Canceled();
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                // 入力待機状態

                if (!_isFree)
                {
                    // ニュートラル位置チェック
                    if (!context.ControlIsActuated(startPressPoint))
                    {
                        _isFree = true;
                    }

                    break;
                }

                // スティックがニュートラル位置なら、入力チェック
                if (context.ControlIsActuated(endPressPoint))
                {
                    // 一気にスティックが倒された場合
                    _isFree = false;
                    _startPressTime = context.time;

                    // Started、Performedコールバックを一気に発火
                    context.Started();
                    context.PerformedAndStayPerformed();
                }
                else if (context.ControlIsActuated(startPressPoint))
                {
                    // スティックが倒され始めた場合
                    _isFree = false;
                    _startPressTime = context.time;

                    // Startedコールバック発火
                    context.Started();
                    context.SetTimeout(maxDelay);
                }

                break;

            case InputActionPhase.Started:
                // スティックが倒され始めている状態

                if (context.time - _startPressTime <= maxDelay)
                {
                    // 最大許容時間内にスティックが完全に倒されたかチェック
                    if (context.ControlIsActuated(endPressPoint))
                    {
                        // 倒されたらPerformedコールバックを発火
                        context.PerformedAndStayPerformed();
                    }
                }
                else
                {
                    // 最大許容時間内にスティックが完全に倒されなければ中断とみなす
                    context.Canceled();
                }

                break;

            case InputActionPhase.Performed:
                // スティック早倒し中

                // スティックが戻されているかどうかのチェック
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
