﻿using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMultiplier = 2;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private float _fireRate = 0.15f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldsPrefab;
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private GameObject _negativePowerPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _leftEngine, _rightEngine;
    [SerializeField] private GameObject _homingLaser;
    public float horizontalInput;
    
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _shieldPower = 3;
    [SerializeField] private int _ammocount = 15; 
    private int _score;
    
    [SerializeField] private SpawnManager _spawnManager;
     private UIManager _uiManager;
    
    private bool _IsTripleShotActive = false;
    private bool _IsSpeedBoosterActive = false;
    private bool _IsShieldsActive = false;
    
     private bool _IsHomingLaserActive = false;
    
    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private AudioClip _ammoBuzzerSound;
    private AudioSource _audioSource;

    
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is Null.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }


    
    void Update()
    {
        CalculateMovement();
        Speedup();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            AttractPickups();
        }
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_IsSpeedBoosterActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else if (_IsSpeedBoosterActive == true)
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        }


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }

    void AttractPickups()
    {
        Powerup[] pickups = FindObjectsOfType<Powerup>();
        foreach (Powerup pickup in pickups)
        {
            pickup.ActivateMagnet();
        }
    }
    
    
    void FireLaser()
    {
        if (_ammocount > 0)
        {
            _ammocount--;
            _uiManager.UpdateAmmoCount(_ammocount);
            _canFire = Time.time + _fireRate;

            if (_IsHomingLaserActive)
            {
                Instantiate(_homingLaser, transform.position, Quaternion.identity);
            }
            else if (_IsTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            }

            _audioSource.Play();
        }
    }

    public void Damage()
    {
        if (_IsShieldsActive)
        {
            TakeDamage();
            if (_shieldPower == 0)
            {
                _shieldVisualizer.SetActive(false); 
                _IsShieldsActive = false;
            }
        }
        else
        {
            _lives--;

       
            if (_lives < 0)
            {
                _lives = 0;
                Debug.LogWarning("Player lives dropped below 0. Clamped to 0.");
            }
        }

    
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject); 
        }

        ShakeOnHit();
    }

    public void TripleShotActive()
    {
        _IsTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _IsTripleShotActive = false;
    }

    public void SpeedBoosterActive()
    {
        _IsSpeedBoosterActive = true;
        StartCoroutine(SpeedBoosterPowerDownRoutine());
    }

    IEnumerator SpeedBoosterPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _IsSpeedBoosterActive = false;
    }

    public void ShieldsActive()
    {
        
        if (!_IsShieldsActive)
        {
            _shieldPower = 3; 
            _IsShieldsActive = true;
            _shieldVisualizer.SetActive(true); 
            ShieldsVisualize();
        }
    }
    
    
    public void AmmoActive()
    {
        _ammocount = 15;
        _uiManager.UpdateAmmoCount(_ammocount);
    }

    public void HealthActive()
    {
        if (_lives < 3)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);

            if (_lives == 2)
                _leftEngine.SetActive(false);
            else if (_lives == 3)
                _rightEngine.SetActive(false);
        }
    }

    public void NegativeActive()
    {
        if (_lives > 0)
        {
            Damage();
        }
    } 
    
    public void MagiccallerActive()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }

            Debug.Log("Magiccaler activated! All enemies destroyed.");
        }
    
     void Speedup()
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _speed = 10;
                }
                else
                {
                    _speed = 5;
                }
            }
     
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MagiccallerActive();
            Destroy(gameObject);
        }
    }
    

        public void Addscore(int points)
        {
            _score += points;
            _uiManager.UpdateScore(_score);

        }
    
        public void TakeDamage()
        {
            if (_IsShieldsActive)
            {
                _shieldPower--;
                ShieldsVisualize();
            }
        }

        public void ShieldsVisualize()
        {
            if (_IsShieldsActive)
            {
                SpriteRenderer sr = _shieldVisualizer.GetComponent<SpriteRenderer>();

                switch (_shieldPower)
                {
                    case 3: sr.color = Color.green; break;
                    case 2: sr.color = Color.yellow; break;
                    case 1: sr.color = Color.red; break;
                    case 0:
                        _IsShieldsActive = false;
                        _shieldVisualizer.SetActive(false); 
                        break;
                }
            }
        }

        public void ShakeOnHit()
        {
            ShakeCamera cameraShaker = FindObjectOfType<ShakeCamera>();
            if (cameraShaker != null)
            {
                cameraShaker.InitiateShake(0.3f);
            }
        }
    

        public void ActivateHomingLaser()
        {
            _IsHomingLaserActive = true;
            StartCoroutine(HomingLaserPowerDownRoutine());
        }

        IEnumerator HomingLaserPowerDownRoutine()
        {
            yield return new WaitForSeconds(5.0f);
           _IsHomingLaserActive  = false; 
        }
    }

