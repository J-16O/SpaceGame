using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _aggressionRange = 3f;
    [SerializeField] private float _ramSpeed = 5f;
    [SerializeField] private Transform firePoint;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private int _health = 1;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _shieldVisualizer;

    private Player _player;
    private Transform _playerTransform;
    private Animator _anim;
    private AudioSource _audioSource;
    
    private bool _moveleft;
    private bool _isRamming = false;
    private bool _isAggressive = false;

    void Start()
    {
        InitializeEnemy();
        StartCoroutine(ZigZagMovement());
    }

    void Update()
    {
        CalculateMovement();
        AlternateMovement();
        HandleAggression();

        if (Time.time > _canFire)
        {
            FireLaser();
        }
    }
    
    // Aggression logic
    private void HandleAggression()
    {
        if (_isAggressive && _playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= _aggressionRange)
            {
                _isRamming = true;
            }

            if (_isRamming)
            {
                transform.position = Vector2.MoveTowards(transform.position, _playerTransform.position, _ramSpeed * Time.deltaTime);
            }
        }
    }


    private void InitializeEnemy()
    {
        _playerTransform = GameObject.FindWithTag("Player")?.transform;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null) Debug.LogError("Player is NULL.");
        if (_anim == null) Debug.LogError("Animator is NULL.");

        // Random aggression
        if (Random.value < 1f)
        {
            _isAggressive = true;
        }

        // Shield logic
        if (Random.value < 0.3f)
        {
            _health = 2;
            _shieldVisualizer?.SetActive(true);
        }
        else
        {
            _health = 1;
            _shieldVisualizer?.SetActive(false);
        }
    }
    
    void FireLaser()
    {
        _fireRate = Random.Range(3f, 7f);
        _canFire = Time.time + _fireRate;

        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        foreach (Laser laser in lasers)
        {
            laser.AssignEnemyLaser();
        }
    }
    
    // Movement logic
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7f, 0);
        }
    }

    // Zigzag movement logic
    void AlternateMovement()
    {
        Vector3 direction = _moveleft ? new Vector3(-1f, -1f, 0f) : new Vector3(1f, -1f, 0f);
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    IEnumerator ZigZagMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            _moveleft = !_moveleft;
        }
    }

    // On collision logic
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            player?.Damage();

            TriggerDeath();
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            _health--;

            if (_health == 1)
            {
                _shieldVisualizer?.SetActive(false);
            }

            if (_health <= 0)
            {
                _player?.Addscore(10);
                TriggerDeath();
            }
        }
    }
    

    void TriggerDeath()
    {
        _anim.SetTrigger("On Enemy Death");
        _speed = 0;
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 2.5f);
    }
}

