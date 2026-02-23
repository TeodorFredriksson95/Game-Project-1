#region
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#endregion

public class PlayerHealth : MonoBehaviour
{
    public static event Action<float, float> OnHealthChanged; // review: these aren't events yet. Right now its just a delegate, i.e., a function pointer. Make it public static event Action ... to make it an event.
    public static event Action<float, float, bool> OnMaxHealthIncreased;
    public static event Action OnPlayerDied;

    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth = 100;
    [SerializeField] RoomRespawnController respawner;
    [SerializeField] PlayerRespawnable player;


    private Pause_Menu pauseMenu;
    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
    public float CurrentHealth => currentHealth;

    public bool IsDead => currentHealth <= 0;
    Transform checkpoint;

    float previousHealth;

    private void Awake()
    {
        maxHealth = 10;
    }

    private void OnEnable()
    {
        pauseMenu = FindFirstObjectByType<Pause_Menu>();
        RoomRespawnController.OnNewCheckpoint += BackToCheckpoint;
    }

    private void OnDisable()
    {
        RoomRespawnController.OnNewCheckpoint -= BackToCheckpoint;
    }

    void BackToCheckpoint(Transform backheckpoint)
    {
        checkpoint = backheckpoint;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) TakeDamage(float.MaxValue);
    }

    public void IncreaseMaxHealth(float amount, bool refreshCurrentHealth)
    {
        maxHealth += amount;

        if (refreshCurrentHealth) currentHealth = maxHealth;

        OnMaxHealthIncreased?.Invoke(maxHealth, amount, refreshCurrentHealth);

        Debug.Log("Increased player health from: " + (maxHealth - amount) + " to: " + maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth != 0)
        {
            previousHealth = currentHealth;
            float newHealth = currentHealth - damage;
            currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
            /*Logger.Log($"Player took: {damage} damage. " + "\n" +
                       $"Current health is: {currentHealth}", this, "Player");
                       */
            OnHealthChanged?.Invoke(currentHealth, previousHealth);
        }

        if (IsDead)
        {
            OnPlayerDied?.Invoke();
            if (maxHealth > 2)
            {
                maxHealth -= 2;
                Debug.Log("Player died. Remaining lives: " + maxHealth / 2);
                respawner.RespawnPlayer(player, checkpoint);
            }
            else
            {
                Debug.Log("Player died. No remaining lives. Game Over.");
              //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                pauseMenu.LoseGame();
                //TODO: Show game over screen, rn this just restarts the level
            }
            currentHealth = maxHealth;
            OnMaxHealthIncreased?.Invoke(maxHealth, maxHealth, true);
            //IncreaseHealth(currentHealth);
        }
    }

    public void IncreaseHealth(float amount)
    {
        previousHealth = currentHealth;
        float newHealth = currentHealth + amount;
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, previousHealth);

        Logger.Log("Player was healed for: " + amount + ". Current health is: " + currentHealth, this, "Player");
    }
}
