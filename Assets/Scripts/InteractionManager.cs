using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction")]
    public float interactionRange = 3f;
    public LayerMask interactionLayer = -1;
    
    [Header("PC UI")]
    public TextMeshProUGUI interactionText; // Только для PC
    
    [Header("VR Settings")]
    public Transform vrController; // Правый контроллер
    public InputActionProperty triggerAction; // XR Input Action
    
    private Camera playerCamera;
    private IInteractable currentInteractable;
    private bool isVRMode;
    
    void Start()
    {
        // Автоматически определяем VR режим
        isVRMode = vrController != null;
        
        if (!isVRMode)
        {
            // PC режим - ищем камеру
            playerCamera = GetComponentInChildren<Camera>();
        }
        else
        {
            // VR режим - включаем input
            triggerAction.action?.Enable();
        }
        
        Debug.Log(isVRMode ? "Запущен VR режим" : "Запущен PC режим");
    }
    
    void Update()
    {
        CheckForInteractable();
        
        if (GetInteractionInput() && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
    
    bool GetInteractionInput()
    {
        if (isVRMode)
        {
            // VR: XR Toolkit Input Action
            return triggerAction.action?.WasPressedThisFrame() ?? false;
        }
        else
        {
            // PC: Клавиша E
            return Input.GetKeyDown(KeyCode.E);
        }
    }
    
    void CheckForInteractable()
    {
        Ray ray = GetInteractionRay();
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null)
            {
                SetInteractable(interactable);
                return;
            }
        }
        
        SetInteractable(null);
    }
    
    Ray GetInteractionRay()
    {
        if (isVRMode && vrController != null)
        {
            // VR: Луч из контроллера
            return new Ray(vrController.position, vrController.forward);
        }
        else if (playerCamera != null)
        {
            // PC: Луч из центра экрана
            return playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        }
        else
        {
            // Fallback
            return new Ray(transform.position, transform.forward);
        }
    }
    
    void SetInteractable(IInteractable interactable)
    {
        // Hide previous VR text
        if (currentInteractable != null && isVRMode)
        {
            MonoBehaviour prevComponent = currentInteractable as MonoBehaviour;
            if (prevComponent != null)
            {
                VRTextController vrText = prevComponent.GetComponent<VRTextController>();
                if (vrText != null) vrText.HideText();
            }
        }
        
        currentInteractable = interactable;
        
        if (!isVRMode && interactionText != null)
        {
            // PC: central UI text
            if (interactable != null)
            {
                interactionText.text = "[E] " + interactable.GetInteractionText();
                interactionText.gameObject.SetActive(true);
            }
            else
            {
                interactionText.gameObject.SetActive(false);
            }
        }
        else if (isVRMode && interactable != null)
        {
            // VR: show text on object
            MonoBehaviour component = interactable as MonoBehaviour;
            if (component != null)
            {
                VRTextController vrText = component.GetComponent<VRTextController>();
                if (vrText != null) vrText.ShowText();
            }
        }
    }
    
    void OnDestroy()
    {
        // Отключаем VR input при уничтожении
        if (isVRMode && triggerAction.action != null)
        {
            triggerAction.action.Disable();
        }
    }
    
    // Debug информация
    void OnDrawGizmosSelected()
    {
        // Показываем луч взаимодействия в Scene view
        Ray ray = GetInteractionRay();
        Gizmos.color = currentInteractable != null ? Color.green : Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * interactionRange);
    }
}