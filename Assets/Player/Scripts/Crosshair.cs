using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("Crosshair")]
    [Tooltip("Width of segment in pixels")]
    public float Width = 10.0f;
    [Tooltip("Height of segment in pixels")]
    public float Height = 2.0f;
    [Tooltip("Spacing of segments in pixels")]
    public float Spacing = 5.0f;

    [Header("UI")]
    [Tooltip("Crosshair left segment")]
    public RectTransform LeftSeg;
    [Tooltip("Crosshair right segment")]
    public RectTransform RightSeg;
    [Tooltip("Crosshair up segment")]
    public RectTransform UpSeg;
    [Tooltip("Crosshair down segment")]
    public RectTransform DownSeg;

    public void Start()
    {
        UpdateCrosshair();
    }

    public void UpdateCrosshair()
    {
        LeftSeg.anchoredPosition = new Vector2(-Spacing, 0);
        LeftSeg.sizeDelta = new Vector2(Width, Height);

        RightSeg.anchoredPosition = new Vector2(Spacing, 0);
        RightSeg.sizeDelta = new Vector2(Width, Height);

        UpSeg.anchoredPosition = new Vector2(0, Spacing);
        UpSeg.sizeDelta = new Vector2(Height, Width);

        DownSeg.anchoredPosition = new Vector2(0, -Spacing);
        DownSeg.sizeDelta = new Vector2(Height, Width);
    }
}
