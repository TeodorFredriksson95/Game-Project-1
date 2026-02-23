using UnityEngine;

public class EnemySpawnpoint : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawSolidDisc(transform.position, transform.up, 0.4f);
    }
#endif
}
