using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public GameObject prefabToSpawn1;
    public GameObject prefabToSpawn2;
    public GameObject prefabToSpawn3;

    [Tooltip("Tiempo de espera entre clientes")]
    public float waitBeforeSpawn = 5f;  // Time to wait before spawning

    private int completedTasks = 0;
    private int failedTasks = 0;
    private int totalClients = 0;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Step 1: Wait before spawning
            yield return new WaitForSeconds(waitBeforeSpawn);

            int idx = Random.Range(0, 3);

            GameObject spawnedObject = null;
            switch (idx)
            {
                case 0:
                    // Step 2: Spawn the first prefab
                    spawnedObject = Instantiate(prefabToSpawn1, transform.position, Quaternion.identity);
                    break;
                case 1:
                    // Step 2: Spawn the second prefab
                    spawnedObject = Instantiate(prefabToSpawn2, transform.position, Quaternion.identity);
                    break;
                case 2:
                    // Step 2: Spawn the third prefab
                    spawnedObject = Instantiate(prefabToSpawn3, transform.position, Quaternion.identity);
                    break;
            }
            totalClients++;

            // Step 3: Wait until the spawned object is destroyed
            yield return new WaitUntil(() => spawnedObject == null);
        }
    }

    public void TaskCompleted()
    {
        completedTasks++;
        Debug.Log("Task completed! Total completed tasks: " + completedTasks);
    }

    public void TaskFailed()
    {
        failedTasks++;
        Debug.Log("Task failed! Total failed tasks: " + failedTasks);
    }
}
