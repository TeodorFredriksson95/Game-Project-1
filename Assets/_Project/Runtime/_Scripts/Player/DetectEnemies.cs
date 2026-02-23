using System.Collections.Generic;
using UnityEngine;

public class DetectEnemies : MonoBehaviour
{
    readonly HashSet<GameObject> enemiesInRange = new();
    public readonly List<(GameObject enemy, float angle)> enemyAngles = new();

    void Update()
    {
        // get rid of nulls
        enemiesInRange.RemoveWhere(go => !go);

        // remake angles
        enemyAngles.Clear();
        foreach (var go in enemiesInRange)
        {
            if (!go) continue;
            enemyAngles.Add((go, GetAngle(go)));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemiesInRange.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemiesInRange.Remove(other.gameObject);
    }

    float GetAngle(GameObject target)
    {
        if (!target) return 0f;
        Vector3 to = target.transform.position - transform.position;
        float targetYaw = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;
        float playerYaw = transform.eulerAngles.y;
        return Mathf.DeltaAngle(playerYaw, targetYaw);
    }
}
