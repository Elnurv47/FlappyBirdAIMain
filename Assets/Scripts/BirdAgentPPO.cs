using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class BirdAgentPPO : Agent
{
    private float timer = 0f;

    [SerializeField] private Bird bird;
    [SerializeField] private Rigidbody2D rb2D;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            AddReward(0.3f);
            timer = 0f;
        }

        AddReward(0.001f);
    }

    public override void OnEpisodeBegin()
    {
        timer = 0f;
        bird.Reset();
        PipeSpawner.DestroyAllPipes();
        Collectible[] existing = FindObjectsByType<Collectible>(FindObjectsSortMode.None);
        foreach (var c in existing) Destroy(c.gameObject);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;

        sensor.AddObservation(transform.position.y / screenHeight);
        sensor.AddObservation(rb2D.linearVelocity.y / 10f);

        Pipe nextPipe = PipeSpawner.GetNextPipe();
        if (nextPipe != null)
        {
            float xDist = nextPipe.transform.position.x - transform.position.x;
            float gapCenterY = (nextPipe.transform.position.y + nextPipe.LowerPipe.transform.position.y) / 2f;
            float gapYDist = gapCenterY - transform.position.y;

            sensor.AddObservation(xDist / screenWidth);
            sensor.AddObservation(gapYDist / screenHeight);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }

        Collectible[] collectibles = FindObjectsByType<Collectible>(FindObjectsSortMode.None);
        Collectible nearest = null;
        float minX = float.MaxValue;

        foreach (var c in collectibles)
        {
            float xDist = c.transform.position.x - transform.position.x;
            if (xDist >= 0 && xDist < minX)
            {
                minX = xDist;
                nearest = c;
            }
        }

        if (nearest != null)
        {
            float yDist = nearest.transform.position.y - transform.position.y;
            sensor.AddObservation(yDist / screenHeight);
            sensor.AddObservation(minX / screenWidth);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.DiscreteActions[0] == 1)
        {
            bird.Jump();
            Pipe nextPipe = PipeSpawner.GetNextPipe();
            if (nextPipe != null)
            {
                if (transform.position.y < nextPipe.CheckpointUpperY - 0.5f && transform.position.y > nextPipe.CheckpointLowerY + 0.5f)
                {
                    AddReward(0.8f);
                }
            }

            if (transform.position.y > Camera.main.orthographicSize)
            {
                AddReward(-1f);
                EndEpisode();
            }
        }

        AddReward(-0.001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            PipeSpawner.Pipes.Dequeue();
            AddReward(1.0f);
        }
        if (collision.CompareTag("Ground") || collision.CompareTag("Pipe"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
        if (collision.TryGetComponent<Collectible>(out var coin))
        {
            AddReward(1f);
            Destroy(coin.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Pipe"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }
}
