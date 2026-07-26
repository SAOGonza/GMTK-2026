using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Swimming")]
    [SerializeField] private float swimSpeed = 3f;
    [SerializeField] private float swimBoostMultiplier = 1.35f;

    [SerializeField] private float idleBeforeSinking = 2f;

    [SerializeField] private float riseAcceleration = 2f;
    [SerializeField] private float sinkAcceleration = 1.5f;

    [SerializeField] private float maxRiseSpeed = 2.5f;
    [SerializeField] private float maxSinkSpeed = 1.5f;

    private float swimIdleTimer;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedForce = -2f;

    [Header("Game Timer")]
    [SerializeField] GameTimer gameTimer;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] antidoteConsumeSFX;
    [SerializeField] private AudioClip introVoiceLine;

    private PlayerInventory inventory;

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
        inventory = GetComponent<PlayerInventory>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        SwimState = new PlayerSwimState(this, StateMachine);


    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);

        Invoke(nameof(PlayIntroVoiceLine), 2f);
    }

    private void Update()
    {
        ReadMovementInput();
        ReadAntidoteInput();

        StateMachine.CurrentState?.Update();
    }

    private void PlayIntroVoiceLine()
    {
        if (audioSource == null || introVoiceLine == null)
            return;

        audioSource.PlayOneShot(introVoiceLine);
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

    private void ReadAntidoteInput()
    {
        if (Keyboard.current == null || !Keyboard.current.digit1Key.wasPressedThisFrame)
            return;

        if (
            gameTimer == null ||
            gameTimer.IsAntidoteActive ||
            inventory == null ||
            inventory.AntidoteCount <= 0
        )
            return;

        inventory.RemoveAntidote(1);
        PlayAntidoteSound();
        gameTimer.ApplyAntidote();
    }

    private void PlayAntidoteSound()
    {
        if (audioSource == null || antidoteConsumeSFX == null || antidoteConsumeSFX.Length == 0)
            return;

        int randomIndex = Random.Range(0, antidoteConsumeSFX.Length);
        audioSource.PlayOneShot(antidoteConsumeSFX[randomIndex]);
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

        bool isRising = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

        bool isBoosting =
            Keyboard.current != null &&
            (
                Keyboard.current.leftShiftKey.isPressed ||
                Keyboard.current.rightShiftKey.isPressed
            );

        bool hasDirectionalInput =
            MoveInput.sqrMagnitude > 0.01f;

        UpdateSwimVerticalVelocity(hasDirectionalInput, isRising);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * MoveInput.y) + (right * MoveInput.x);

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        float currentSwimSpeed = swimSpeed;

        if (isBoosting)
            currentSwimSpeed *= swimBoostMultiplier;

        Vector3 velocity = moveDirection * currentSwimSpeed * MovementSpeedMultiplier;

        velocity.y += verticalVelocity;

        CharacterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateSwimVerticalVelocity(bool hasDirectionalInput,bool isRising)
    {
        if (isRising)
        {
            swimIdleTimer = 0f;
            verticalVelocity = Mathf.MoveTowards(verticalVelocity, maxRiseSpeed, riseAcceleration * Time.deltaTime);

            return;
        }

        if (hasDirectionalInput)
        {
            swimIdleTimer = 0f;
            verticalVelocity = Mathf.MoveTowards(verticalVelocity, 0f, sinkAcceleration * Time.deltaTime);

            return;
        }

        swimIdleTimer += Time.deltaTime;

        if (swimIdleTimer < idleBeforeSinking)
        {
            verticalVelocity = Mathf.MoveTowards(verticalVelocity, 0f, sinkAcceleration * Time.deltaTime);
            return;
        }

        verticalVelocity = Mathf.MoveTowards(verticalVelocity, -maxSinkSpeed, sinkAcceleration * Time.deltaTime);
    }

    public void EnterWater()
    {
        if (IsUnderwater)
            return;

        IsUnderwater = true;

        verticalVelocity = 0f;
        swimIdleTimer = 0f;

        StateMachine.ChangeState(SwimState);
    }

    public void ExitWater()
    {
        if (!IsUnderwater)
            return;

        IsUnderwater = false;

        verticalVelocity = 0f;
        swimIdleTimer = 0f;

        if (MoveInput.sqrMagnitude > 0f)
            StateMachine.ChangeState(MoveState);
        else
            StateMachine.ChangeState(IdleState);
    }
}
