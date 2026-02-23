using System.Collections;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UIElements;

public class LootIndicator : MonoBehaviour
{
    [SerializeField] float animSpeed = 0.67f;
    [Header("Standard Mode")]
    [SerializeField] GameObject standard1;
    [SerializeField] GameObject standard2;

        [Header("OneHand Mode")]
    [SerializeField] GameObject oneHanded1;
    [SerializeField] GameObject oneHanded2;
    private GameObject button1;
    private GameObject button2;
    WaitForSeconds wait;

    SpriteRenderer[] spriteRenderer = { };

    Chest chest;

    Camera mainCamera;
    Coroutine animateCoroutine;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("OneHandedSetting") == 0)
        {
            DestroyImmediate(oneHanded1);
            DestroyImmediate(oneHanded2);
            button1 = standard1;
            button2 = standard2;
        }
        else
        {
            DestroyImmediate(standard1);
            DestroyImmediate(standard2);
            button1 = oneHanded1;
            button2 = oneHanded2;
        }


    }

    void OnEnable()
    {
        
            spriteRenderer = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in spriteRenderer)
            sprite.enabled = false;

        chest = GetComponentInParent<Chest>();

        wait = new WaitForSeconds(animSpeed);
        mainCamera = Camera.main;
    }

    public void ShowIndicator(bool show)
    {
        if (spriteRenderer != null)
        {
            foreach (SpriteRenderer sprite in spriteRenderer)
                sprite.enabled = show;
        }

        if (show)
        {
            if (animateCoroutine == null)
                animateCoroutine = StartCoroutine(AnimateLoop());
        }
        else
        {
            if (animateCoroutine != null)
            {
                StopCoroutine(animateCoroutine);
                animateCoroutine = null;
            }

            if (button1 != null) button1.SetActive(false);
            if (button2 != null) button2.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (mainCamera != null && chest != null)
            transform.forward = mainCamera.transform.forward;

        if (chest != null && chest.IsOpened && chest.obj == null)
        {
            if (spriteRenderer != null)
            {
                foreach (SpriteRenderer sprite in spriteRenderer)
                    sprite.enabled = false;
            }

            if (animateCoroutine != null)
            {
                StopCoroutine(animateCoroutine);
                animateCoroutine = null;
            }

            if (button1 != null) button1.SetActive(false);
            if (button2 != null) button2.SetActive(false);
        }
    }

    IEnumerator AnimateLoop()
    {
        if (button1 != null) button1.SetActive(true);
        if (button2 != null) button2.SetActive(false);

        while (true)
        {
            yield return wait;

            if (button1 == null || button2 == null) yield break;

            bool b1 = button1.activeSelf;
            button1.SetActive(!b1);
            button2.SetActive(b1);
        }
    }
}
