using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UpgradesHUD : MonoBehaviour
{
    public static UpgradesHUD Instance;

    // Labels
    Label ForceUpgradeLabel;
    Label SpeedUpgradeLabel;

    int healthUpgradeInt;
    int speedUpgradeInt;
    int forceUpgradeInt;

    void Awake()
    {
        Instance = this;

        var doc = GetComponent<UIDocument>();
        
        //Labels
        ForceUpgradeLabel = doc.rootVisualElement.Q<Label>("ForceUpgradeNr");
        SpeedUpgradeLabel = doc.rootVisualElement.Q<Label>("SpeedUpgradeNr");

        if (Int32.TryParse(SpeedUpgradeLabel.text, out int speedVal))
        {
            speedUpgradeInt = speedVal;
        }
        if (Int32.TryParse(ForceUpgradeLabel.text, out int forceVal))
        {
            forceUpgradeInt = forceVal;
        }

    }


    public void UpgradeSpeedValue(int val)
    {
        speedUpgradeInt += val;
        SpeedUpgradeLabel.text = speedUpgradeInt.ToString();
    }

    public void UpgradeForceValue(int val)
    {
        forceUpgradeInt += val;
        ForceUpgradeLabel.text = forceUpgradeInt.ToString();
    }
     
}
