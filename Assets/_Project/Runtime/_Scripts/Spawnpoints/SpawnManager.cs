using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Transform enemyPrefab;
    [SerializeField] SpawnTrigger spawnTrigger;

    [Header("SpawnButton")]
    [SerializeField] bool isTriggeredByButton = false;

    List<Transform> enemies = new List<Transform>();

    private void OnEnable()
    {
        if (isTriggeredByButton)
            RotateBridge.OnEnemyButtonPressed += SpawnEnemies;
        else
            spawnTrigger.OnTriggerSpawn += SpawnEnemies;

    }
    private void OnDisable()
    {
        if (isTriggeredByButton)
            RotateBridge.OnEnemyButtonPressed -= SpawnEnemies;
        else
            spawnTrigger.OnTriggerSpawn -= SpawnEnemies;

    }

    private void Awake()
    {

        foreach (Transform transform in transform)
        {
            enemies.Add(transform);
        }
    }

    void SpawnEnemies()
    {
        var player = FindFirstObjectByType<PlayerController>().transform;
        var room = FindFirstObjectByType<RoomRegistry>();

        for (int i = 0; i < enemies.Count - 1; i++) // Count -1 becuase the spawntrigger is also a child of the Spawnmanager.
                                                    // If we dont do -1 then we will spawn an enemy inside the trigger box aswell
        {
            Vector3 spawnPos = enemies[i].position;

            // spawn
            var enemyObj = Instantiate(enemyPrefab, enemies[i].position, enemies[i].rotation);
            NavMeshAgent agent = enemyObj.GetComponent<NavMeshAgent>();

            if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
            else
            {
                Debug.LogError("Enemy failed to spawn on NavMesh at: " + spawnPos);
            }

            // parent it under room BEFORE Awake finishes
            if (room != null)
                enemyObj.SetParent(room.transform);

            var enemyComp = enemyObj.GetComponent<Enemy>();
            //Debug.Log(enemyComp.Mesh);

            // assign target + room
            enemyComp.Target = player;
            enemyComp.Room = room;

            // register
            room?.Register(enemyComp);

        }
    }


}