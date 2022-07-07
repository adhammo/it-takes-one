using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public RectTransform Fill;

    public Death PlayerDeath;

    private Image _fillImage;

    void Start()
    {
        _fillImage = Fill.GetComponent<Image>();
        UpdateHealth();
    }

    void Update()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        float ratio = PlayerDeath.CurrentHealth / PlayerDeath.MaxHealth;
        _fillImage.fillAmount = ratio;
    }
}
