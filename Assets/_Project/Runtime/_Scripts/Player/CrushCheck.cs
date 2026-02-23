using System.Collections;
using UnityEngine;

public class CrushCheck : MonoBehaviour
{
    [SerializeField, Tooltip("Show crush debug logs in console.")]
    private bool debug = false;

    [SerializeField, Tooltip("Damage dealt to enemies when stepped on.")]
    private float crushDamage = 9999f;

    [SerializeField, Tooltip("Number of frames to freeze the CharacterController after a crush.")]
    private int freezeTicks = 2;

    CharacterController controller;

    void Awake()
    {
        controller = GetComponentInParent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool crushedSomething = false;

        if (other.TryGetComponent(out Crate crate))
        {
            if (debug)
                Debug.Log($"[CrushCheck] Destroying crate: {crate.name}");
            Destroy(crate.gameObject);
            crushedSomething = true;
        }

        else if (other.TryGetComponent(out IDamageable damageable))
        {
            if (debug)
                Debug.Log($"[CrushCheck] Crushing enemy: {other.name} for {crushDamage} damage");

            damageable.TakeDamage(crushDamage);

            crushedSomething = true;
        }

        if (crushedSomething && controller != null)
            StartCoroutine(FreezeController());
    }

    IEnumerator FreezeController()
    {
        controller.enabled = false;
        for (int i = 0; i < freezeTicks; i++)
            yield return null; // wait one frame
        controller.enabled = true;

        if (debug)
            Debug.Log($"[CrushCheck] Controller unfrozen after {freezeTicks} ticks.");
    }
}
