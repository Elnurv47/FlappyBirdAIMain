using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BirdAgent : Agent
{
    private float timer = 0f;
    private bool isInitializing = false;

    [SerializeField] private Bird bird;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private MultiAgentAcademy academy;

    private void Start()
    {
        //BirdManager.Instance.RegisterBird(this);
    }

    private void ApplyGroupPerformanceReward()
    {
        if (timer > 5f)
        {
            BirdManager.Instance.AgentGroup.AddGroupReward(1.0f);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1f)
        {
            AddReward(0.1f);
            timer = 0f;
        }

        ApplyGroupPerformanceReward();
    }

    public bool IsInitializing() => isInitializing;

    public override void OnEpisodeBegin()
    {
        Debug.Log("HERE");
        BirdManager.Instance.RegisterBird(this);
        //Debug.Log(bird.gameObject.name + " " + bird.gameObject.activeSelf + " " + bird.gameObject.activeInHierarchy);
        BirdManager.Instance.SetActive(bird.gameObject);
        isInitializing = true;
        timer = 0f;
        bird.Reset();
        PipeSpawner.DestroyAllPipes();
        isInitializing = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.y);
        sensor.AddObservation(rb2D.linearVelocity.y);

        Pipe nextPipe = PipeSpawner.GetNextPipe();

        if (nextPipe != null)
        {
            sensor.AddObservation(nextPipe.transform.position.x - transform.position.x);

            float gapCenterY = (nextPipe.transform.position.y + nextPipe.LowerPipe.transform.position.y) / 2;
            sensor.AddObservation(gapCenterY - transform.position.y);
        }

        float distanceToGround = transform.position.y;
        sensor.AddObservation(distanceToGround);

        float distanceToCeiling = Camera.main.orthographicSize * 2 - transform.position.y;
        sensor.AddObservation(distanceToCeiling);
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

            if (transform.position.y > 5f)
            {
                AddReward(-0.5f);
                //EndEpisode();
                BirdManager.Instance.BirdFailed(this);
            }
        }

        AddReward(0.05f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            if (PipeSpawner.Pipes.Count == 0)
            {
                PipeSpawner.Pipes.Dequeue();
            }

            AddReward(1.0f);
        }

        if (collision.CompareTag("Ground"))
        {
            AddReward(-1.0f);
            //EndEpisode();
            BirdManager.Instance.BirdFailed(this);
        }

        if (collision.TryGetComponent(out Pipe pipe))
        {
            AddReward(-1.0f);
            //EndEpisode();
            BirdManager.Instance.BirdFailed(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Pipe"))
        {
            AddReward(-1.0f);
            //EndEpisode();
            BirdManager.Instance.BirdFailed(this);
        }
    }

    public void ApplyGroupReward(float reward)
    {
        academy.AddGroupReward(reward);
    }
}
