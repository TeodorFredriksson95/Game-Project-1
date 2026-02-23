using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    [SerializeField] protected UpgradeType upgradeType;
    public UpgradeType UpgradeType => upgradeType;
}

public enum UpgradeType
{
	Force,
	Health,
	Speed,
	Coin,
	Weapon
}