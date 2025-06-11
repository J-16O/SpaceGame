using UnityEngine;

public class BossLaser : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction; 

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.Damage();
            }

            Destroy(gameObject);
        }
    }
}