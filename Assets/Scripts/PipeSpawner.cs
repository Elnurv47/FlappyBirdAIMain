using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeSpawner : MonoBehaviour
{
    private static Queue<Pipe> pipes;
    public static Queue<Pipe> Pipes { get => pipes; }

    [SerializeField] private Pipe pipePrefab;

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

            yield return new WaitForSeconds(2);
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
