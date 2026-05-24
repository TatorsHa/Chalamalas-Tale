using UnityEngine;
using System.Collections.Generic;

public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleEffect effect = CollectibleEffect.None;
    [SerializeField] private float amount = 1f;
    [SerializeField] private bool persistAfterCollection;
    [SerializeField] private string persistentId;

    private static readonly HashSet<string> CollectedPersistentIds = new HashSet<string>();

    private void Start()
    {
        if (!persistAfterCollection || string.IsNullOrWhiteSpace(persistentId))
        {
            return;
        }

        if (CollectedPersistentIds.Contains(persistentId))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        PlayerController playerController = other.GetComponentInParent<PlayerController>();

        // Ignore attack hitboxes or unrelated trigger bodies that do not belong to the player root.
        if (playerHealth == null && playerController == null)
        {
            return;
        }

        bool wasCollected = ApplyEffect(playerHealth, playerController);

        if (!wasCollected)
        {
            return;
        }

        if (persistAfterCollection && !string.IsNullOrWhiteSpace(persistentId))
        {
            CollectedPersistentIds.Add(persistentId);
        }

        Destroy(gameObject);
    }

    private bool ApplyEffect(PlayerHealth playerHealth, PlayerController playerController)
    {
        switch (ResolveEffect())
        {
            case CollectibleEffect.UnlockRangedAttack:
                playerController?.EnablePlayerRangedAttack();
                return playerController != null;

            case CollectibleEffect.IncreaseMaxHealth:
                playerHealth?.IncreaseMaxHealth(amount);
                return playerHealth != null;

            case CollectibleEffect.IncreaseDamage:
                playerController?.IncreaseAttackDamage(Mathf.RoundToInt(amount));
                return playerController != null;

            case CollectibleEffect.Heal:
                return playerHealth != null && playerHealth.Heal(amount);

            default:
                return false;
        }
    }

    private CollectibleEffect ResolveEffect()
    {
        if (effect != CollectibleEffect.None)
        {
            return effect;
        }

        switch (gameObject.tag)
        {
            case "RangedAttackUpgrade":
                return CollectibleEffect.UnlockRangedAttack;
            case "PermanentHealthUpgrade":
                return CollectibleEffect.IncreaseMaxHealth;
            case "DamageUpgrade":
                return CollectibleEffect.IncreaseDamage;
            case "HealthDrop":
                return CollectibleEffect.Heal;
            default:
                return CollectibleEffect.None;
        }
    }
}

public enum CollectibleEffect
{
    None,
    UnlockRangedAttack,
    IncreaseMaxHealth,
    IncreaseDamage,
    Heal,
}