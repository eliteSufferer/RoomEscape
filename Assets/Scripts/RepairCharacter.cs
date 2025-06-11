using UnityEngine;

public class RepairCharacter : MonoBehaviour, IInteractable
{
    [Header("Movement Settings")]
    public Transform targetPoint;
    public float moveSpeed = 3f;
    
    [Header("Repair Settings")]
    public Transform repairTarget; // Объект к которому поворачиваться (телевизор)
    public Television targetTV; // Телевизор который чинить
    public float repairDuration = 2f; // Длительность анимации починки
    public float rotationSpeed = 5f; // Скорость поворота
    
    private bool isMoving = false;
    private bool isTurning = false;
    private bool isRepairing = false;
    private bool hasFinished = false;
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Interact()
    {
        // Проверяем что телевизор еще не починен
        if (targetTV != null && targetTV.isFixed)
        {
            Debug.Log("Телевизор уже починен!");
            return;
        }
        
        if (!isMoving && !isTurning && !isRepairing && !hasFinished && targetPoint != null)
        {
            StartMoving();
        }
    }
    
    public string GetInteractionText()
    {
        if (targetTV != null && targetTV.isFixed)
            return "Телевизор уже починен";
        else if (hasFinished)
            return "Уже починил телевизор";
        else if (isRepairing)
            return "Чинит телевизор...";
        else if (isTurning)
            return "Поворачивается к телевизору...";
        else if (isMoving)
            return "Идет к телевизору...";
        else
            return "Попросить починить телевизор";
    }
    
    void StartMoving()
    {
        isMoving = true;
        
        // Включаем анимацию ходьбы
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }
        
        Debug.Log("Начинаю движение к телевизору");
    }
    
    void Update()
    {
        if (isMoving && targetPoint != null)
        {
            // Движение к цели
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            
            // Поворот к цели во время движения
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
            
            // Проверяем дошли ли
            float distance = Vector3.Distance(transform.position, targetPoint.position);
            if (distance < 0.1f)
            {
                StopMovingAndStartTurning();
            }
        }
        else if (isTurning && repairTarget != null)
        {
            // Поворот к объекту починки
            Vector3 directionToTarget = (repairTarget.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            
            // Плавно поворачиваемся
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            // Проверяем повернулись ли достаточно
            float angle = Quaternion.Angle(transform.rotation, targetRotation);
            if (angle < 5f) // 5 градусов точности
            {
                StartRepair();
            }
        }
    }
    
    void StopMovingAndStartTurning()
    {
        // Останавливаем движение, начинаем поворот
        isMoving = false;
        isTurning = true;
        
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }
        
        Debug.Log("Дошел до точки, поворачиваюсь к телевизору!");
    }
    
    void StartRepair()
    {
        // Останавливаем поворот, начинаем починку
        isTurning = false;
        isRepairing = true;
        
        if (animator != null)
        {
            animator.SetBool("isRepairing", true);
        }
        
        Debug.Log("Повернулся к телевизору, начинаю чинить!");
        
        // Через repairDuration секунд заканчиваем ремонт
        Invoke("FinishRepair", repairDuration);
    }
    
    void FinishRepair()
    {
        isRepairing = false;
        hasFinished = true;
        
        if (animator != null)
        {
            // Выключаем починку, возвращаемся в idle
            animator.SetBool("isRepairing", false);
        }
        
        // 🎉 ЧИНИМ ТЕЛЕВИЗОР! 🎉
        if (targetTV != null)
        {
            targetTV.FixTV();
            Debug.Log("Телевизор починен! Код появился на экране!");
        }
        else
        {
            Debug.LogError("Target TV не назначен!");
        }
        
        Debug.Log("Починка завершена!");
    }
}