using UnityEngine;

public class PlayerMeleeSwipeAttackArea : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        if (playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }

        AimAtNearestEnemy();
        Debug.Log($"Parent position: {transform.parent.position}, AttackArea position: {transform.position}");
    }

    private void AimAtNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            transform.localPosition = new Vector3(0f, -5f, 0f);
            return;
        }

        PlayerController player =
            transform.parent.GetComponent<PlayerController>();

        Vector2 facingDirection = GetFacingDirection(player.CurrentFacing);

        GameObject nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            Vector2 toEnemy =
                (enemy.transform.position - transform.parent.position).normalized;

            float dot = Vector2.Dot(facingDirection, toEnemy);

            // Ignore only enemies behind player
            if (dot < 0f)
                continue;

            float distance =
                Vector2.Distance(transform.parent.position,
                                enemy.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }

        if (nearest != null)
        {
            Vector2 direction =
                (nearest.transform.position - transform.parent.position).normalized;

            float angle =
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            // No enemy in front → attack straight ahead
            float angle =
                Mathf.Atan2(facingDirection.y, facingDirection.x)
                * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        transform.localPosition = new Vector3(0f, -5f, 0f);
    }
    private Vector2 GetFacingDirection(PlayerController.PlayerFacingDirection facing)
    {
        switch (facing)
        {
            case PlayerController.PlayerFacingDirection.Up:
                return Vector2.up;

            case PlayerController.PlayerFacingDirection.Down:
                return Vector2.down;

            case PlayerController.PlayerFacingDirection.Left:
                return Vector2.left;

            case PlayerController.PlayerFacingDirection.Right:
                return Vector2.right;

            default:
                return Vector2.down;
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check first whether the object hit has the tag enemy, as to avoid damaging a door
        // (This tag has to be assigned manually to the enemy game objects)
        if (collider.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponentInParent<IDamageable>()?.TakeDamage(GetDamageAmount());
            Debug.Log("Enemy hit!");
        }
        /**
        // If an enemy is in the trigger collider area, check if it has a health component
        enemyHealth = collider.GetComponent<currentHealth>();

        if(enemyHealth > 0)
        {
            // Make the enemy take damage
            enemyHealth.TakeDamage(damage);
        }
        **/
    }

    private float GetDamageAmount()
    {
        if (playerController == null)
        {
            return 0f;
        }

        return playerController.GetAttackDamage();
    }
}
