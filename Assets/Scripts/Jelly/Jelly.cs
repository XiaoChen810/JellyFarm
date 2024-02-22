using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Jelly : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("Data")]
    public JellyData data = new JellyData();

    [Header("Move")]
    public float moveSpeed = 1f;
    public float waitTime = 2f; // 等待时间，可以根据需要调整
    public bool canMove = true;
    private BoxCollider2D moveBoundary;

    private float sizeUnit = 0.001f;

    private void Start()
    {
        // 限定移动范围
        GameObject mapBoundaryObject = GameObject.Find("MapBoundary");
        if (mapBoundaryObject != null)
        {
            // 在找到的游戏对象上获取BoxCollider2D组件
            moveBoundary = mapBoundaryObject.GetComponent<BoxCollider2D>();

            if (moveBoundary == null)
            {
                Debug.LogError("No MapBoundary");
            }
        }
        else
        {
            Debug.LogError("MapBoundary can't found");
        }

        coll = GetComponent<BoxCollider2D>();
        coll.isTrigger = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(RandomMoveCoroutine());
        anim.SetBool("isWalk", true);

        if (GameManager.Instance == null) return;

        data.Cost = GameManager.Instance.jellys[data.Id].jellyPrice;
        data.Name = GameManager.Instance.jellys[data.Id].jellyName;
        if (data.Price == 0) data.Price = CalculatePrice();
    }

    private void Update()
    {
        // 翻转角色朝向
        FlipCharacter();

        //每60秒岁数加1，价格更新，蕴含果冻值更新，尺寸更新，等级更新
        data._time += Time.deltaTime;
        data.Age = CalculateAge();
        data.Price = CalculatePrice();
        data.JellyCount = CalculateJellyCount();
        data.Size += Time.deltaTime;
        data.Level = CalculateLevel();
    }

    #region Calculate

    private int CalculatePrice()
    {
        return Mathf.FloorToInt(data.Cost * (data.Age + 1) * 0.3f + data.Size * 0.2f);
    }
    private int CalculateJellyCount()
    {
        return Mathf.FloorToInt(data.Level * data.GrowSpeed + data.Age * 500);
    }
    private float CalculateSize()
    {
        float size = data.Size * sizeUnit + 0.5f;
        if(size < 3)
        {
            return size;
        }
        return 3;
    }
    private int CalculateLevel()
    {
        return Mathf.FloorToInt(data.Size / data.GrowSpeed) + 1;
    }
    private int CalculateAge()
    {
        return Mathf.FloorToInt(data._time / 60);
    }
    #endregion

    #region Touch

    /// <summary>
    /// 触摸或销售
    /// </summary>
    public void TouchOrSell()
    {
        // 如果正在销售
        if (GameManager.Instance.onSell)
        {
            GameManager.Instance.Sell(this);
            return;
        }
        // 如果正在转换
        if (GameManager.Instance.onChange)
        {
            GameManager.Instance.Change(this);
            return;
        }
        // 只是触摸
        anim.SetTrigger("doTouch");
        int add = Random.Range(3, 10);
        GameManager.Instance.jellyCount += add;
        data.Size += add / 2;
        AudioManager.Instance.PlaySound_Touch();
    }

    /// <summary>
    /// 打开JellyPanel，设置为自己的数据
    /// </summary>
    public void OpenJellyPanel()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        UIManager.Instance.JellyPanelOpen(sr.sprite, data.Name, data.Price, data.JellyCount, data.Age);
    }

    #endregion

    #region Move

    IEnumerator RandomMoveCoroutine()
    {
        while (true)
        {
            if (canMove && moveBoundary != null)
            {
                Vector2 randomPoint = GetRandomPointWithinBoundary();
                Vector3 targetPosition = new Vector3(randomPoint.x, randomPoint.y, 0f);
                yield return StartCoroutine(MoveToTarget(targetPosition));
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    Vector2 GetRandomPointWithinBoundary()
    {
        float minX = moveBoundary.bounds.min.x;
        float maxX = moveBoundary.bounds.max.x;
        float minY = moveBoundary.bounds.min.y;
        float maxY = moveBoundary.bounds.max.y;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }

    IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            anim.SetBool("isWalk", true);
            Vector2 moveDirection = (targetPosition - transform.position).normalized;
            rb.velocity = moveDirection * moveSpeed;

            yield return null;
        }

        rb.velocity = Vector2.zero;
        anim.SetBool("isWalk", false);

    }

    private Vector3 previousPosition;

    void FlipCharacter()
    {
        Vector3 currentPosition = transform.position;
        // 判断角色是朝左还是朝右移动
        if (currentPosition.x > previousPosition.x)
        {
            // 面朝右边
            transform.localScale = new Vector3(CalculateSize(), CalculateSize(), CalculateSize());
        }
        else if (currentPosition.x < previousPosition.x)
        {
            // 面朝左边
            transform.localScale = new Vector3(-CalculateSize(), CalculateSize(), CalculateSize());
        }
        // 更新前一帧位置
        previousPosition = currentPosition;
    }

    #endregion
}



[System.Serializable]
public class JellyData
{
    //商品Id，不是唯一
    public int Id;
    //名字，可以改
    public string Name;
    //花费多少钱买的
    public float Cost;
    //体型，当前
    public float Size;
    //价格，当前
    public int Price;
    //含果冻量，当前
    public int JellyCount;
    //岁数，当前
    public float Age;
    //等级，当前
    public int Level;
    //成长速度，越小越快
    public float GrowSpeed = 300;
    //存活时间
    public float _time;
}
