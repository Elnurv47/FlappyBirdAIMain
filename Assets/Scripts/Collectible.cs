using UnityEngine;

public class Collectible : MonoBehaviour
{
    private float moveSpeed = 2f;

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