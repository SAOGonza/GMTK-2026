using UnityEngine;
using UnityEngine.Events;

/* This script does only four things:
1. Provide interaction text
2. Add one power cell
3. Spawn optional VFX
4. Destroy itself
*/

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] private PickupData pickupData;

    public string InteractionPrompt
    {
        get
        {
            if (pickupData == null)
                return "Press E to pick up";

            return $"Press E to pick up {pickupData.PickupName}" + $"\n\n{pickupData.Description}";
        }
    }

    public virtual void Interact(Player player)
    {
        SpawnPickupVFX();
        PlayPickupSFX();
        Destroy(gameObject);
    }

    private void SpawnPickupVFX()
    {
        if (pickupData == null || pickupData.PickupVFX == null)
            return;

        Instantiate(pickupData.PickupVFX, transform.position, Quaternion.identity);
    }

    private void PlayPickupSFX()
    {
        if (pickupData == null || pickupData.PickupSFX == null)
            return;

        AudioSource.PlayClipAtPoint(pickupData.PickupSFX, transform.position);
    }
}