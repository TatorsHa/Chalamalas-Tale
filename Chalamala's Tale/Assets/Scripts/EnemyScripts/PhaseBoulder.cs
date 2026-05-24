using System.Collections;
using UnityEngine;

/// Spawns a persistent boulder with a visual warning before it becomes solid.
/// After the warning delay the collider is enabled and it stays for the rest of the phase.

public class PhaseBoulder : MonoBehaviour
{
    [Header("Warning")]
    [SerializeField] private float warningDelaySeconds = 2f;
    [Tooltip("Sprite shown during the warning phase (e.g. a faint outline).")]
    [SerializeField] private Sprite warningSprite;
    [Tooltip("Sprite shown once the boulder is solid.")]
    [SerializeField] private Sprite solidSprite;
    [Tooltip("Alpha of the warning sprite (0 = invisible, 1 = fully opaque).")]
    [SerializeField] [Range(0f, 1f)] private float warningAlpha = 0.4f;

    private SpriteRenderer sr;
    private Collider2D[] cols;

    public void Initialize(float warningDelay)
    {
        warningDelaySeconds = Mathf.Max(0f, warningDelay);
        StopAllCoroutines();
        if (isActiveAndEnabled)
        {
            StartCoroutine(AppearSequence());
        }
    }

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        cols = GetComponentsInChildren<Collider2D>(includeInactive: true);

        // Start in warning state
        SetCollidersEnabled(false);

        if (sr != null)
        {
            if (warningSprite != null)
            {
                sr.sprite = warningSprite;
            }

            Color c = sr.color;
            c.a = warningAlpha;
            sr.color = c;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(AppearSequence());
    }

    private IEnumerator AppearSequence()
    {
        yield return new WaitForSeconds(Mathf.Max(0f, warningDelaySeconds));

        // Become solid
        if (sr != null)
        {
            if (solidSprite != null)
            {
                sr.sprite = solidSprite;
            }

            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }

        SetCollidersEnabled(true);
    }

    private void SetCollidersEnabled(bool value)
    {
        if (cols == null) return;

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i] != null)
            {
                cols[i].enabled = value;
            }
        }
    }
}
