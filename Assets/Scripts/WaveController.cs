using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public Transform spawnStart;
    public List<Path> ListPath;
    public GameObject Enemy;
    public float SpawnRate = 2.0f;
    public List<BotController> ListBots = new List<BotController>();
    float Duration = 0.0f;

    int MaxEnemy = 12;
    public int currentEnemySpawn = 0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemySpawn >= MaxEnemy) return;

        if (Duration > 1.0f / SpawnRate)
        {
            CreateEnemy();
            Duration = 0.0f;
        }
        Duration += Time.deltaTime;

    }
    
    public void ChangePath(int indexEnemy, int indexPath)
    {
        if (indexPath >= ListPath.Count - 1) return;
        if (ListPath[indexPath + 1].AllFinish)
        {
            bool nextPath = true;
            for (int i = 0; i < ListBots.Count; i++)
            {
                if (ListBots[i].IsAlive() && ListBots[i].finishPaths[indexPath] == false)
                {
                    nextPath = false;
                    break;
                }
            }

            if (nextPath)
            {
                for (int i = 0; i < ListBots.Count; i++)
                {
                    if (ListBots[i].IsAlive())
                    {
                        ListBots[i].MovingPath = ListPath[indexPath + 1];
                        ListBots[i].IndexPath = indexPath + 1;
                    }
                }
            }
        }
        else
        {
            if (ListBots[indexEnemy].IsAlive())
            {
                ListBots[indexEnemy].MovingPath = ListPath[indexPath + 1];
                ListBots[indexEnemy].IndexPath = indexPath + 1;
            }
        }
    }

    void CreateEnemy()
    {
        BotController EnemyClone = ObjectPoolManager.Spawn(Enemy, spawnStart.position, Quaternion.identity).GetComponent<BotController>();
        EnemyClone.ActiveObj(ListPath[0], currentEnemySpawn, this);
        ListBots.Add(EnemyClone);

        currentEnemySpawn++;
    }
}
