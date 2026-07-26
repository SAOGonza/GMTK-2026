using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game_Level";
    [SerializeField] private SceneFade sceneFade;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip playSound;
    [SerializeField] private AudioClip quitSound;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Volume")) AudioListener.volume = PlayerPrefs.GetFloat("Volume");
    }

    public void PlayGame()
    {
        audioSource.PlayOneShot(playSound);
        sceneFade.LoadScene(gameSceneName);
    }

    public void LoadScene(string name)
    {
        audioSource.PlayOneShot(playSound);
        sceneFade.LoadScene(name);
    }

    public void QuitGame()
    {
        audioSource.PlayOneShot(quitSound);
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
