using MelenitasDev.SoundsGood;
using System.Collections;
using UnityEngine;

public class RelicMusic : MonoBehaviour
{
    [SerializeField] GameObject drums;

    public void StartDrums()
    {
        drums.SetActive(true);
    }
}
