using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int totalCollectibles = 8;
    [SerializeField] private float timeRemaining = 45f;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private GameObject restartButton;

    private int collectedCount;
    private bool gameEnded;

    public bool AllCollected => collectedCount >= totalCollectibles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (resultText != null)
            resultText.gameObject.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(false);

        UpdateUI();
    }

    private void Update()
    {
        if (gameEnded)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            Lose();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";

        if (countText != null)
            countText.text = $"Collected: {collectedCount}/{totalCollectibles}";
    }

    public void CollectOne()
    {
        if (gameEnded)
            return;

        collectedCount++;
        UpdateUI();
    }

    public void Win()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        ShowResult("You Win!");
    }

    public void Lose()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        ShowResult("You Lose!");
    }

    private void ShowResult(string message)
    {
        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.text = message;
        }

        if (restartButton != null)
            restartButton.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}