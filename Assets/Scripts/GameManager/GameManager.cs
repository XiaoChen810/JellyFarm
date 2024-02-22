using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int jellyCount;
    public int moneyCount;

    [Header("Bool")]
    public bool onSell;
    public bool onChange;

    [Header("Jelly")]
    public List<Goods> jellys;

    [Header("Lock")]
    public Sprite lockSprite;
    public int shopLevel;

    [Header("InGame")]
    //已生成的jelly列表和对应数据
    public List<GameObject> jellyObjs = new List<GameObject>();
    public List<JellyData> jellyDatas = new List<JellyData>();
    //用于保存的key字符串
    private readonly string key_jellyObjs = "jellyObjs";
    private readonly string key_jellyDatas = "jellyDatas";

    private float _time;

    public GameObject MenuPanel;

    private void Awake()
    {
        // 设置单例实例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = -1;
        UIManager.Instance.OnClickToolBtn += OpenSell;
        UIManager.Instance.OnClickJellyBtn += OpenChange;
        Load();
    }

    private void Update()
    {
        //每1.5f秒jellyCount值自增一点
        _time += Time.deltaTime;
        if(_time > 1.5f)
        {
            int add = UnityEngine.Random.Range(0, 10);
            jellyCount += add;
            _time = 0;
        }

        //计算当前可结束商品等级
        shopLevel = Mathf.FloorToInt(jellyCount / 5000) + 1;

        //当按下退出键，暂停游戏，打开菜单并保存
        if (Input.GetKeyDown(KeyCode.Escape) && !MenuPanel.activeSelf)
        {
            MenuPanel.SetActive(true);
            Time.timeScale = 0f;
            Save();
        }
    }

    private void OpenSell()
    {
        onSell = !onSell;
        if (onChange)
        {
            onChange = false;
        }
    }
    private void OpenChange()
    {
        onChange = !onChange;
        if (onSell)
        {
            onSell = false;
        }
    }

    public void Buy(int index)
    {
        if (index > shopLevel) return;
        if (moneyCount - jellys[index].jellyPrice < 0)
        {
            Debug.Log("钱不够");
            AudioManager.Instance.PlaySound_Fail();
            return;
        }

        moneyCount -= jellys[index].jellyPrice;
        GameObject newjelly = Instantiate(jellys[index].jellyPrefab);
        newjelly.name = jellys[index].jellyName;
        AudioManager.Instance.PlaySound_Buy();

        jellyObjs.Add(newjelly);
        Save();
    }
        
    /// <summary>
    /// 卖掉Jelly转换成MoneyCount
    /// </summary>
    /// <param name="jelly"></param>
    public void Sell(Jelly jelly)
    {
        AudioManager.Instance.PlaySound_Sell();
        moneyCount += jelly.data.Price;
        for (int i = 0; i < jellyObjs.Count; i++)
        {
            if (jellyObjs[i] == jelly.gameObject)
            {
                jellyObjs.RemoveAt(i);
                jellyDatas.RemoveAt(i);
                Destroy(jelly.gameObject);
                break;
            }
        }
        AudioManager.Instance.PlaySound_Clear();
        Save();
    }
    /// <summary>
    /// 卖掉Jelly转换成JellyCount
    /// </summary>
    /// <param name="jelly"></param>
    public void Change(Jelly jelly)
    {
        AudioManager.Instance.PlaySound_Sell();
        jellyCount += jelly.data.JellyCount;
        for (int i = 0; i < jellyObjs.Count; i++)
        {
            if (jellyObjs[i] == jelly.gameObject)
            {
                jellyObjs.RemoveAt(i);
                jellyDatas.RemoveAt(i);
                Destroy(jelly.gameObject);
                break;
            }
        }
        AudioManager.Instance.PlaySound_Clear();
        Save();
    }
    /// <summary>
    /// 获取商品信息，若不达当前可解锁等级则返回“锁”
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Goods GetGoods(int index)
    {
        if(index <= shopLevel)
        {
            return jellys[index];
        }
        else
        {
            Goods goods = new Goods(lockSprite, jellys[index].jellyName, -1);
            return goods;
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        ES3.Save<List<GameObject>>(key_jellyObjs, jellyObjs);
        ES3.Save<int>("jellyCount", jellyCount);
        ES3.Save<int>("moneyCount", moneyCount);

        jellyDatas.Clear();
        for (int i = 0; i < jellyObjs.Count; i++)
        {
            jellyDatas.Add(jellyObjs[i].GetComponent<Jelly>().data);
        }
        ES3.Save(key_jellyDatas, jellyDatas);
    }

    private void Load()
    {
        jellyObjs = ES3.Load<List<GameObject>>(key_jellyObjs);
        jellyCount = ES3.Load<int>("jellyCount");
        moneyCount = ES3.Load<int>("moneyCount");
        jellyDatas = ES3.Load<List<JellyData>>(key_jellyDatas);

        if(jellyObjs.Count != jellyDatas.Count)
        {
            Debug.LogWarning("ERROR");
            return;
        }
        for (int i = 0;i < jellyDatas.Count; i++)
        {
            jellyObjs[i].GetComponent<Jelly>().data = jellyDatas[i];
        }
    }
}
