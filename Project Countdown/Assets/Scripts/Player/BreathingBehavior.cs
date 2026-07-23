using UnityEngine;

public class BreathingBehavior : MonoBehaviour
{
    private bool Underwater = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Underwater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Underwater = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Underwater)
        {
            GameManager.Instance.Oxygen -= Time.deltaTime * 10f;
            GameManager.Instance.Oxygen = Mathf.Max(0f, GameManager.Instance.Oxygen);
        }
        else
        {
            GameManager.Instance.Oxygen += Time.deltaTime * 100f;
            GameManager.Instance.Oxygen = Mathf.Min(100f, GameManager.Instance.Oxygen);
        }
    }
}
