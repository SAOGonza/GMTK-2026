using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class GenericInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteract;

    public string InteractionPrompt => "Interact";

    public void Interact(Player player)
    {
        OnInteract.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
