using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    AnimationEventHook m_animEve;
    GameObject m_gameObject;

    private void Start()
    {
        m_gameObject = GameObject.Find("Player");
        m_animEve = m_gameObject.GetComponent<AnimationEventHook>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_animEve.Index = 2;
            m_animEve.IndexJump = 5;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_animEve.Index = 0;
            m_animEve.IndexJump = 3;
        }
    }
}
