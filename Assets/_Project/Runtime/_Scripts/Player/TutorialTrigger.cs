using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] float animationSpeed;
    [SerializeField] float animationTime;
    [SerializeField] List<Sprite> standardsprites = new List<Sprite>();
    [SerializeField] List<Sprite> OneHandedsprites = new List<Sprite>();


    bool playerInside;
    float triggerTimer;

    private List<Sprite> sprites = new List<Sprite>();
    void Awake()
    {
        triggerTimer = animationTime + 1;
        if (PlayerPrefs.GetInt("OneHandedSetting") == 0)
            sprites = standardsprites;
        else
            sprites = OneHandedsprites; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        triggerTimer = animationTime;
        TutorialUI.Instance.ShowIndicator(sprites, animationSpeed, animationTime);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
    }

    void Update()
    {
        if (!playerInside) return;

        triggerTimer -= Time.deltaTime;

        if (triggerTimer <= 0)
        {
            triggerTimer = animationTime;
            TutorialUI.Instance.ShowIndicator(sprites, animationSpeed, animationTime);
        }
    }
}
