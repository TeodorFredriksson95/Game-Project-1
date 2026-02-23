using System;
using System.Linq;
using MelenitasDev.SoundsGood;
using UnityEngine;
using UnityEngine.Rendering;

public class Relic : MonoBehaviour, IInteractable
{
    [SerializeField] VolumeProfile afterVolume;

    public static event Action OnRelicPickedUp;

    Sound pickupSound;

    public void Start()
    {
        Debug.Assert(afterVolume, "No volume profile found. Please put PP_After Volume Profile directly from objects Prefab");
        pickupSound = new Sound(SFX.AmbientShriek);
        pickupSound.SetOutput(Output.SFX);
        pickupSound.SetSpatialSound();
        pickupSound.SetVolume(0.5f);
        pickupSound.SetPosition(transform.position);
    }

    bool pickedUp;

    public void Interact()
    {
        if (pickedUp) return;
        pickedUp = true;

        SafeDisableVisualsAndColliders();
        enabled = false;

        pickupSound.SetPosition(transform.position);
        pickupSound.Play();
        StartCoroutine(DelayedCleanup());
        try
        {
            Debug.Log($"{name} was picked up!");

            var music = FindAnyObjectByType<RelicMusic>();
            if (music) music.StartDrums();

            var pauseMenu = FindAnyObjectByType<Pause_Menu>();
            if (pauseMenu) pauseMenu.StartCountDown();

            var player = FindFirstObjectByType<PlayerController>();
            if (player)
            {
                var playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth) playerHealth.IncreaseMaxHealth(10, true);

                player.BaseMoveSpeed *= 1.5f;

                if (player.Weapon)
                {
                    player.Weapon.AttackCooldown *= 0.75f;
                    player.Weapon.KickCooldown *= 0.75f;
                }

                player.HasRelic = true;
            }

            var globalVolume = FindFirstObjectByType<Volume>();
            if (globalVolume && afterVolume) globalVolume.profile = afterVolume;

            var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (var e in enemies)
                if (e.Type == Enemy.EnemyType.Banshee) e.RankUp();

            OnRelicPickedUp?.Invoke();
        }
        finally
        {
            Destroy(gameObject);
        }
    }

    System.Collections.IEnumerator DelayedCleanup()
    {
        yield return new WaitForSeconds(0.1f); 
        if (this && gameObject)
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            Destroy(gameObject);
        }
    }

    void SafeDisableVisualsAndColliders()
    {
        var ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        if (ignoreRaycast >= 0) gameObject.layer = ignoreRaycast;

        foreach (var c in GetComponentsInChildren<Collider>(true)) c.enabled = false;
        foreach (var r in GetComponentsInChildren<Renderer>(true)) r.enabled = false;
    }
}
