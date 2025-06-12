using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    
    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float upDownRange = 80f;
    
    [Header("Gravity")]
    public float gravity = -20f; // 🔥 ДОБАВИЛИ ГРАВИТАЦИЮ
    public float groundCheckDistance = 0.3f;
    
    private CharacterController characterController;
    private Camera playerCamera;
    private float verticalRotation = 0;
    private Vector3 velocity; // 🔥 Для вертикальной скорости
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        
        // Заблокировать курсор в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // Не двигаться и не крутить камеру на паузе
        if (Time.timeScale == 0f) return;
        
        HandleMouseLook();
        HandleMovement();
        HandleGravity(); // 🔥 НОВАЯ ФУНКЦИЯ
    }
    
    void HandleMouseLook()
    {
        // 🔥 НЕ КРУТИТЬ КАМЕРУ ЕСЛИ КУРСОР РАЗБЛОКИРОВАН (UI открыт)
        if (Cursor.lockState != CursorLockMode.Locked)
            return;
            
        // Движение мышью
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Поворот по горизонтали
        transform.Rotate(0, mouseX, 0);
        
        // Поворот по вертикали
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
    
    void HandleMovement()
    {
        // 🔥 НЕ ДВИГАТЬСЯ ЕСЛИ КУРСОР РАЗБЛОКИРОВАН (UI открыт)
        if (Cursor.lockState != CursorLockMode.Locked)
            return;
            
        // Движение WASD
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // 🔥 ТОЛЬКО ГОРИЗОНТАЛЬНОЕ ДВИЖЕНИЕ (без Y)
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        movement = Vector3.ClampMagnitude(movement, 1f); // Нормализация диагонали
        
        characterController.Move(movement * speed * Time.deltaTime);
    }
    
    void HandleGravity()
    {
        // 🔥 ГРАВИТАЦИЯ И ПРОВЕРКА ЗЕМЛИ
        bool isGrounded = IsGrounded();
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Небольшая сила прижатия к земле
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Падение
        }
        
        // Применяем вертикальную скорость
        characterController.Move(velocity * Time.deltaTime);
    }
    
    bool IsGrounded()
    {
        // 🔥 Raycast вниз для проверки земли
        Vector3 rayStart = transform.position;
        float rayDistance = (characterController.height / 2f) + groundCheckDistance;
        
        return Physics.Raycast(rayStart, Vector3.down, rayDistance);
    }
    
    // 🔥 Debug визуализация (опционально)
    void OnDrawGizmosSelected()
    {
        if (characterController != null)
        {
            Gizmos.color = Color.red;
            Vector3 rayStart = transform.position;
            float rayDistance = (characterController.height / 2f) + groundCheckDistance;
            Gizmos.DrawRay(rayStart, Vector3.down * rayDistance);
        }
    }
}