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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackCounter-= Time.deltaTime;
        
        if (targetEnemy == null)
        {
            Enemy nearEnemy = GetNearEnemy();
            if (GetNearEnemy() != null && Vector2.Distance(transform.localPosition, GetNearEnemy().transform.localPosition) <= attackRadius)
            {
                targetEnemy = nearEnemy;
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

    public void FixUpdate()
    {
        if (isattack == true) 
        {
            Attack();
        }
    }
    
    public void Attack()
    {
        isattack = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;
        if (targetEnemy == null)
        {
            Destroy(projectile);
        }
        else
        {
            //move projectile
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }

    IEnumerator MoveProjectile(Projectile projectile)
    {
        while(GetTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null )
        {
            var dir = targetEnemy.transform.position - transform.position;
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
