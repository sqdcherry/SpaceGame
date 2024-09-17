using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject blaster1;
    [SerializeField] private GameObject blaster2;
    [SerializeField] private GameObject crashText;

    [SerializeField] private int _maxHeathPoints = 3;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _scaleRotation = 5;
    [SerializeField] private float _shootForce = 10;
    [SerializeField] public int _currentHeathPoints;
    [SerializeField] public int _damage = 1;

    private BoxCollider col; 
    private Rigidbody rb; 
    private Vector3 move;
    private Vector2 rotation;
    private Camera mainCamera;
    private Animator anim;

    private GameInput gameInput;

    private void Awake()
    {
        gameInput = new GameInput();
    }

    private void Start()
    {
        _currentHeathPoints = _maxHeathPoints;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        ReadMovement();

        var dir = new Vector3(1f, 0, 0);
        transform.Translate((dir * _moveSpeed) * Time.fixedDeltaTime);
    }

    private void OnEnable()
    {
        gameInput.Enable();
        gameInput.Gameplay.Shoot.performed += Shoot;

        Asteroid.onCrash += Crash;
    }

    private void OnDisable()
    {
        gameInput.Disable();
        Asteroid.onCrash -= Crash;
    }

    private void ReadMovement()
    {
        var inputDirection = gameInput.Gameplay.Movement.ReadValue<Vector3>();
        var direction = new Vector3(inputDirection.x, inputDirection.y, 0f);

        MoveRotation(direction);
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        GameObject currentBullet;
        int numberOfBlaster = UnityEngine.Random.Range(1, 3);

        if (numberOfBlaster == 1)
        {
            currentBullet = Instantiate(bulletPrefab, blaster1.transform.position, blaster1.transform.rotation);
            currentBullet.GetComponent<Rigidbody>().velocity = blaster1.transform.forward * _shootForce;
        }
        else
        {
            currentBullet = Instantiate(bulletPrefab, blaster2.transform.position, blaster2.transform.rotation);
            currentBullet.GetComponent<Rigidbody>().velocity = blaster2.transform.forward * _shootForce;
        }

        currentBullet.GetComponent<Rigidbody>().velocity = blaster1.transform.forward * _shootForce;
        Debug.Log("Shoot");
        Destroy(currentBullet, 5f);
    }

    private void MoveRotation(Vector3 direction)
    {
        rotation = direction * _scaleRotation;

        transform.Rotate(Vector3.up, rotation.x, Space.World);
        transform.Rotate(mainCamera.transform.right, -rotation.y, Space.World);
    }

    public void GetDamage(int damage)
    {
        _currentHeathPoints -= damage;

        Debug.Log("Player get damage");

        if (_currentHeathPoints <= 0)
        {
            Debug.Log("Player is dead");
            Time.timeScale = 0;
        }
    }

    private void Crash()
    {
        StartCoroutine("CrashCoroutine");
    }

    IEnumerator CrashCoroutine()
    {
        float currentMoveSpeed = _moveSpeed;
        float currentRotationScale = _scaleRotation;
        _moveSpeed = 0;
        _scaleRotation = 0;
        crashText.SetActive(true);
        anim.SetTrigger("Crash");
        yield return new WaitForSeconds(0.6f);
        crashText.SetActive(true);
        _moveSpeed = currentMoveSpeed;
        _scaleRotation = currentRotationScale;
    }
}
