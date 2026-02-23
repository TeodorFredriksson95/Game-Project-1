using MelenitasDev.SoundsGood;
using System;
using System.Collections;
using UnityEngine;
using VInspector;

public class Pedastol : MonoBehaviour
{
    public InteractableType Type => InteractableType.Default;
    [Tab("Pedastol Settings")]
    [SerializeField] private float scaleFactor = 1.5f;
    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private Transform creationSpot;
    [SerializeField] private bool relic;

    [Tab("Spin Settings")]
    [SerializeField] private bool spin = true;
    [SerializeField] private float spinSpeedDegreesPerSecond = 45f;
    [SerializeField] private Vector3 spinAxis = Vector3.up;

    [Tab("Bob Settings")]
    [SerializeField] private bool bob = true;
    [SerializeField] private float bobAmplitude = 0.00067f; 
    [SerializeField] private float bobFrequency = 0.67f; 
    [SerializeField] private float bobPhase = 0f; 

    [Tab("Indicator")]

    
    [Header("Standard Mode")]
    [SerializeField] GameObject standard1;
    [SerializeField] GameObject standard2;

        [Header("OneHand Mode")]
    [SerializeField] GameObject oneHanded1;
    [SerializeField] GameObject oneHanded2;
    private GameObject button1;
    private GameObject button2;
    WaitForSeconds wait = new WaitForSeconds(.67f);

    Sound tonalHint;
    public bool IsUsed { get; private set; }

    // Grab *all* ParticleSystems in this chest hierarchy (even inactive)
    ParticleSystem[] particleSystems;

    private Transform instantiatedReward;
    private Vector3 rewardBaseLocalPos;

    void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(includeInactive: true);

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

    void Start()
    {
        tonalHint = new Sound(SFX.TonalHint);
        tonalHint.SetOutput(Output.SFX);
        tonalHint.SetSpatialSound();
        tonalHint.SetVolume(0.3f);
        tonalHint.SetPosition(transform.position);

        if (rewardPrefab != null && creationSpot != null)
        {
            var obj = Instantiate(rewardPrefab, creationSpot.position, creationSpot.rotation);
            obj.transform.SetParent(creationSpot, worldPositionStays: false);

            obj.transform.localPosition = Vector3.zero;

            obj.transform.localRotation = rewardPrefab.transform.localRotation;

            Vector3 prefabLocal = rewardPrefab.transform.localScale;
            Vector3 parentLossy = creationSpot.lossyScale;

            Vector3 safeParentLossy = new Vector3(
                parentLossy.x != 0f ? parentLossy.x : 1f,
                parentLossy.y != 0f ? parentLossy.y : 1f,
                parentLossy.z != 0f ? parentLossy.z : 1f
            );

            Vector3 desiredLocalScale = new Vector3(
                prefabLocal.x * scaleFactor / safeParentLossy.x,
                prefabLocal.y * scaleFactor / safeParentLossy.y,
                prefabLocal.z * scaleFactor / safeParentLossy.z
            );

            obj.transform.localScale = desiredLocalScale;

            instantiatedReward = obj.transform;
            rewardBaseLocalPos = instantiatedReward.localPosition;
        }
    }

    void OnEnable()
    {
        StartCoroutine(AnimateLoop());
    }

    private void Update()
    {
        // Rotate the creation spot so children rotate with it.
        if (spin && creationSpot != null)
        {
            creationSpot.Rotate(spinAxis.normalized, spinSpeedDegreesPerSecond * Time.deltaTime, Space.Self);
        }

        // Bob the instantiated reward up and down in local space
        if (bob && instantiatedReward != null)
        {
            float y = Mathf.Sin(Time.time * bobFrequency * Mathf.PI * 2f + bobPhase) * bobAmplitude;
            instantiatedReward.localPosition = rewardBaseLocalPos + Vector3.up * y;
        }
        if(instantiatedReward == null)
        {
            if(button1 != null)
            {
                Destroy(button1);
                Destroy(button2);
            }
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