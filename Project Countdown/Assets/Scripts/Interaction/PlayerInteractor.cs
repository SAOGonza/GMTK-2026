using NUnit.Framework.Constraints;
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
        if (currentInteractable == null || Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
            currentInteractable.Interact(player);
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
