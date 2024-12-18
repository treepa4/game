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
    public float reachThreshold = 0.1f; // Допустимая погрешность для достижения точки
    GameObject exit;

    // Массив точек движения
    GameObject[] mPoint;

    void Start()
    {
        // Находим все объекты с тегом MoveingPoint
        mPoint = GameObject.FindGameObjectsWithTag("MoveingPoint");

        // Сортируем массив по координате X (можно изменить на Y, если нужно)
        mPoint = mPoint.OrderBy(point => point.name).ToArray();

        exit = GameObject.FindWithTag("Finish");
        Manager.Instance.RegEnemy(this);
    }

     void Update()
    {
        // Проверка, что объект ещё движется по точкам
        if (nextPoint < mPoint.Length)
        {
            // Движение к следующей точке
            MoveTowards(mPoint[nextPoint].transform.position);

            // Проверяем, достигли ли текущей цели
            if (Vector2.Distance(transform.position, mPoint[nextPoint].transform.position) < reachThreshold)
            {
                nextPoint += 1; // Переключаемся на следующую точку
            }
        }
        // Движение к точке выхода (после последней точки)
        else if (nextPoint == mPoint.Length)
        {
            MoveTowards(exit.transform.position);

            // Проверяем, достигли ли точки выхода
            if (Vector2.Distance(transform.position, exit.transform.position) < reachThreshold)
            {
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
}
