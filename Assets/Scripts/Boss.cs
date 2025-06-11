using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject bosslaserPrefab;
    [SerializeField] private float fireRate = 2.0f;
    private float _canFire = -1f;
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    void Update()
    {


        if (Time.time > _canFire)
        {
            _canFire = Time.time + fireRate;

            GameObject laser = Instantiate(bosslaserPrefab, transform.position, Quaternion.identity);

            
            BossLaser bossLaserScript = laser.GetComponent<BossLaser>();
            if (bossLaserScript != null)
            {
                bossLaserScript.direction = Vector2.down; 
            }
        }
    }
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damageAmount = 1)
    {
        currentHealth -= damageAmount;
        Debug.Log("Boss took damage. Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);

       
    }
    


}