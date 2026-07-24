using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Swimming")]
    [SerializeField] private float swimSpeed = 3f;
    [SerializeField] private float riseSpeed = 3f;
    [SerializeField] private float sinkSpeed = 1f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedForce = -2f;

    private float verticalVelocity;

    public CharacterController CharacterController { get; private set; }

    public Vector2 MoveInput { get; private set; }

    public bool IsUnderwater { get; private set; }

    public float MovementSpeedMultiplier { get; private set; } = 1f;

    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerSwimState SwimState { get; private set; }

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        SwimState = new PlayerSwimState(this, StateMachine);
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
            return;

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
            return;

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

        Vector3 velocity = moveDirection * moveSpeed * MovementSpeedMultiplier;

        // CharacterController.Move does not apply gravity.
        // This keeps the controller pressed against the ground.
        ApplyGravity();
        velocity.y = verticalVelocity;

        CharacterController.Move(velocity * Time.deltaTime);
    }

    public void SetMovementSpeedMultiplier(float multiplier)
    {
        MovementSpeedMultiplier = Mathf.Max(0f, multiplier);
    }

    private void ApplyGravity()
    {
        if (CharacterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedForce;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    public void ApplyGroundGravity()
    {
        ApplyGravity();

        CharacterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    public void Swim()
    {
        if (cameraTransform == null)
            return;

        verticalVelocity = 0f; // Reset vertical velocity when swimming.

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Keep WASD movement horizontal even when the player looks up or down.
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * MoveInput.y) + (right * MoveInput.x);
        Vector3 velocity = moveDirection * swimSpeed * MovementSpeedMultiplier;

        // Determine if player is rising or sinking based on vertical input.
        bool isRising = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

        velocity.y = isRising ? riseSpeed : -sinkSpeed;

        CharacterController.Move(velocity * Time.deltaTime);
    }

    public void EnterWater()
    {
        IsUnderwater = true;
        StateMachine.ChangeState(SwimState);
    }

    public void ExitWater()
    {
        IsUnderwater = false;
        if (MoveInput.sqrMagnitude > 0f)
            StateMachine.ChangeState(MoveState);
        else
            StateMachine.ChangeState(IdleState);
    }
}
