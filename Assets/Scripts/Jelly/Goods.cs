using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// …ÃµÍªıŒÔ
/// </summary>

[System.Serializable]
public class Goods
{
    public Goods(Sprite sprite,string name,int price)
    {
        jellyImage = sprite;
        jellyName = name;
        jellyPrice = price;
        jellyPrefab = null;
    }

    public string jellyName;
    public int jellyPrice;
    public Sprite jellyImage;
    public GameObject jellyPrefab;
}
