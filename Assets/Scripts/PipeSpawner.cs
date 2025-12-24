using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private float pipeSpawnerIntervalMin = 2f;
    [SerializeField] private float pipeSpawnerIntervalMax = 4f;

    private static Queue<Pipe> pipes;
    public static Queue<Pipe> Pipes { get => pipes; }

    [SerializeField] private Pipe pipePrefab;

    public Action<Transform> OnPipeSpawned;

    private void Awake()
    {
        pipes = new Queue<Pipe>();

        StartCoroutine(SpawnPipeCoroutine());
    }

    private IEnumerator SpawnPipeCoroutine()
    {
        while (true)
        {
            Vector2 spawnPosition = 
                new Vector2(
                    transform.parent.position.x + 8, 
                    Random.Range(transform.parent.position.y - 4.5f, transform.parent.position.y - 2f));
            Pipe spawnedPipe = Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
            pipes.Enqueue(spawnedPipe);
            OnPipeSpawned?.Invoke(spawnedPipe.gameObject.transform);

            float pipeSpawnerInterval = Random.Range(pipeSpawnerIntervalMin, pipeSpawnerIntervalMax);
            yield return new WaitForSeconds(pipeSpawnerInterval);
        }
    }

    public static Pipe GetNextPipe()
    {
        if (pipes.Count == 0) return null;
        Pipe nextPipe = pipes.Peek();
        return nextPipe;
    }

    public static void DestroyAllPipes()
    {
        while (pipes != null && pipes.Count > 0)
        {
            var pipe = pipes.Dequeue();
            if (pipe != null)
            {
                Destroy(pipe.gameObject);
            }
        }
    }
}
