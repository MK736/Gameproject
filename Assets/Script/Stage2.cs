using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Player.instance.Stagename = "stage3";
            SceneManager.LoadScene("stage2");
            //this.transform.position = new Vector3(853,5,712);
        }
    }
}
