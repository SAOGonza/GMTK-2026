using UnityEngine;

public class RotatePickupController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
