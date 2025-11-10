using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    private float jumpForce = 8f;

    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private ScoreSystem scoreSystem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    public void Jump()
    {
        rb2D.velocity = Vector2.up * jumpForce;
    }

    public void Reset()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        rb2D.velocity = Vector3.zero;
        scoreSystem.ResetScore();
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Pipe"))
        {
            SceneManager.LoadScene(0);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            scoreSystem.AddScore();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Collectible>(out Collectible collectible))
        {
            Destroy(collectible.gameObject);
            scoreSystem.AddScore();
        }
    }
}
