using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InfectionEffectsController : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private Player player;

    [Header("Visual References")]
    [SerializeField] private CanvasGroup infectionOverlay;
    [SerializeField] private CanvasGroup gameOverScreen;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stage75Clip;
    [SerializeField] private AudioClip stage50Clip;
    [SerializeField] private AudioClip stage25Clip;
    [SerializeField] private AudioClip transformationClip;

    [Header("Camera")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("75 Percent Effect")]
    [SerializeField] private float stage75Duration = 6f;
    [SerializeField, Range(0f, 1f)]
    private float stage75OverlayStrength = 0.2f;

    [Header("50 Percent Effect")]
    [SerializeField] private float stage50Duration = 6f;
    [SerializeField, Range(0f, 1f)]
    private float stage50OverlayStrength = 0.4f;

    [Header("25 Percent Effect")]
    [SerializeField] private float stage25Duration = 6f;
    [SerializeField, Range(0f, 1f)]
    private float stage25OverlayStrength = 0.65f;
    [SerializeField, Range(0f, 1f)]
    private float stumbleSpeedMultiplier = 0.4f;

    [Header("Game Over")]
    [SerializeField] private float gameOverFadeDuration = 2f;

    private bool triggered75;
    private bool triggered50;
    private bool triggered25;
    private bool triggeredZero;

    private Coroutine activeEffect;

    private void Awake()
    {
        if (infectionOverlay != null)
        {
            infectionOverlay.alpha = 0f;
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.alpha = 0f;
        }
    }

    private void OnEnable()
    {
        if (gameTimer != null)
        {
            gameTimer.OnGaugeChanged += HandleGaugeChanged;
        }
    }

    private void Start()
    {
        if (gameTimer != null)
        {
            HandleGaugeChanged(gameTimer.CurrentGauge);
        }
    }

    private void OnDisable()
    {
        if (gameTimer != null)
        {
            gameTimer.OnGaugeChanged -= HandleGaugeChanged;
        }
    }

    private void HandleGaugeChanged(float gaugeValue)
    {
        if (!triggered75 && gaugeValue <= 0.75f)
        {
            triggered75 = true;

            StartStageEffect(
                stage75Clip,
                stage75Duration,
                stage75OverlayStrength,
                false
            );
        }

        if (!triggered50 && gaugeValue <= 0.50f)
        {
            triggered50 = true;

            StartStageEffect(
                stage50Clip,
                stage50Duration,
                stage50OverlayStrength,
                false
            );
        }

        if (!triggered25 && gaugeValue <= 0.25f)
        {
            triggered25 = true;

            StartStageEffect(
                stage25Clip,
                stage25Duration,
                stage25OverlayStrength,
                true
            );
        }

        if (!triggeredZero && gaugeValue <= 0f)
        {
            triggeredZero = true;

            if (activeEffect != null)
            {
                StopCoroutine(activeEffect);
            }

            StartCoroutine(PlayTransformationSequence());
        }
    }

    private void StartStageEffect(
        AudioClip clip,
        float duration,
        float overlayStrength,
        bool useStumble)
    {
        if (activeEffect != null)
        {
            StopCoroutine(activeEffect);
            RestoreTemporaryEffects();
        }

        activeEffect = StartCoroutine(
            PlayStageEffect(
                clip,
                duration,
                overlayStrength,
                useStumble
            )
        );
    }

    private IEnumerator PlayStageEffect(
        AudioClip clip,
        float duration,
        float overlayStrength,
        bool useStumble)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }

        if (useStumble)
        {
            player?.SetMovementSpeedMultiplier(
                stumbleSpeedMultiplier
            );

            impulseSource?.GenerateImpulse();
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float normalizedTime =
                Mathf.Clamp01(elapsed / duration);

            /*
             * Creates several heartbeat-like pulses:
             * 0 -> 1 -> 0 repeatedly.
             */
            float pulse =
                Mathf.Pow(Mathf.Abs(Mathf.Sin(normalizedTime * Mathf.PI * 6f)), 3f);

            if (infectionOverlay != null)
            {
                infectionOverlay.alpha = pulse * overlayStrength;
            }

            yield return null;
        }

        RestoreTemporaryEffects();

        activeEffect = null;
    }

    private IEnumerator PlayTransformationSequence()
    {
        RestoreTemporaryEffects();

        player?.SetMovementSpeedMultiplier(0f);

        if (transformationClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(transformationClip);
        }

        float elapsed = 0f;

        while (elapsed < gameOverFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float fade = Mathf.Clamp01(elapsed / gameOverFadeDuration);

            if (gameOverScreen != null)
            {
                gameOverScreen.alpha = fade;
            }

            yield return null;
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.alpha = 1f;
        }
    }

    private void RestoreTemporaryEffects()
    {
        if (infectionOverlay != null)
        {
            infectionOverlay.alpha = 0f;
        }

        player?.SetMovementSpeedMultiplier(1f);
    }
}