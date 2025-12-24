using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private float minXOffset = 2f;
    [SerializeField] private float maxXOffset = 3f;
    [SerializeField] private float minSpawnY = -3f;
    [SerializeField] private float maxSpawnY = 3f;
    [SerializeField] private float spawnChance = 0.5f;
    [SerializeField] private PipeSpawner pipeSpawner;

    private void Start()
    {
        pipeSpawner.OnPipeSpawned += PipeSpawner_OnPipeSpawned;
    }
    void SpawnCollectible(Transform pipeTransform)
    {
        if (Random.value > spawnChance) return;


        float randomSpawnY = Random.Range(minSpawnY, maxSpawnY);

        Vector3 spawnPos = new Vector3(pipeTransform.position.x + Random.Range(minXOffset, maxXOffset),
                                       pipeTransform.position.y + randomSpawnY,
                                       0f);

        Instantiate(collectiblePrefab, spawnPos, Quaternion.identity);
    }

    public void PipeSpawner_OnPipeSpawned(Transform pipeTransform)
    {
        SpawnCollectible(pipeTransform);
    }
}
