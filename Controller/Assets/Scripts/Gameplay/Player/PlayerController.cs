using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    private const float GRAVITY_FORCE = -25.0f, GROUND_DISTANCE = 0.2f;
 
    private Vector3 _movementDirection, _movementInput, _playerVelocity;

    private float _horizontalAxis, _verticalAxis, _currentSpeed;

    private enum PlayerState
    {
        Idle,
        Walk, 
        Run,
        Crouch
    }

    [Header("Main Components")]
    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private GameManager gameManager;

    [Header("Movement Stats")]
    [SerializeField]
    private float movementSpeed = 10.0f;

    [SerializeField]
    private float jumpForce = 10.0f;

    [Header("Crouch Stats")]
    [SerializeField]
    private PlayerState playerState = PlayerState.Idle;

    [SerializeField]
    private float crouchSpeed = 5.0f;

    [SerializeField]
    private float standartHeight = 2.0f;

    [SerializeField]
    private float crouchHeight = 1.0f;

    [Header("Ground Detection")]
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    private bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, GROUND_DISTANCE, groundLayer);
    }

    private void Start()
    {
        _currentSpeed = movementSpeed;

        gameManager = FindObjectOfType<GameManager>();

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
            return;
        }
    }

    private void Update()
    {
        MyInput();
        HandleMovement();
        HandleJump();
        HandleSprint();
        HandleCrouch();
    }

    private void MyInput()
    {
        Vector2 input = gameManager.inputActions.Player.Movement.ReadValue<Vector2>();

        _horizontalAxis = input.x;
        _verticalAxis = input.y;
    }

    private void HandleMovement()
    {
        if (playerState == PlayerState.Walk)
        {
            movementSpeed = _currentSpeed;
        }

        _movementInput = transform.forward * _verticalAxis + transform.right * _horizontalAxis;
        _movementDirection = _movementInput.normalized;

        characterController.Move(_movementDirection * movementSpeed * Time.deltaTime);

        if (_movementDirection.sqrMagnitude > 0.0f) playerState = PlayerState.Walk;

        else playerState = PlayerState.Idle;

        _playerVelocity.y += GRAVITY_FORCE * Time.deltaTime;
        characterController.Move(_playerVelocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (isGrounded() && gameManager.inputActions.Player.Jump.triggered)
        {
            _playerVelocity.y = jumpForce;
        }
    }

    private void HandleCrouch()
    {
        if (gameManager.inputActions.Player.Crouch.IsPressed() && playerState != PlayerState.Run) 
        {
            movementSpeed = _currentSpeed / 2.0f;
            playerState = PlayerState.Crouch;
            characterController.height = Mathf.Lerp(characterController.height, crouchHeight, crouchSpeed * Time.deltaTime);
        }
        else
        {
            characterController.height = Mathf.Lerp(characterController.height, standartHeight, crouchSpeed * Time.deltaTime); ;
        }
    }

    private void HandleSprint()
    {
        if (gameManager.inputActions.Player.Sprint.IsPressed() && playerState != PlayerState.Crouch)
        {
            playerState = PlayerState.Run;
            movementSpeed = _currentSpeed + 2;
        }
    }
}