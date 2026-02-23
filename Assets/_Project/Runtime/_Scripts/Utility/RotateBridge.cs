using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class RotateBridge : MonoBehaviour, IInteractable
{
    public static event Action OnEnemyButtonPressed;
    public InteractableType Type => InteractableType.Default;

    [SerializeField] Transform rotateTarget;  
    [SerializeField] float degreesPerSecond = 180f;
    [SerializeField] float arriveThreshold = 0.25f;
    [SerializeField] float cooldownSeconds = 0.5f;
    [SerializeField] NavMeshSurface mainSurface;

//Yagiz

    [Header("Standard Mode")]
    [SerializeField] GameObject standard1;
    [SerializeField] GameObject standard2;

        [Header("OneHand Mode")]
    [SerializeField] GameObject oneHanded1;
    [SerializeField] GameObject oneHanded2;
    private GameObject button1;
    private GameObject button2;
    WaitForSeconds wait = new WaitForSeconds(.67f);


//yagiz end
    Quaternion targetLocalRot;
    bool rotating;
    float nextAllowedTime;

    [Header("Bridge Post Animation")]
    [SerializeField] Transform parent;
    [SerializeField] float animationDuration = 4f;
    float animationTimer = 0f;
    Vector3 startPos;
    Vector3 endPos;
    private bool isSinking;
    private bool enemiesSpawned = false;

    void Awake()
    {
        if (!rotateTarget) { Debug.LogError("Assign rotateTarget to Bridge Pivot"); enabled = false; return; }
        targetLocalRot = rotateTarget.localRotation;
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
        StartCoroutine(AnimateLoop());
    }

    void Update()
    {
        if (!rotating) return;

        if (isSinking)
        {
            animationTimer += Time.deltaTime / animationDuration;

            parent.position = Vector3.Lerp(startPos, endPos, animationTimer);

            if (animationTimer >= animationDuration)
            {
                isSinking = false;
                animationTimer = 0f;
            }
        }

        rotateTarget.localRotation = Quaternion.RotateTowards(
            rotateTarget.localRotation,
            targetLocalRot,
            degreesPerSecond * Time.deltaTime
        );

        if (Quaternion.Angle(rotateTarget.localRotation, targetLocalRot) <= arriveThreshold)
        {
            rotateTarget.localRotation = targetLocalRot;
            rotating = false;
            nextAllowedTime = Time.time + cooldownSeconds;

            // Rebuild navmesh once, after the move completes
            if (mainSurface != null)
            {
                // mainSurface.BuildNavMesh();
                // mainSurface.BuildNavMeshAsync();
                mainSurface.UpdateNavMesh(mainSurface.navMeshData);
            }
        }
    }

    void OnBridgeButtonPressed()
    {
        SpawnBridgeEnemies();
        FlipBridge();
    }

    void SpawnBridgeEnemies()
    {
        if (!enemiesSpawned)
    {
        startPos = parent.position;
        endPos = parent.position + Vector3.down * 5;
        enemiesSpawned = true;
        isSinking = true;
        OnEnemyButtonPressed?.Invoke();
    }
    }

    void FlipBridge()
    {
        if (rotating) return;
        if (Time.time < nextAllowedTime) return;


        targetLocalRot = targetLocalRot * Quaternion.Euler(0f, 180f, 0f);
        rotating = true;


        Destroy(button1.gameObject);
        Destroy(button2.gameObject);
    }

    IEnumerator AnimateLoop()
    {

            if (button1 != null) button1.SetActive(true);
            if (button2 != null) button2.SetActive(false);

            while (!rotating)
            {
                yield return wait;

                if (button1 == null || button2 == null) yield break;

                bool b1 = button1.activeSelf;
                button1.SetActive(!b1);
                button2.SetActive(b1);
            }
    }
    public void Interact() => OnBridgeButtonPressed();
}
