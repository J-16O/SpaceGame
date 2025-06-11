using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    [SerializeField] private Text _finishText;
    [SerializeField] private Text _restartText;
    

    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Restart key pressed");
            SceneManager.LoadScene(1); 
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void EndGame()
    {
        _finishText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _isGameOver = true;
    }

    public void GameOver()
    {
        Debug.Log("GameManager::GameOver");
        _isGameOver = true;
    }
    }