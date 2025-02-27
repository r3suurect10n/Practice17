using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    private float moveInput;
    private bool jumpPressed;

    [SerializeField] private float moveSpeed = 5f; // Скорость движения
    [SerializeField] private float jumpForce = 5f; // Сила прыжка
    [SerializeField] private LayerMask groundLayer; // Слой земли
    [SerializeField] private Transform groundCheck; // Точка проверки земли
    [SerializeField] private float checkRadius = 0.2f; // Радиус проверки
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInput(); // Инициализация Input System
    }

    void OnEnable()
    {
        playerInput.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>().x;
            Debug.Log("Move Input: " + moveInput);
        };
        playerInput.Player.Move.canceled += ctx =>
        {
            moveInput = 0f;
            Debug.Log("Move Input: " + moveInput);
        };
        playerInput.Player.Jump.performed += ctx => jumpPressed = true;
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        // Проверка земли
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);

        // Прыжок
        if (jumpPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jumpPressed = false; // Сбрасываем флаг прыжка
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput * moveSpeed, rb.linearVelocity.y, 0f);
        rb.linearVelocity = movement;
    }

    // Визуализация проверки земли в редакторе
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}