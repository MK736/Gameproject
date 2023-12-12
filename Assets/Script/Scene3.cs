using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Player.instance.Stagename = "stage3";
            SceneManager.LoadScene("stage3");
            //this.transform.position = new Vector3(853,5,712);
        }
    }
}
