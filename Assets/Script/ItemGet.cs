using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemGet : MonoBehaviour
{

    ItemManager m_itemma;
    GameObject player;
    PlayerScript m_player;


    // Start is called before the first frame update
    void Awake()
    {
        m_itemma = GetComponent<ItemManager>();
        player = GameObject.Find("Player");
        m_player = player.GetComponent<PlayerScript>();
    }

   void OnTriggerEnter(Collider other)
    {
        if (m_player.isAtack == true)
        {
            if (other.CompareTag("weapon"))
            {
                Destroy(gameObject);
                m_itemma.ItemDrop();

            }
        }

    }
}
