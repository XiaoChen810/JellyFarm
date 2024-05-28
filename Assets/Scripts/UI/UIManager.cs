using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("果冻量和存款")]
    public Text jellyCount;
    public Text moneyCount;

    [Header("果冻")]
    public Button jellyBtn;
    public Image jellyImage1;
    public Image jellyImage2;

    [Header("锤子")]
    public Button toolBtn;
    public Image toolImage1;
    public Image toolImage2;

    [Header("商店")]
    public Button shopBtn;
    public Image shopImage1;
    public Image shopImage2;
    private bool isCooldown = false;
    private float pressCooldown = 0.5f; // 设置按下冷却时间

    public Action OnClickJellyBtn;
    public Action OnClickToolBtn;
    public Action OnClickShopBtn;

    [Header("JellyPanel")]
    public GameObject jellyPanel;
    public Image headImage;
    public Text nameText;
    public Text priceText;
    public Text jellyCountText;
    public Text oldText;
    public Button panelBtn;

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
        jellyBtn.onClick.AddListener(JellyBtnOnClick);
        toolBtn.onClick.AddListener(ToolBtnOnClick);
        shopBtn.onClick.AddListener(ShopBtnOnClick);
    }

    private void Update()
    {
        jellyCount.text = GameManager.Instance.jellyCount.ToString();
        moneyCount.text = GameManager.Instance.moneyCount.ToString();
    }

    public void JellyBtnOnClick()
    {
        AudioManager.Instance.PlaySound_Button();
        jellyImage1.gameObject.SetActive(jellyImage2.gameObject.activeSelf);
        jellyImage2.gameObject.SetActive(!jellyImage1.gameObject.activeSelf);

        if (toolImage2.gameObject.activeSelf)
        {
            toolImage1.gameObject.SetActive(true);
            toolImage2.gameObject.SetActive(false);
        }

        OnClickJellyBtn?.Invoke();
    }
    public void ToolBtnOnClick()
    {
        AudioManager.Instance.PlaySound_Button();
        toolImage1.gameObject.SetActive(toolImage2.gameObject.activeSelf);
        toolImage2.gameObject.SetActive(!toolImage1.gameObject.activeSelf);

        if (jellyImage2.gameObject.activeSelf)
        {
            jellyImage1.gameObject.SetActive(true);
            jellyImage2.gameObject.SetActive(false);
        }

        OnClickToolBtn?.Invoke();
    }
    public void ShopBtnOnClick()
    {
        if (isCooldown) return;
        StartCoroutine(ShopBtnOnClickWithCooldown());

        AudioManager.Instance.PlaySound_Button();
        shopImage1.gameObject.SetActive(shopImage2.gameObject.activeSelf);
        shopImage2.gameObject.SetActive (!shopImage1.gameObject.activeSelf);
        OnClickShopBtn?.Invoke();
    }
    private IEnumerator ShopBtnOnClickWithCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(pressCooldown);
        isCooldown = false;
    }
    public void JellyPanelOpen(Sprite sprite, string name, float price, float jellyCount, float old)
    {
        Animator anim = jellyPanel.GetComponent<Animator>();
        anim.SetBool("IsOpen", true);
        headImage.sprite = sprite;
        nameText.text = name;
        priceText.text = price.ToString();
        jellyCountText.text = jellyCount.ToString();
        oldText.text = old.ToString();
    }
    public void JellyPanelClose()
    {
        Animator anim = jellyPanel.GetComponent<Animator>();
        if (anim.GetBool("IsOpen"))
        {
            anim.SetBool("IsOpen", false);
        }
    }
}
