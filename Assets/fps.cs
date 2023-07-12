using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
    }

     void Update()
    {
        float fps = 1f / Time.deltaTime;
        Debug.LogFormat("{0}fps", fps);
    }
}
