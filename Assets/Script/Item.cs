using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update

    public enum Type
    {
        Beaf
    }


    [SerializeField]
    public Type itemType = Type.Beaf;
    //�A�C�e���̖��O
    [SerializeField]
    private string ItemName = "";
    //�A�C�e���̏��
    [SerializeField]
    private string information = "";
    //�A�C�e���̌��i�R�C���Ȃ牽���Ȃ̂����j
    [SerializeField]
    private int amount = 0;

    public Type GetItemType()
    {
        return itemType;
    }

    public string GetItemName()
    {
        return ItemName;
    }

    public string GetInformation()
    {
        return information;
    }

    public int GetAmount()
    {
        return amount;
    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
