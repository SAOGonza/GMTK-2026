using UnityEngine;

public class BreathingBehavior : MonoBehaviour
{
    private bool underwater;
    private bool hasDrowned;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip submergeClip;
    [SerializeField] AudioClip emergeClip;

    [Header("Oxygen")]
    [SerializeField] private float oxygenDrainRate = 10f;
    [SerializeField] private float oxygenRecoveryRate = 100f;

    [Header("Bubbles Particles")]
    [SerializeField] GameObject bubblesParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            underwater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            underwater = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance == null)
            return;

        // Stop processing oxygen after Game Over or Victory.
        if (!GameManager.Instance.IsGameActive)
            return;

        if (underwater)
        {
            GameManager.Instance.Oxygen -= Time.deltaTime * oxygenDrainRate;
            GameManager.Instance.Oxygen = Mathf.Max(0f, GameManager.Instance.Oxygen);

            // Play underwater sounds.
            if (!audioSource.isPlaying)
                audioSource.Play();

            // Show bubbles in front of player while underwater.
            if (!bubblesParticles.activeSelf)
                bubblesParticles.SetActive(true);

            CheckForDrowning();
        }

        else
        {
            // Stop underwater sounds.
            if (audioSource.isPlaying)
                audioSource.Stop();

            // Stop bubbles when we emerge to the surface.
            if (bubblesParticles.activeSelf)
                bubblesParticles.SetActive(false);

            GameManager.Instance.Oxygen += Time.deltaTime * oxygenRecoveryRate;
            GameManager.Instance.Oxygen = Mathf.Min(100f, GameManager.Instance.Oxygen);
        }
    }

    private void CheckForDrowning()
    {
        if (hasDrowned)
            return;

        if (GameManager.Instance.Oxygen > 0f)
            return;

        hasDrowned = true;
        GameManager.Instance?.TriggerGameOver();
    }
}
