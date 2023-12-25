using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Transform cubeParent;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI cubesCollectedText;

    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private float gameDuration = 120;
    private float timer;

    private int totalCubes = 0;
    private int points = 0;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        totalCubes = cubeParent.childCount;
        timer = gameDuration;
    }
    private void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;

            int min = Mathf.FloorToInt(timer / 60);
            int sec = Mathf.FloorToInt(timer % 60);

            timerText.text = min.ToString("00") + ":" + sec.ToString("00");
        }
        else if (!isGameOver)
        {
            GameOver();
        }
    }
    public void AddPoint()
    {
        points++;

        if (points >= totalCubes)
        {
            GameOver();
        }
        cubesCollectedText.text = points + " / " + totalCubes;
    }
    public void GameOver()
    {
        if (isGameOver)
            return;

        Debug.Log("Game Over");

        isGameOver = true;
        gameOverPanel.SetActive(true);

        timer = 0;
        timerText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}