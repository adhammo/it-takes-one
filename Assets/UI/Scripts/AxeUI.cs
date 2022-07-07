using UnityEngine;
using UnityEngine.UI;

public class AxeUI : MonoBehaviour
{
    public RectTransform Fill;

    public Fighter PlayerFighter;

    private Image _fillImage;

    void Start()
    {
        _fillImage = Fill.GetComponent<Image>();
        UpdateThrow();
    }

    void Update()
    {
        UpdateThrow();
    }

    void UpdateThrow()
    {
        float ratio = PlayerFighter.CurrentThrowCooldown / PlayerFighter.ThrowCooldown;
        _fillImage.fillAmount = ratio;
    }
}
