using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerups;
  
   [SerializeField]
    private GameObject ammoPrefab;
    [SerializeField]
    private GameObject magiccalerPrefab;

    [SerializeField] private bool _stopSpawning = false;

    private float minEnemySpawnTime = 2f;
    private float maxEnemySpawnTime = 5f;
    private float maxPickupSpawnTime = 8f;

    [SerializeField] private float nextEnemySpawnTime;
    [SerializeField] private float nextPickupSpawnTime;
    
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _littleLevelText;


    [SerializeField] private GameObject bossPrefab;
    public GameManager gameManager;
    private int currentLevel = 1 ;
    private Coroutine powerupCoroutine;
    private Boss Boss;
    private bool bossSpawned = false;
  
    

    public void Update()
    {
        if (currentLevel == 4 && !bossSpawned)
        {
            Debug.Log("Trying to spawn boss...");
            SpawnBoss();
        }
    }


    private IEnumerator SpawnLevelsSequentially()
    { for (int i = 1; i <= 4; i++)
        {
            yield return StartCoroutine(StartLevel(i));
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0); 
            yield return new WaitForSeconds(2f); 
        }
        
    }
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnLevelsSequentially());

        if (currentLevel < 4)
        {
            powerupCoroutine = StartCoroutine(SpawnPowerupRoutine());
        }
        else if (powerupCoroutine != null)
        {
            StopCoroutine(powerupCoroutine);
            powerupCoroutine = null;
        }
            
    
    }
    

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {

            Vector3 posTospawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 8);
            Instantiate(_powerups[randomPowerup], posTospawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

   

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


    IEnumerator StartLevel(int currentLevel)
    {
        _levelText.text = "LEVEL " + currentLevel;
        _levelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        _levelText.gameObject.SetActive(false);

        this.currentLevel = currentLevel; // ✅ update the field!
        _littleLevelText.text = "Level " + currentLevel;
        
        {
            if (currentLevel <= 3)
            {
                int enemyCount = 5;
                if (currentLevel == 2) enemyCount = 7;
                else if (currentLevel == 3) enemyCount = 10;
                for (int i = 0; i < enemyCount; i++)
                {
                    Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 6f, 0f);
                    Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
                }
                
            }
        }
        yield return null;
    }
    
    void SpawnBoss()
    {
        Vector3 spawnPos = new Vector3(0f, 2f, 0f);
        Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        bossSpawned = true;
    }
    
}


