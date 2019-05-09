using UnityEngine;
using System.Collections;

public class SpawnGameObject : MonoBehaviour {
    public float secondsBetweenSpawning;
    public GameObject[] spawnObjects;
    public float radius;

    bool isSpawning;
    // Use this for initialization
    void Start()
    {
        isSpawning = false;
    }
    private void Update()
    {
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
        if (GameManager.gm.gameState != GameManager.gameStates.Play)
            isSpawning = false;
        else
        {
            int index = Random.Range(0, spawnObjects.Length);
            if (spawnObjects[index] != null)
            {
                float theta = Random.Range(-180, 180);
                theta = Mathf.Deg2Rad * theta;
                Vector3 spawnPosition = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0f);
                GameObject spawn = Instantiate(spawnObjects[index], spawnPosition, Quaternion.identity) as GameObject;
                spawn.transform.parent = transform;
            }
            Invoke("Spawn", secondsBetweenSpawning);
        }
    }
}
