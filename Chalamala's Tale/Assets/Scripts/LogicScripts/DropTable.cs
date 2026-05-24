using UnityEngine;

[System.Serializable]
public class DropTableEntry
{
    public GameObject prefab;
    [Range(0f, 1f)] public float chance = 1f;
    public int minCount = 1;
    public int maxCount = 1;
}

public class DropTable : MonoBehaviour
{
    [SerializeField] private Transform dropOrigin;
    [SerializeField] private Vector2 spawnJitter = new Vector2(0.35f, 0.35f);
    [SerializeField] private DropTableEntry[] drops;

    private bool hasSpawned;

    public bool SpawnDrops()
    {
        if (hasSpawned)
        {
            return false;
        }

        hasSpawned = true;
        bool spawnedAny = false;
        Vector3 origin = dropOrigin != null ? dropOrigin.position : transform.position;

        foreach (DropTableEntry entry in drops)
        {
            if (entry == null || entry.prefab == null)
            {
                continue;
            }

            if (Random.value > Mathf.Clamp01(entry.chance))
            {
                continue;
            }

            int minimum = Mathf.Max(0, entry.minCount);
            int maximum = Mathf.Max(minimum, entry.maxCount);
            int spawnCount = Random.Range(minimum, maximum + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                Instantiate(entry.prefab, GetSpawnPosition(origin), Quaternion.identity);
                spawnedAny = true;
            }
        }

        return spawnedAny;
    }

    private Vector3 GetSpawnPosition(Vector3 origin)
    {
        return origin + new Vector3(
            Random.Range(-spawnJitter.x, spawnJitter.x),
            Random.Range(-spawnJitter.y, spawnJitter.y),
            0f);
    }
}