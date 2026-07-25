using UnityEngine;

[CreateAssetMenu(
    fileName = "New Pickup Data",
    menuName = "Game/Pickup Data")]
public class PickupData : ScriptableObject
{
    [Header("Information")]
    [SerializeField] private string pickupName;
    [TextArea(15, 20)]
    [SerializeField] private string description = "";

    [Header("Presentation")]
    [SerializeField] private GameObject pickupVFX;
    [SerializeField] private AudioClip pickupSFX;

    public string PickupName => pickupName;
    public string Description => description;
    public GameObject PickupVFX => pickupVFX;
    public AudioClip PickupSFX => pickupSFX;
}
