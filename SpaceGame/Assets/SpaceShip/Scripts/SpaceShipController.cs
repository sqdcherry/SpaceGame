using UnityEngine;
using System;

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] private int _maxHeathPoints = 3;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _scaleSpeed = 10;
    [SerializeField] private float _scaleRotation = 3;
    [SerializeField] private int _curentHeathPoints;

    private BoxCollider col; 
    private Rigidbody rb; 
    private Vector3 move;
    private Vector2 rotation;
    private Transform _mainCamera;


    private GameInput _gameInput;

    private void Awake()
    {
        _gameInput = new GameInput();
    }

    private void Start()
    {
        _curentHeathPoints = _maxHeathPoints;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        _mainCamera = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        ReadMovement();

        transform.Translate(move * Time.fixedDeltaTime);
    }

    private void OnEnable()
    {
        _gameInput.Enable();
    }

    private void OnDisable()
    {
        _gameInput.Disable();
    }

    private void ReadMovement()
    {
        var inputDirection = _gameInput.Gameplay.Movement.ReadValue<Vector3>();
        var direction = new Vector3(inputDirection.x, inputDirection.y, 0f);

        //Move(direction);
        MoveRotation(direction);
    }

    private void Move(Vector3 direction)
    {
        float scaledMoveSpeed = _scaleSpeed * Time.deltaTime;

        move = new Vector3(direction.x, direction.y, 0f);
        transform.position += move * scaledMoveSpeed;
    }

    private void MoveRotation(Vector3 direction)
    {
        rotation = direction * 1f;

        transform.Rotate(Vector3.up, rotation.x, Space.World);
        transform.Rotate(-_mainCamera.right, -rotation.y, Space.World);
    }
}
