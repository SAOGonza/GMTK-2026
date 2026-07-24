using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeDuration = 1f;

    public bool isFading { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadScene(string sceneName)
    {
        if (isFading)
            return;

        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        isFading = true;

        fadeOverlay.gameObject.SetActive(true);
        fadeOverlay.blocksRaycasts = true;
        fadeOverlay.alpha = 1f;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            
            fadeOverlay.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);

            yield return null;
        }

        fadeOverlay.alpha = 0f;
        fadeOverlay.blocksRaycasts = false;

        isFading = false;
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isFading = true;

        fadeOverlay.gameObject.SetActive(true);
        fadeOverlay.blocksRaycasts = true;

        
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            
            fadeOverlay.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        fadeOverlay.alpha = 1f;
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
