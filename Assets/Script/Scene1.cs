using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Player.instance.Stagename = "stage3";
            SceneManager.LoadScene("New Scene");
            //this.transform.position = new Vector3(853,5,712);
        }
    }
}
