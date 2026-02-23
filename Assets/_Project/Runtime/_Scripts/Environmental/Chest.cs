#region
using System;
using MelenitasDev.SoundsGood;
using UnityEngine;
using VInspector;
#endregion

[SelectionBase]
public class Chest : MonoBehaviour, IInteractable
{
    private enum Reward { Upgrade, Weapon, Relic, Other }

    [Tab("Reward")]
    [SerializeField] private Reward reward = Reward.Other;
    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private float offset = 0.75f;
    [SerializeField] private float scaleFactor = 1.5f;
    [EndIf]

    [Tab("Pop Piece")]
    [SerializeField, Tooltip("Path relative to this object to the mesh to delete on open.")]
    private string lidPath = "chest/Cube.001";

    [SerializeField, Tooltip("Prefab that will be spawned to fly/tumble (collisionless).")]
    private GameObject popPrefab;

    [SerializeField, Tooltip("Upward impulse for the pop piece.")]
    private float popUpForce = 4.5f;

    [SerializeField, Tooltip("Random torque strength for tumble.")]
    private float popTorque = 15f;

    [SerializeField, Tooltip("Seconds before the pop piece cleans itself up.")]
    private float popLifetime = 2.0f;

    LootIndicator lootIndicator;
    Sound tonalHint;

    [HideInInspector] public GameObject obj;

    public bool IsOpened { get; private set; }
    public InteractableType Type => InteractableType.Chest;

    public event Action OnRelicChestOpened;

    // Grab *all* ParticleSystems in this chest hierarchy (even inactive)
    ParticleSystem[] particleSystems;

    void Awake()
    {
        // Cache early in case something else plays them in Start()
        particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
    }

    void Start()
    {
        name = $"Chest ({reward})";

        lootIndicator = GetComponentInChildren<LootIndicator>();

        tonalHint = new Sound(SFX.TonalHint);
        tonalHint.SetOutput(Output.SFX);
        tonalHint.SetSpatialSound();
        tonalHint.SetVolume(0.3f);
        tonalHint.SetPosition(transform.position);
    }

    public void Interact() => Open();

    public void Open()
    {
        if (IsOpened) return;

        // 1) POP THE LID (delete original piece, spawn collisionless flyer)
        TryPopLid();

        // 2) Spawn the reward as before
        if (rewardPrefab != null)
        {
            obj = Instantiate(rewardPrefab, transform.position + Vector3.up * offset, transform.rotation);
            obj.transform.localScale *= scaleFactor;
        }

        tonalHint.Play();

        // 3) Stop & clear particles so they don't linger
        if (particleSystems != null)
        {
            foreach (var ps in particleSystems)
            {
                if (ps == null) continue;
                ps.Stop(withChildren: true, stopBehavior: ParticleSystemStopBehavior.StopEmittingAndClear);
                Destroy(ps.gameObject);
            }
        }

        gameObject.layer = 0; // non-interactable/default layer or whatever you use post-open
        IsOpened = true;

        if (reward == Reward.Relic) OnRelicChestOpened?.Invoke();
    }

    void TryPopLid()
    {
        if (string.IsNullOrWhiteSpace(lidPath) || popPrefab == null) return;

        // Find the original lid piece by path under this chest
        var lidTransform = transform.Find(lidPath);
        if (lidTransform == null)
        {
            // Fallback: try to find by name anywhere under this object
            lidTransform = FindDeepChild(transform, "Cube.001");
            if (lidTransform == null) return; // Nothing to do
        }

        // Record pose before destroying the model piece
        Vector3 spawnPos = lidTransform.position;
        Quaternion spawnRot = lidTransform.rotation;
        Vector3 spawnScale = lidTransform.lossyScale;

        // Delete original visual (including any colliders so it won't block/ghost)
        Destroy(lidTransform.gameObject);

        // Spawn the collisionless pop prefab at the same pose
        var popGO = Instantiate(popPrefab, spawnPos, spawnRot);

        // Optional: match overall visual scale (only if your prefab is neutral-scaled)
        // We cannot directly assign lossyScale—so uniformly scale as an approximation:
        float uniform = (spawnScale.x + spawnScale.y + spawnScale.z) / 3f;
        popGO.transform.localScale *= uniform;

        // Ensure the spawned piece flies/tumbles + auto-cleans
        var flyer = popGO.GetComponent<PopFlyer>();
        if (flyer == null) flyer = popGO.AddComponent<PopFlyer>();

        flyer.Configure(
            upForce: popUpForce,
            torqueStrength: popTorque,
            lifetime: popLifetime,
            removeAllColliders: true,
            gravity: true
        );
    }

    // Utility: deep search by name (first match)
    static Transform FindDeepChild(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            if (child.name == name) return child;
            var found = FindDeepChild(child, name);
            if (found != null) return found;
        }
        return null;
    }
}