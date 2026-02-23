using Codice.Client.Common.FsNodeReaders;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreateEnemySpawnPoint))]
public class CreateEnemySpawnPointButtonEditor : Editor
{
    static int counter = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateEnemySpawnPoint script = (CreateEnemySpawnPoint)target;

        if (GUILayout.Button("Add Enemy Spawnpoint"))
        {
            GameObject newSpawnpoint = new GameObject("New Spawnpoint " + counter);
            Undo.RegisterCreatedObjectUndo(newSpawnpoint, "Create Spawnpoint");

            newSpawnpoint.transform.SetParent(script.transform);
            newSpawnpoint.transform.localPosition = Vector3.zero;
            newSpawnpoint.AddComponent<EnemySpawnpoint>();
            counter++;
        }

    }
}