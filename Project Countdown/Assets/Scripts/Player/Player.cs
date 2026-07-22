using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundedForce = -1f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    public CharacterController CharacterController { get; private set; }

    public Vector2 MoveInput { get; private set; }

    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        ReadMovementInput();

        StateMachine.CurrentState?.Update();
    }

    private void ReadMovementInput()
    {
        MoveInput = Vector2.zero;

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.wKey.isPressed)
            MoveInput += Vector2.up;

        if (Keyboard.current.sKey.isPressed)
            MoveInput += Vector2.down;

        if (Keyboard.current.aKey.isPressed)
            MoveInput += Vector2.left;

        if (Keyboard.current.dKey.isPressed)
            MoveInput += Vector2.right;

        MoveInput = Vector2.ClampMagnitude(MoveInput, 1f);
    }

    public void Move()
    {
        if (cameraTransform == null)
        {
            return;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Keep movement flat even when the player looks up or down.
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection =
            forward * MoveInput.y +
            right * MoveInput.x;

        Vector3 velocity = moveDirection * moveSpeed;

        // CharacterController.Move does not apply gravity.
        // This keeps the controller pressed against the ground.
        velocity.y = groundedForce;

        CharacterController.Move(velocity * Time.deltaTime);
    }
}
