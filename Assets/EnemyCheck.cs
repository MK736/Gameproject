using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyCheck : MonoBehaviour
{
    [SerializeField] private TriggerEvent onTriggerStay = new TriggerEvent();

    [SerializeField] private TriggerEvent onTriggerExit = new TriggerEvent();

    /// <summary>
    /// Is Trigger��ON�ő���Collider�Əd�Ȃ��Ă���Ƃ��ɌĂ΂ꑱ����
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {

        onTriggerStay.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke(other);

    }

    [Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {
    }
}
