using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction")]
    public float interactionRange = 3f;
    public LayerMask interactionLayer = -1;
    
    [Header("UI")]
    public TextMeshProUGUI interactionText;
    
    private Camera playerCamera;
    private IInteractable currentInteractable;
    
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }
    
    void Update()
    {
        CheckForInteractable();
        
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
    
    void CheckForInteractable()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
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
    
    void SetInteractable(IInteractable interactable)
    {
        currentInteractable = interactable;
        
        if (interactionText != null)
        {
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
    }
}