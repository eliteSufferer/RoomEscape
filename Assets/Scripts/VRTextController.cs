using UnityEngine;

public class VRTextController : MonoBehaviour
{
    [Header("VR Text Settings")]
    public Canvas vrTextCanvas; // Drag your World Space Canvas here
    
    [Header("Auto Setup")]
    public bool findCanvasAutomatically = true; // Automatically find child Canvas
    
    private bool isVRMode = true;
    
    void Start()
    {
        // Auto-detect VR mode
        // isVRMode = FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.ActionBasedController>() != null;

        
        // Auto-find Canvas if not assigned
        if (findCanvasAutomatically && vrTextCanvas == null)
        {
            vrTextCanvas = GetComponentInChildren<Canvas>();
        }
        
        // Initially hide text
        if (vrTextCanvas != null)
        {
            vrTextCanvas.gameObject.SetActive(false);
        }
        
        // If not VR mode, disable this component
        if (!isVRMode)
        {
            this.enabled = false;
        }
    }
    
    // Called when player looks at this object
    public void OnLookAt()
    {
        if (vrTextCanvas != null && isVRMode)
        {
            vrTextCanvas.gameObject.SetActive(true);
        }
    }
    
    // Called when player looks away from this object
    public void OnLookAway()
    {
        if (vrTextCanvas != null && isVRMode)
        {
            vrTextCanvas.gameObject.SetActive(false);
        }
    }
    
    // Alternative method names for compatibility
    public void ShowText()
    {
        OnLookAt();
    }
    
    public void HideText()
    {
        OnLookAway();
    }
}