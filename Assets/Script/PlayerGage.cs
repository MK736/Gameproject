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

    public void GaugeReduction(float redcationValue, float time = 1f)
    {
        var valueFrom = m_Player.g_PlayerHP / m_Player.g_MaxPlayerHP;
        var valueTo = (m_Player.g_PlayerHP - redcationValue) / m_Player.g_MaxPlayerHP;

        // —ÎƒQ[ƒWŒ¸­
        GreenGauge.fillAmount = valueTo;

        if (redGaugeTween != null)
        {
            redGaugeTween.Kill();
        }

        // ÔƒQ[ƒWŒ¸­
        redGaugeTween = DOTween.To(
            () => valueFrom,
            x => {
                RedGauge.fillAmount = x;
            },
            valueTo,
            time
        );
    }

    public void SetPlayer(Player m_Player)
    {
        this.m_Player = m_Player;
    }
}
