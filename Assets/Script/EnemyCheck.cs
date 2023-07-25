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
    /// Is Trigger‚ªON‚Å‘¼‚ÌCollider‚Æd‚È‚Á‚Ä‚¢‚é‚Æ‚«‚ÉŒÄ‚Î‚ê‘±‚¯‚é
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
