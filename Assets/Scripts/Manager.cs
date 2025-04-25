using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next, win, play, gameover
}

public class Manager : Loader<Manager>
{
    [SerializeField]
    Text currentWave;
    [SerializeField]
    Text btnText;
    [SerializeField]
    Text escapedTxt;
    [SerializeField]
    Button btn;
    [SerializeField]
    int totalWaves = 10;
    [SerializeField]
    Text totalMoneyText;
    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    Enemy[] enemies;
    [SerializeField]
    int maxEnemiesOnScreen;
    [SerializeField]
    int totalEnemies;
    [SerializeField]
    int enemiesPerSpawn;
    int waveNum = 0;
    int totalEscaped =0;
    int rndEscaped =0; 
    int totalKilld =0;
    int totalMoney = 10;
    int enemiesToSpawn=0;
    const float spawnDelay = 0.5f;
    public List<Enemy> EnemyList = new List<Enemy>();
    gameStatus currentstate = gameStatus.play;
    int enemyNeedtoSpawn = 0;
    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value; 
        }
    }   

    public int RndEscaped
    {
        get
        {
            return rndEscaped;
        }
        set
        {
            rndEscaped = value; 
        }
    }   

    public int TotalKilld
    {
        get
        {
            return totalKilld;
        }

        set
        {
            totalKilld = value; 
        }
    }   

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set 
        { 
            totalMoney=value; 
            totalMoneyText.text = TotalMoney.ToString();
        }

    }   



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        btn.gameObject.SetActive(false);
        ShowMenu();
    }
 
    // Update is called once per frame
    private void Update()
    {
        HandleEscape();
    }

    IEnumerator Spawn()
    {
    
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {

                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0,enemiesToSpawn)]) as Enemy;
                    // Enemy newEnemy = Instantiate(enemies[3]) as Enemy;

                    newEnemy.transform.position = spawnPoint.transform.position;
                    // enemiesOnScreen += 1;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }
    
    public void RegEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnRegEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyEnemies()
    {
        foreach (Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void getMoney(int amount )
    {
        TotalMoney += amount;
    }

    public void minusMoney(int amount)
    {
        TotalMoney-=amount;
    }

    public void IsWaveOver()
    {
        escapedTxt.text = "Escaped: " + TotalEscaped;
        
        if ( (RndEscaped + TotalKilld) == totalEnemies )
        {
            if (waveNum <= enemies.Length)
            {
                enemiesToSpawn = waveNum;
            }
            SetGameStatus();
            ShowMenu();
        }
    }

    public void SetGameStatus()
    {
        if (totalEscaped>=10)
        {
            currentstate = gameStatus.gameover;
        }
        else if (waveNum == 0 && (RndEscaped + TotalKilld) == 0)
        {
            currentstate= gameStatus.play;
        }
        else if (waveNum >= totalWaves)
        {
            currentstate = gameStatus.win;
        }

        else 
        {
            currentstate = gameStatus.next;
        }
    }

    public void PlayBtnPressed()
    {
        switch(currentstate)
        {
            case gameStatus.next:
                waveNum+=1;
                totalEnemies +=waveNum;
                break;

            default:
                enemiesToSpawn=0;
                waveNum = 0;
                totalEnemies = 5;
                TotalEscaped = 0;
                TotalMoney = 20;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagBuildSite();
                totalMoneyText.text=TotalMoney.ToString();
                escapedTxt.text="Escaped: " + TotalEscaped;
                break;
        }
        DestroyEnemies();
        TotalKilld=0;
        RndEscaped=0;
        currentWave.text = "W A V E    "  +(waveNum +1);
        StartCoroutine(Spawn());
        btn.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        switch (currentstate)
        {
            case gameStatus.gameover:
                btnText.text = "TRY AGAIN";
                break;
            case gameStatus.next:
                btnText.text = "NEXT WAVE";
                break;
            case gameStatus.play:
                btnText.text = "PLAY";
                break;
            case gameStatus.win:
                btnText.text = "PLAY";
                break;

        }
        btn.gameObject.SetActive(true);
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDrug();
            TowerManager.Instance.towerBtnPressed=null;
            
        }

    }

}
