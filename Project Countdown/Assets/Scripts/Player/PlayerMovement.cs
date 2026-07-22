using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // No movement.
        if (Keyboard.current == null || cameraTransform == null)
            return;

        Vector2 input = Vector2.zero;
        input = CheckPlayerInput(input);

        // Prevent diagonal movement from being faster than straight movement.
        input = Vector2.ClampMagnitude(input, 1f);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Prevent player from moving up and down when the camera is tilted.
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * input.y) + (right * input.x);
        Vector3 movement = moveDirection * moveSpeed;

        // Keep the CharacterController pressed against the ground.
        movement.y = -1f;

        characterController.Move(movement * Time.deltaTime);
    }

    private static Vector2 CheckPlayerInput(Vector2 input)
    {
        if (Keyboard.current.wKey.isPressed) // Forward
            input.y += 1f;

        if (Keyboard.current.sKey.isPressed) // Backward
            input.y -= 1f;

        if (Keyboard.current.dKey.isPressed) // Right
            input.x += 1f;

        if (Keyboard.current.aKey.isPressed) // Left
            input.x -= 1f;
        return input;
    }
}
