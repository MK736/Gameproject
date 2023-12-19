using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AnimationEventHook.instance.Index = 1;
            AnimationEventHook.instance.IndexJump = 4;

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
