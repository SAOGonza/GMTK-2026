using UnityEngine;

public class BreathingBehavior : MonoBehaviour
{
    private bool underwater;
    private bool hasDrowned;

    [Header("Oxygen")]
    [SerializeField] private float oxygenDrainRate = 10f;
    [SerializeField] private float oxygenRecoveryRate = 100f;

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

            CheckForDrowning();
        }

        else
        {
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
