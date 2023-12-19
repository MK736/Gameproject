using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AnimationEventHook.instance.Index = 2;
            AnimationEventHook.instance.IndexJump = 5;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AnimationEventHook.instance.Index = 0;
            AnimationEventHook.instance.IndexJump = 3;
        }
    }
}
