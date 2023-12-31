using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update

    public enum Type
    {
        Beaf,
        Heal
    }

    [SerializeField]
    public Type itemType = Type.Beaf;
    //アイテムの名前
    [SerializeField]
    private string ItemName = "";
    //アイテムの情報
    [SerializeField]
    private string information = "";
    //アイテムの個数（コインなら何枚なのか等）
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
}
