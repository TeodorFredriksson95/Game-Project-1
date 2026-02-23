using UnityEngine;

[CreateAssetMenu(fileName = "HealthUpgrade", menuName = "Upgrades/Health")]
public class HealthUpgrade : Upgrade
{
    public float maxHealth;
    public bool updateCurrentHealth;
}
