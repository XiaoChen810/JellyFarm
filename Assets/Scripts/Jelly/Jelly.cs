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
    public float waitTime = 2f; // �ȴ�ʱ�䣬���Ը�����Ҫ����
    public bool canMove = true;
    private BoxCollider2D moveBoundary;

    private float sizeUnit = 0.001f;

    private void Start()
    {
        // �޶��ƶ���Χ
        GameObject mapBoundaryObject = GameObject.Find("MapBoundary");
        if (mapBoundaryObject != null)
        {
            // ���ҵ�����Ϸ�����ϻ�ȡBoxCollider2D���
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
        // ��ת��ɫ����
        FlipCharacter();

        //ÿ60��������1���۸���£��̺�����ֵ���£��ߴ���£��ȼ�����
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
    /// ����������
    /// </summary>
    public void TouchOrSell()
    {
        // �����������
        if (GameManager.Instance.onSell)
        {
            GameManager.Instance.Sell(this);
            return;
        }
        // �������ת��
        if (GameManager.Instance.onChange)
        {
            GameManager.Instance.Change(this);
            return;
        }
        // ֻ�Ǵ���
        anim.SetTrigger("doTouch");
        int add = Random.Range(3, 10);
        GameManager.Instance.jellyCount += add;
        data.Size += add / 2;
        AudioManager.Instance.PlaySound_Touch();
    }

    /// <summary>
    /// ��JellyPanel������Ϊ�Լ�������
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
        // �жϽ�ɫ�ǳ����ǳ����ƶ�
        if (currentPosition.x > previousPosition.x)
        {
            // �泯�ұ�
            transform.localScale = new Vector3(CalculateSize(), CalculateSize(), CalculateSize());
        }
        else if (currentPosition.x < previousPosition.x)
        {
            // �泯���
            transform.localScale = new Vector3(-CalculateSize(), CalculateSize(), CalculateSize());
        }
        // ����ǰһ֡λ��
        previousPosition = currentPosition;
    }

    #endregion
}



[System.Serializable]
public class JellyData
{
    //��ƷId������Ψһ
    public int Id;
    //���֣����Ը�
    public string Name;
    //���Ѷ���Ǯ���
    public float Cost;
    //���ͣ���ǰ
    public float Size;
    //�۸񣬵�ǰ
    public int Price;
    //������������ǰ
    public int JellyCount;
    //��������ǰ
    public float Age;
    //�ȼ�����ǰ
    public int Level;
    //�ɳ��ٶȣ�ԽСԽ��
    public float GrowSpeed = 300;
    //���ʱ��
    public float _time;
}
