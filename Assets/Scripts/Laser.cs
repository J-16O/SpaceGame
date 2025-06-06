using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 7.0f;
    [SerializeField] private AudioClip _ammoBuzzer;
    public int _AmmoCount = 15;
    private bool _isEnemyLaser = false; 
    public bool isPlayerLaser = true;

    
    void Update()
    {
        if (_isEnemyLaser)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
        
        
        if (Mathf.Abs(transform.position.y) > 10f)
        {
            Destroy(this.gameObject);
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
        isPlayerLaser = false; 
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }

        
        if (other.CompareTag("Enemy") && isPlayerLaser)
        {
            Destroy(other.gameObject); 
            Destroy(this.gameObject); 
        }

       
        if (other.CompareTag("Pickups"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        
        
        if (other.CompareTag("Boss") && isPlayerLaser)
        {
            
            Debug.Log("Laser collided with: " + other.name);
            Boss boss = other.GetComponent<Boss>() ?? other.GetComponentInParent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage();
            }
            else
            {
                Debug.Log("Boss is null");
            }
            Destroy(gameObject);
        }
        
    }
}

