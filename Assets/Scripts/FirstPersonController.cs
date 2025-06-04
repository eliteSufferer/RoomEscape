using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    
    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float upDownRange = 80f;
    
    private CharacterController characterController;
    private Camera playerCamera;
    private float verticalRotation = 0;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        
        // Заблокировать курсор в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // Движение мышью
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // Поворот по горизонтали
        transform.Rotate(0, mouseX, 0);
        
        // Поворот по вертикали
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        
        // Движение WASD
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(movement * speed * Time.deltaTime);
        
        // ESC для разблокировки курсора
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}