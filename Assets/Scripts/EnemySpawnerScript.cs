using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour
{
    public float secondsBetweenSpawning;
    public GameObject[] weakEnemiesPrefabs;
    public GameObject[] averageEnemiesPrefabs;
    public GameObject[] strongEnemiesPrefabs;
    public float radius;

    bool isSpawning;
    // Use this for initialization
    void Start()
    {
        isSpawning = false;
    }

    private void Update()
    {
        if (GameManager.gm.freezed)
            return;
        if (GameManager.gm.gameState == GameManager.gameStates.Play)
        {
            if (isSpawning == true)
                return;
            else
            {
                isSpawning = true;
                Invoke("Spawn", secondsBetweenSpawning);
            }
        }
    }
    void Spawn()
    {
        if (GameManager.gm.freezed)
            isSpawning = false;
        else if (GameManager.gm.gameState != GameManager.gameStates.Play)
            isSpawning = false;
        else
        {
            int type = Random.Range(0, 5);
            int index;
            switch (type)
            {
                case 0:
                case 1:
                    index = Random.Range(0, weakEnemiesPrefabs.Length);
                    if (weakEnemiesPrefabs[index] != null)
                    {
                        float theta = Random.Range(-180, 180);
                        theta = Mathf.Deg2Rad * theta;
                        Vector3 spawnPosition = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
                        GameObject spawn = Instantiate(weakEnemiesPrefabs[index], spawnPosition, Quaternion.identity) as GameObject;
                        spawn.transform.parent = transform;
                    }
                    Invoke("Spawn", secondsBetweenSpawning);
                    break;
                case 2:
                case 3:
                case 4:
                    index = Random.Range(0, averageEnemiesPrefabs.Length);
                    if (averageEnemiesPrefabs[index] != null)
                    {
                        float theta = Random.Range(-180, 180);
                        theta = Mathf.Deg2Rad * theta;
                        Vector3 spawnPosition = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
                        GameObject spawn = Instantiate(averageEnemiesPrefabs[index], spawnPosition, Quaternion.identity) as GameObject;
                        spawn.transform.parent = transform;
                    }
                    Invoke("Spawn", secondsBetweenSpawning);
                    break;
                case 5:
                    index = Random.Range(0, strongEnemiesPrefabs.Length);
                    if (strongEnemiesPrefabs[index] != null)
                    {
                        float theta = Random.Range(-180, 180);
                        theta = Mathf.Deg2Rad * theta;
                        Vector3 spawnPosition = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
                        GameObject spawn = Instantiate(strongEnemiesPrefabs[index], spawnPosition, Quaternion.identity) as GameObject;
                        spawn.transform.parent = transform;
                    }
                    Invoke("Spawn", secondsBetweenSpawning);
                    break;
            }
        }
    }
}

