/* ANYTHING THE PLAYER CAN INTERACT WITH WILL IMPLEMENT THIS INTERFACE */
public interface IInteractable
{
    string InteractionPrompt { get; }

    void Interact(Player player);
}
