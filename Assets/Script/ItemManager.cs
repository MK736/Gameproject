using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{

    public GameObject ItemObject;
    [SerializeField]
    private Item item;

    public void ItemDrop()
    {
        if (item.GetItemName() == "�F��")
        {
            Instantiate(ItemObject, transform.position, Quaternion.identity);
        }
    }

}
