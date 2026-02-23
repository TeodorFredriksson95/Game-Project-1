using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeLootable : MonoBehaviour, IInteractable
{
    [SerializeField] Upgrade data;

    public void Interact()
    {
        Logger.Log($"{name} was picked up!", this, "Upgrade");

        UpgradesHUD upgradesHUD = FindFirstObjectByType<UpgradesHUD>();
        var player = FindAnyObjectByType<PlayerController>();
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        player.ApplyUpgrade(data);


        // Upgrading the *amount* of upgrades picked up. If we want the upgrades to reflect the actual stat values
        // we can refactor the switch statement to check to check for stat types instead,
        // and then pass the Upgrade 'data' object as parameter instead.

        //Limited time. Coin HUD is updated inside Coin script.
        switch (data.UpgradeType)
        {
            case UpgradeType.Force:
                upgradesHUD.UpgradeForceValue(1);
                break;
            case UpgradeType.Speed:
                upgradesHUD.UpgradeSpeedValue(1);
                break;
            case UpgradeType.Health:
                var healthIncrease = data as HealthUpgrade;
                playerHealth.IncreaseMaxHealth(healthIncrease.maxHealth, healthIncrease.updateCurrentHealth);
                playerHealth.IncreaseHealth(healthIncrease.maxHealth); // variable name is confusing here, but we're healing for the same amount 
                                                                      // that we upgrade our max health. Upgrade max health by 2, heal 2.
                break;
            default:
                Debug.Log("The upgrade type has not been registered in the UpgradeType enum.");
                break;
        }

        Destroy(gameObject);
    }
}
