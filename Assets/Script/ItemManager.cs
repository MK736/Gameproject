using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject ItemObject;
    [SerializeField]
    private Item item;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ItemDrop()
    {
        if (item.GetItemName() == "ŒF“÷")
        {
            Instantiate(ItemObject, transform.position, Quaternion.identity);
        }
    }

}
