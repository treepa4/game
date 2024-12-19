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
    Enemy targetEnemy=null;
    float attackCounter;
    bool isattack = false;
    void Start()
    {
        attackCounter = timeBetweenAttacks;

        
    }

    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        
        if (targetEnemy == null)
        {
            Enemy nearestEnemy = GetNearEnemy();
            // if (nearestEnemy != null)
            // {
            //     Debug.Log("Nearest enemy found: " + nearestEnemy.name);
            // }
            // else
            // {
            //     Debug.Log("No enemies in range");
            // }

            if (nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius)
            {
                targetEnemy = nearestEnemy;
            }
        }

        else
        {
            if (attackCounter <= 0)
            {
                isattack = true;

                attackCounter = timeBetweenAttacks;
            } 
            else
            {
                isattack = false;

            }
            if(Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null;
            }
        }
        
        
    }

    public void FixedUpdаte()
    {
        if (isattack == true) 
        {
            Attack();
        }
    }
    
    public void Attack()
    {
        Debug.Log("Attack triggered");

        isattack = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;
        if (targetEnemy == null)
        {
            Debug.Log("No target enemy, destroying projectile");

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
        while(GetTargetDistance(targetEnemy) > 0.10f && projectile != null && targetEnemy != null )
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirect = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirect, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }
        if (projectile != null || targetEnemy != null)
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
        Debug.Log($"Enemies in range: {enemiesInRange.Count}");

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
