using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TMP_Text interactionPromptText;

    [Header("Voice Lines")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] pickupVoiceLines;
    [SerializeField] private AudioClip[] powerCellPickupVoiceLines;

    private Player player;

    private IInteractable currentInteractable;

    private void Awake()
    {
        player = GetComponent<Player>();

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Start()
    {
        HidePrompt();
    }

    private void Update()
    {
        DetectInteractable();
        HandleInteractionInput();
    }

    private void DetectInteractable()
    {
        currentInteractable = null;

        if (playerCamera == null)
        {
            HidePrompt();
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer);

        if (!hitSomething)
        {
            HidePrompt();
            return;
        }

        currentInteractable = hit.collider.GetComponentInParent<IInteractable>();

        if (currentInteractable == null)
        {
            HidePrompt();
            return;
        }

        IContextualInteractable contextualInteractable = currentInteractable as IContextualInteractable;
        contextualInteractable?.SetInteractor(player);

        ShowPrompt(currentInteractable.InteractionPrompt);
    }

    private void HandleInteractionInput()
    {
        if (currentInteractable == null || Keyboard.current == null || !Keyboard.current.eKey.wasPressedThisFrame)
            return;

        int index = Random.Range(0, pickupVoiceLines.Length);


        if (currentInteractable is PowerCellPickup)
            PlayRandomVoiceLine(powerCellPickupVoiceLines);
        else
            PlayRandomVoiceLine(pickupVoiceLines);


        currentInteractable.Interact(player);
    }

    private void PlayRandomVoiceLine(AudioClip[] voiceLines)
    {
        if (
            audioSource == null ||
            voiceLines == null ||
            voiceLines.Length == 0
        )
        {
            return;
        }

        int randomIndex = Random.Range(0, voiceLines.Length);
        audioSource.PlayOneShot(voiceLines[randomIndex]);
    }

    private void ShowPrompt(string message)
    {
        if (interactionPromptText == null)
        {
            return;
        }

        interactionPromptText.gameObject.SetActive(true);
        interactionPromptText.text = message;
    }

    private void HidePrompt()
    {
        if (interactionPromptText == null)
        {
            return;
        }

        interactionPromptText.gameObject.SetActive(false);
    }
}
