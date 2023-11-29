using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGage : MonoBehaviour
{
    [SerializeField]
    private Image GreenGauge;
    [SerializeField]
    private Image RedGauge;

    private Player m_Player;
    private Tween redGaugeTween;

    public void GaugeReduction(float redcationValue, float time = 1.0f)
    {
        var valueFrom = m_Player.g_PlayerHP / m_Player.g_MaxPlayerHP;
        var valueTo = (m_Player.g_PlayerHP - redcationValue) / m_Player.g_MaxPlayerHP;

        // —ÎƒQ[ƒWŒ¸­
        GreenGauge.fillAmount = valueTo;

        if (redGaugeTween != null)
        {
            redGaugeTween.Kill();
        }

        DOVirtual.DelayedCall(0.5f, () => {
            RedGauge.fillAmount = valueTo;
        });
    }

    public void GaugeUp(float valueTo)
    {
        var valueto = valueTo;

        GreenGauge.fillAmount = valueto;

        if (redGaugeTween != null)
        {
            redGaugeTween.Kill();
        }

        RedGauge.fillAmount = valueto;
    }

    public void SetPlayer(Player m_Player)
    {
        this.m_Player = m_Player;
    }
}
