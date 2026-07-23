using UnityEngine;
using UnityEngine.UI;

public class MonsterGaugeUI : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private Slider monsterGaugeSlider;

    private void Awake()
    {
        if (monsterGaugeSlider != null)
        {
            monsterGaugeSlider.minValue = 0f;
            monsterGaugeSlider.maxValue = 1f;
            monsterGaugeSlider.interactable = false;
        }
    }

    private void OnEnable()
    {
        if (gameTimer != null)
            gameTimer.OnGaugeChanged += UpdateGauge;
    }

    private void Start()
    {
        if (gameTimer != null)
            UpdateGauge(gameTimer.CurrentGauge);
    }

    private void OnDisable()
    {
        if (gameTimer != null)
            gameTimer.OnGaugeChanged -= UpdateGauge;
    }

    private void UpdateGauge(float gaugeValue)
    {
        if (monsterGaugeSlider == null)
            return;

        monsterGaugeSlider.value = gaugeValue;
    }
}
