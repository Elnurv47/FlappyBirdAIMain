using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform upperPipe;
    [SerializeField] private Transform lowerPipe;

    [SerializeField] private Transform CheckpointUpperYTransform;
    [SerializeField] private Transform CheckpointLowerYTransform;

    public Transform UpperPipe { get => upperPipe; }
    public Transform LowerPipe { get => lowerPipe; }

    public float CheckpointUpperY { get => CheckpointUpperYTransform.position.y; }
    public float CheckpointLowerY { get => CheckpointLowerYTransform.position.y; }

    private void Start()
    {
        Destroy(gameObject, 6f);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += new Vector3(-1, 0) * Time.deltaTime * moveSpeed;
    }
}
