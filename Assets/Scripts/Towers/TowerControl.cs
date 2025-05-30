using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks;
    [SerializeField]
    float attackRadius;
    [SerializeField]
    Projectile projectile;
    Enemy targetEnemy = null;
    float attackCounter;
    bool isAttacking = false;
    void Start()
    {
        // attackCounter = timeBetweenAttacks;

        
    }

void Update()
{
    attackCounter -= Time.deltaTime;

    if (targetEnemy == null || targetEnemy.IsDead)
    {
        targetEnemy = GetNearEnemy();
    }
    else
    {
        if (attackCounter <= 0)
        {
            Attack(); // Вызываем сразу, без флага
            attackCounter = timeBetweenAttacks;
        }
        
        if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
        {
            targetEnemy = null;
        }
    }
}

    // Update is called once per frame
    // void Update()
    // {
    //     attackCounter -= Time.deltaTime * Time.timeScale;
        
    //     if (targetEnemy == null || targetEnemy.IsDead)
    //     {
    //         Enemy nearestEnemy = GetNearEnemy();
    //         // if (nearestEnemy != null)
    //         // {
    //         //     Debug.Log("Nearest enemy found: " + nearestEnemy.name);
    //         // }
    //         // else
    //         // {
    //         //     Debug.Log("No enemies in range");
    //         // }

    //         if (nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius)
    //         {
    //             targetEnemy = nearestEnemy;
    //         }
    //     }
    //     else
    //     {
    //         if (attackCounter <= 0)
    //         {
    //             isAttacking = true;

    //             attackCounter = timeBetweenAttacks;
    //         } 
    //         else
    //         {
    //             isAttacking = false;

    //         }
    //         if(Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
    //         {
    //             targetEnemy = null;
    //         }
    //     }
        
        
    // }

    // public void FixedUpdate()
    // {
    //     if (isAttacking == true) 
    //     {
    //         Attack();
    //     }
    // }
    
    public void Attack()
    {
        if (projectile == null)
        {
            // Debug.LogError("Projectile prefab is not assigned!");
            return;
        }

        // Debug.Log("Attack triggered");

        isAttacking = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;
        if (targetEnemy == null)
        {
            // Debug.Log("No target enemy, destroying projectile");

            Destroy(newProjectile);
        }
        else
        {
            //move projectile
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }

    IEnumerator MoveProjectile(Projectile projectile)
{
    float maxLifetime = 3f; // Ограничение на существование снаряда
    float timeElapsed = 0f;

    while (GetTargetDistance(targetEnemy) > 0.10f && projectile != null && targetEnemy != null)
    {
        if (timeElapsed > maxLifetime) break; // Прерываем корутину, если прошло больше 3 секунд
        timeElapsed += Time.deltaTime;

        var dir = targetEnemy.transform.localPosition - transform.localPosition;
        var angleDirect = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angleDirect, Vector3.forward);
        projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
        
        yield return null;
    }

    if (projectile != null)
    {
        Destroy(projectile);
    }
}
 

    private float GetTargetDistance(Enemy thisEnemy)
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetNearEnemy();
            if (thisEnemy == null)
            {
                return 0f;
            }
        }

        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach (Enemy enemy in Manager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition)<= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }

        }
        // Debug.Log($"Enemies in range: {enemiesInRange.Count}");

        return enemiesInRange; 
    }

    private Enemy GetNearEnemy()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach (Enemy enemy in GetEnemiesInRange() )
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition)<= smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy= enemy;

            }

        }
        return nearestEnemy;

    }
}
