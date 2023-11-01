using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Item))]
public class ItemManager : MonoBehaviour
{

    public GameObject ItemObject;
    [SerializeField]
    private Item item;

    public void ItemDrop()
    {
        if (item.GetItemName() == "ŒF“÷")
        {
            Instantiate(ItemObject, transform.position, Quaternion.identity);
        }
    }

}
