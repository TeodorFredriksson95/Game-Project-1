using System;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public event Action OnTriggerSpawn;
    bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            OnTriggerSpawn?.Invoke();
            isTriggered = true;
        }
    }
}
