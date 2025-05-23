using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Подключаем для сортировки

public class Enemy : MonoBehaviour
{
    [SerializeField]
     float speed;
    [SerializeField]
     int nextPoint = 0;
    [SerializeField]
    int health;
    [SerializeField]
    int amountReward;
    Collider2D enemycollider;
    public float reachThreshold = 0.1f; // Допустимая погрешность для достижения точки
    GameObject exit;
    bool isDead = false;
    // Массив точек движения
    GameObject[] mPoint;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    void Start()
    {   
        enemycollider = GetComponent<Collider2D>();
        // Находим все объекты с тегом MoveingPoint
        mPoint = GameObject.FindGameObjectsWithTag("MoveingPoint");

        // Сортируем массив по координате X (можно изменить на Y, если нужно)
        mPoint = mPoint.OrderBy(point => point.name).ToArray();

        exit = GameObject.FindWithTag("Finish");
        Manager.Instance.RegEnemy(this);
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile")) // Проверяем, что это снаряд
        {
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            Enemyhit(newP.AttackDamage);
            
            Destroy(collision.gameObject); // Уничтожаем снаряд
        }
    }



     void Update()
    {
        // Проверка, что объект ещё движется по точкам
        if (nextPoint < mPoint.Length && isDead == false)
        {
            MoveTowards(mPoint[nextPoint].transform.position);

            if (Vector2.Distance(transform.position, mPoint[nextPoint].transform.position) < reachThreshold )
            {
                nextPoint += 1; 
            }
        }
        // Движение к точке выхода (после последней точки)
        else if (nextPoint == mPoint.Length && isDead == false  )
        {
            MoveTowards(exit.transform.position);
            
            if (Vector2.Distance(transform.position, exit.transform.position) < reachThreshold)
            {
                Manager.Instance.RndEscaped += 1;
                Manager.Instance.TotalEscaped += 1;
                Manager.Instance.IsWaveOver();
                Destroy(gameObject); // Уничтожаем объект при достижении выхода
                Manager.Instance.UnRegEnemy(this);
            }
        }
    }


    // Метод для движения к заданной позиции
    void MoveTowards(Vector3 targetPosition)
    {
        // Перемещаем объект к цели со скоростью speed
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void Enemyhit(int hitPoints)
    {
        if (health - hitPoints  > 0)
        {
            health -= hitPoints;
        }
        else 
        {
            Die();
        }

    }
public void Die()
{
    isDead = true;
    enemycollider.enabled = false;
    StartCoroutine(DestroyAfterDelay(0.2f)); // Запускаем корутину с задержкой в 1 секунду
    Manager.Instance.TotalKilld+=1;
    Manager.Instance.getMoney(amountReward);
    Manager.Instance.IsWaveOver();

}

IEnumerator DestroyAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay); // Ожидание указанного времени
    Destroy(gameObject); // Удаление объекта
    Manager.Instance.UnRegEnemy(this);
}


}
