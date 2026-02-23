using UnityEngine;
using UnityEngine.Rendering;

public class SecretDoor : MonoBehaviour
{
    [SerializeField] bool isVisible;
    [SerializeField] Pedastol relicPedastol;

    BoxCollider bc;
    MeshRenderer mr;

    private void OnEnable()
    {
        Relic.OnRelicPickedUp += SwitchDoorVisibility;
    }
    private void OnDisable()
    {
        Relic.OnRelicPickedUp -= SwitchDoorVisibility;
    }

    void Start()
    {
        bc = GetComponent<BoxCollider>();
        mr = GetComponentInChildren<MeshRenderer>();

        bc.enabled = isVisible;
        mr.enabled = isVisible;
    }

    void SwitchDoorVisibility()
    {
        isVisible = !isVisible;

        bc.enabled = isVisible;
        mr.enabled = isVisible;
    }

}
