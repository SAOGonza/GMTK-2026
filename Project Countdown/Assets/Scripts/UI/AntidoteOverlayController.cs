using UnityEngine;

public class AntidoteOverlayController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private CanvasGroup antidoteOverlay;

    [Header("Flicker")]
    [SerializeField, Range(0f, 1f)]
    private float minimumAlpha = 0.08f;

    [SerializeField, Range(0f, 1f)]
    private float maximumAlpha = 0.18f;

    [SerializeField]
    private float flickerSpeed = 5f;

    private bool isActive;

    private void Awake()
    {
        HideOverlay();

        if (antidoteOverlay != null)
        {
            antidoteOverlay.interactable = false;
            antidoteOverlay.blocksRaycasts = false;
        }
    }

    private void OnEnable()
    {
        if (gameTimer == null)
            return;

        gameTimer.OnAntidoteStarted += ShowOverlay;
        gameTimer.OnAntidoteEnded += HideOverlay;
    }

    private void OnDisable()
    {
        if (gameTimer == null)
            return;

        gameTimer.OnAntidoteStarted -= ShowOverlay;
        gameTimer.OnAntidoteEnded -= HideOverlay;
    }

    private void Update()
    {
        if (!isActive || antidoteOverlay == null)
            return;

        float flicker =
            (Mathf.Sin(Time.time * flickerSpeed) + 1f) * 0.5f;

        antidoteOverlay.alpha = Mathf.Lerp(
            minimumAlpha,
            maximumAlpha,
            flicker
        );
    }

    private void ShowOverlay()
    {
        isActive = true;
    }

    private void HideOverlay()
    {
        isActive = false;

        if (antidoteOverlay != null)
            antidoteOverlay.alpha = 0f;
    }
}
