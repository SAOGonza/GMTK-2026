using UnityEngine;

/* This script does only four things:
1. Provide interaction text
2. Add one power cell
3. Spawn optional VFX
4. Destroy itself
*/

[RequireComponent(typeof(Collider))]
public class PowerCellPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private PickupData pickupData;

    public string InteractionPrompt
    {
        get
        {
            if (pickupData == null)
                return "Press E to pick up";

            return $"Press E to pick up {pickupData.PickupName}";
        }
    }

    public void Interact(Player player)
    {
        if (player == null)
            return;

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        // Error handling if the PlayerInventory component is missing
        if (inventory == null)
        {
            Debug.LogWarning("Player is missing PlayerInventory component.");
            return;
        }

        inventory.AddPowerCell();
        SpawnPickupVFX();
        Destroy(gameObject);
    }

    private void SpawnPickupVFX()
    {
        if (pickupData == null || pickupData.PickupVFX == null)
            return;

        Instantiate(pickupData.PickupVFX, transform.position, Quaternion.identity);
    }
}
