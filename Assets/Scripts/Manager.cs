using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Loader<Manager>
{
    [SerializeField]
     GameObject spawnPoint;
     
     [SerializeField]
     GameObject[] enemies;

     [SerializeField]
     int maxEnemiesOnScreen;

     [SerializeField]
     int totalEnemies;
     
     [SerializeField]
     int enemiesPerSpawn;
    int enemiesOnScreen = 0; 
    const float spawnDelay = 0.5f;
    

    void Awake()
    {
    

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
    
        if (enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {

                if (enemiesOnScreen < maxEnemiesOnScreen)
                {
                    GameObject newEnemy = Instantiate(enemies[0]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                    enemiesOnScreen += 1;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }
    
    public void removeEnemyFromScreen()
    {
        if (enemiesOnScreen > 0) 
        {
            enemiesOnScreen -=1 ; 
        }
    }
}
