using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHits = 10;  
    private int currentHits = 0;
    private GameManager gameManager;
    public void TakeDamage()
    {
        currentHits++;
        Debug.Log("Boss hit: " + currentHits + "/" + maxHits);

        if (currentHits >= maxHits)
        {
            Debug.Log("Boss defeated!");
            gameManager = FindObjectOfType<GameManager>(); 
            gameManager.EndGame();                        
            Destroy(gameObject);                        
        }
    }










}