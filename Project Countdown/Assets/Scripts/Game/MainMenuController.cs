using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game_Level";

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeDuration = 1f;

    private bool isTransitioning;

    private void Awake()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 0f;
            fadeOverlay.interactable = false;
            fadeOverlay.blocksRaycasts = false;
        }
    }

    public void PlayGame()
    {
        if (isTransitioning)
            return;

        StartCoroutine(FadeToGame());
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator FadeToGame()
    {
        isTransitioning = true;

        if (fadeOverlay != null)
            fadeOverlay.blocksRaycasts = true;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float fadeAmount = Mathf.Clamp01(elapsed / fadeDuration);

            if (fadeOverlay != null)
                fadeOverlay.alpha = fadeAmount;

            yield return null;
        }

        if (fadeOverlay != null)
            fadeOverlay.alpha = 1f;

        SceneManager.LoadScene(gameSceneName);
    }
}
