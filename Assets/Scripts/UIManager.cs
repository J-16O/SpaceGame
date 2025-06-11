using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Image _ammoCountImg;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Text _ammoCountText;
    [SerializeField] private Sprite[] _ammoSprites;
    

   
    void Start()
    {
        _scoreText.text = "Score:" + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }
    

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score:" + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives < 0)
        {
            currentLives = 0;
            Debug.LogWarning("Clamped currentLives to 0 because it was negative.");
        }

        if (currentLives >= _liveSprites.Length)
        {
            currentLives = _liveSprites.Length - 1;
            Debug.LogWarning("Clamped currentLives to max index of liveSprites.");
        }

        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }



    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = " ";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateAmmoCount(int currentAmmoCount)
    {
        _ammoCountText.text = "Ammo: " + currentAmmoCount;
    }
    
}