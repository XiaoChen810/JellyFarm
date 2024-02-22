using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    public int index = 0;
    public Button buy;
    public Button next;
    public Button last;
    public Image jellyImage;
    public Text nameText;
    public Text priceText;


    private void Start()
    {
        animator = GetComponent<Animator>();
        UIManager.Instance.OnClickShopBtn += OpenAndClose;
        buy.onClick.AddListener(Buy);
        next.onClick.AddListener(Next);
        last.onClick.AddListener(Last);
        InitGoods();
    }

    private void InitGoods()
    {
        Goods newGoods = GameManager.Instance.GetGoods(index);
        jellyImage.sprite = newGoods.jellyImage;
        nameText.text = newGoods.jellyName;
        priceText.text = newGoods.jellyPrice == -1 ? "???" : newGoods.jellyPrice.ToString();
    }

    private void OnDestroy()
    {
        UIManager.Instance.OnClickShopBtn -= OpenAndClose;
    }

    private void OpenAndClose()
    {
        if (isOpen)
        {
            animator.SetBool("IsOpen", false);
            isOpen = false;
        }
        else
        {
            animator.SetBool("IsOpen", true);
            isOpen = true;
        }
    }

    private void Buy()
    {
        if (isOpen)
        {
            GameManager.Instance.Buy(index);
        }
    }

    private void Next()
    {
        index++;
        if(index >= GameManager.Instance.jellys.Count)
        {
            index = 0;
        }
        InitGoods();
    }

    private void Last()
    {
        index--;
        if(index < 0)
        {
            index = GameManager.Instance.jellys.Count - 1;
        }
        InitGoods();
    }
}
