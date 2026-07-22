using UnityEngine;

[CreateAssetMenu(
    fileName = "New Pickup Data",
    menuName = "Game/Pickup Data")]
public class PickupData : ScriptableObject
{
    [Header("Information")]
    [SerializeField] private string pickupName;

    [Header("Presentation")]
    [SerializeField] private GameObject pickupVFX;

    public string PickupName => pickupName;
    public GameObject PickupVFX => pickupVFX;
}
