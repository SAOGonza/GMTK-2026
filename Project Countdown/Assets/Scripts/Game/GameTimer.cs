using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;
    [Header("Monster Gauge")]
    [SerializeField] private float transformationDuration = 600f;

    public float CurrentGauge { get; private set; } = 1f;

    public event Action<float> OnGaugeChanged;
    public event Action OnGaugeDepleted;

    private bool isDepleted;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetGauge();
    }

    private void Update()
    {
        if (isDepleted || GameManager.Instance == null || !GameManager.Instance.IsGameActive)
            return;

        DepleteGauge();
    }

    private void DepleteGauge()
    {
        if (transformationDuration <= 0f)
        {
            SetGauge(0f);
            HandleGaugeDepleted();
            return;
        }

        float depletionAmount = Time.deltaTime / transformationDuration;

        SetGauge(CurrentGauge - depletionAmount);

        if (CurrentGauge <= 0f)
            HandleGaugeDepleted();
    }

    private void SetGauge(float newValue)
    {
        CurrentGauge = Mathf.Clamp01(newValue);

        OnGaugeChanged?.Invoke(CurrentGauge);
    }

    private void HandleGaugeDepleted()
    {
        if (isDepleted)
            return;

        isDepleted = true;

        OnGaugeDepleted?.Invoke();

        GameManager.Instance?.TriggerGameOver();
    }

    public void ResetGauge()
    {
        isDepleted = false;

        SetGauge(1f);
    }
}
