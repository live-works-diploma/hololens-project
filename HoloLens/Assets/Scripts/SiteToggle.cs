using UnityEngine;

public class SiteToggle : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] GameObject offText;
    [SerializeField] GameObject onText;
    
    [Header("Toggle Status")]
    [SerializeField] bool startToggled = false;
    bool isToggled = false;

    [Header("Position")]
    [SerializeField] private RectTransform tipRect;
    private readonly Vector2 _min = new(0, 0.5f);
    private readonly Vector2 _max = new(1, 0.5f);
    private readonly Vector2 _middle = new(0.5f, 0.5f);

    void Start()
    {
        if (startToggled)
        {
            isToggled = true;
            SetOn();
        }
        else
        {
            isToggled = false;
            SetOff();
        }
    }

    public void Toggle()
    {
        if (isToggled)
        {
            isToggled = false;
            SetOff();
        }
        else
        {
            isToggled = true;
            SetOn();
        }
    }

    void SetOn()
    {
        onText.SetActive(true);
        offText.SetActive(false);

        SetAnchors(_max);
    }

    void SetOff()
    {
        offText.SetActive(true);
        onText.SetActive(false);

        SetAnchors(_min);
    }

    void SetAnchors(Vector2 anchor)
    {
        tipRect.anchorMin = anchor;
        tipRect.anchorMax = anchor;
        tipRect.pivot = anchor;
    }
}
