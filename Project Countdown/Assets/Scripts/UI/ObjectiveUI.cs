using System.Collections;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject objectiveUI;

    [Header("Timing")]
    [SerializeField] private float delayBeforeShowing = 7f;
    [SerializeField] private float displayDuration = 25f;

    private void Awake()
    {
        // Make sure it is hidden when the scene begins.
        objectiveUI.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(ShowObjectiveRoutine());
    }

    private IEnumerator ShowObjectiveRoutine()
    {
        // Wait after entering Game_Level.
        yield return new WaitForSeconds(delayBeforeShowing);

        // Show the objective.
        objectiveUI.SetActive(true);

        // Give the player time to read it.
        yield return new WaitForSeconds(displayDuration);

        // Hide it for the rest of the scene.
        objectiveUI.SetActive(false);
    }
}
