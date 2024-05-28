using UnityEngine;
using UnityEngine.Events;

public class ClickHandler : MonoBehaviour
{
    private float _mouseHoldTime = 0;
    private float _maxHoldTime = 0.3f;
    public LayerMask excludedLayer; // ÅÅ³ýµÄÍ¼²ã

    void Update()
    {
#if UNITY_ANDROID
        Hand();
#else
        Mosue();
#endif
    }

    private void Hand()
    {
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                Click(0);
            }

            if (Input.touches[0].phase == TouchPhase.Stationary)
            {
                _mouseHoldTime += Time.deltaTime;
                if (_mouseHoldTime >= _maxHoldTime)
                {
                    Hold(0);
                }
            }

            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                Release(0);
                if (_mouseHoldTime < _maxHoldTime)
                {
                    UIManager.Instance.JellyPanelClose();
                }
                _mouseHoldTime = 0;
            }
        }
    }

    private void Mosue()
    {
        // ¼ì²âÊó±ê×ó¼üµã»÷
        if (Input.GetButtonDown("Fire1")) 
        {
            Click(0);
        }
        // ¼ì²âÊó±êÓÒ¼üµã»÷
        else if (Input.GetButtonDown("Fire2")) 
        {
            Click(1);
        }
        // ¼ì²âÊó±ê×ó¼ü³¤°´
        if (Input.GetMouseButton(0)) 
        {
            _mouseHoldTime += Time.deltaTime;
            if (_mouseHoldTime > _maxHoldTime)
            {
                Hold(0);
            }
        }
        // ¼ì²âÊó±ê×ó¼üËÉ¿ª
        if (Input.GetMouseButtonUp(0))
        {
            if (_mouseHoldTime < _maxHoldTime)
            {
                UIManager.Instance.JellyPanelClose();
            }
            _mouseHoldTime = 0;
            Release(0);
        }
    }

    void Click(int button)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, ~excludedLayer);

        if (hit.collider != null && hit.collider.CompareTag("Jelly"))
        {
            Jelly jelly = hit.collider.GetComponent<Jelly>();
            if (jelly != null)
            {
                if (button == 0)
                {
                    jelly.TouchOrSell();
                }
                else if (button == 1)
                {
                    jelly.OpenJellyPanel();
                }
            }
        }
    }

    void Hold(int button)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, ~excludedLayer);

        if (hit.collider != null && hit.collider.CompareTag("Jelly"))
        {
            // ¼ì²âµ½Åö×²Ìå
            if (button == 0)
            {
                Jelly jelly = hit.collider.GetComponent<Jelly>();
                if (jelly != null)
                {
                    jelly.OpenJellyPanel();
                }
            }
        }

    }

    void Release(int button)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, ~excludedLayer);

        if (hit.collider != null && hit.collider.CompareTag("Jelly"))
        {
            // ¼ì²âµ½Åö×²Ìå
            if (button == 0)
            {

            }
        }
    }
}
