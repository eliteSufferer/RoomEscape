using UnityEngine;
using UnityEngine.AI;

public class RepairCharacter : MonoBehaviour, IInteractable
{
    [Header("Repair Settings")]
    public Television targetTV;
    public float repairDistance = 2f;
    
    [Header("Animation")]
    public string repairAnimationName = "Punching"; // Имя анимации из Mixamo
    
    private NavMeshAgent agent;
    private Animator animator;
    private bool isRepairing = false;
    private bool hasRepaired = false;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    
    public void Interact()
    {
        if (!hasRepaired && !isRepairing)
        {
            StartRepair();
        }
    }
    
    public string GetInteractionText()
    {
        if (hasRepaired)
            return "Телевизор уже починен";
        else if (isRepairing)
            return "Чинит телевизор...";
        else
            return "Попросить починить телевизор";
    }
    
    void StartRepair()
    {
        if (targetTV == null) return;
        
        isRepairing = true;
        
        // Идем к телевизору
        Vector3 tvPosition = targetTV.transform.position;
        Vector3 targetPosition = tvPosition + (transform.position - tvPosition).normalized * repairDistance;
        
        agent.SetDestination(targetPosition);
    }
    
    void Update()
    {
        if (isRepairing && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Дошли до телевизора - начинаем чинить
            PerformRepair();
        }
    }
    
    void PerformRepair()
    {
        // Поворачиваемся к телевизору
        Vector3 lookDirection = (targetTV.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection);
        
        // Играем анимацию
        if (animator != null)
        {
            animator.Play(repairAnimationName);
        }
        
        // Через 2 секунды заканчиваем ремонт
        Invoke("CompleteRepair", 2f);
    }
    
    void CompleteRepair()
    {
        hasRepaired = true;
        isRepairing = false;
        
        // Чиним телевизор
        if (targetTV != null)
        {
            targetTV.FixTV();
        }
        
        Debug.Log("Персонаж починил телевизор!");
    }
}