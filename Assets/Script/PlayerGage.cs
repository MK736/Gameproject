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

    //private Player m_Player;
    private Tween redGaugeTween;

    static public PlayerGage instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GaugeReduction(float redcationValue, float time = 1.0f)
    {
        var valueFrom = MainManager.instance.m_Php / MainManager.instance.m_MaxPhp;
        var valueTo = (MainManager.instance.m_Php - redcationValue) / MainManager.instance.m_MaxPhp;

        // �΃Q�[�W����
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

    //public void SetPlayer(Player m_Player)
    //{
    //    this.m_Player = m_Player;
    //}
}
