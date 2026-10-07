using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudioSource;

    private bool isMuted;
    private const string MusicMutedKey = "MusicMuted";

    [SerializeField] private TMP_Text startMusicButtonText;
    [SerializeField] private TMP_Text gameMusicButtonText;

    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private TMP_Text heightText;
    [SerializeField] private TMP_Text startHighestScoreText;
    [SerializeField] private TMP_Text gameHighestScoreText;

    [SerializeField] private GameObject difficultyCanvas;
    [SerializeField] private TMP_Text difficultyText;

    public static GameManager Instance { get; private set; }

    private float highestHeight = 0f;
    public float HighestHeight => highestHeight;
    private const string HighScoreKey = "HighScore";

    private float currentHeight = 0f;
    public float CurrentHeight => currentHeight;

    private float baseTopY;

    private string selectedDifficulty = "EASY";
    public string SelectedDifficulty => selectedDifficulty;

    private void Awake()
    {
        Time.timeScale = 0f;

        isMuted = PlayerPrefs.GetInt(MusicMutedKey, 0) == 1;

        if (bgmAudioSource != null)
        {
            bgmAudioSource.mute = isMuted;
        }

        UpdateMusicButtonText();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        highestHeight = PlayerPrefs.GetFloat(HighScoreKey, 0f);

        UpdateHighScoreUI();

        GameObject baseObject = GameObject.Find("Base");

        if (baseObject != null)
        {
            Collider2D baseCollider = baseObject.GetComponent<Collider2D>();

            if (baseCollider != null)
            {
                baseTopY = baseCollider.bounds.max.y;
            }
        }

        Debug.Log("Base top Y: " + baseTopY);
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;

        PlayerPrefs.SetInt(
            MusicMutedKey,
            isMuted ? 1 : 0
        );

        PlayerPrefs.Save();

        if (bgmAudioSource != null)
        {
            bgmAudioSource.mute = isMuted;
        }

        UpdateMusicButtonText();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        PieceSpawner spawner = FindFirstObjectByType<PieceSpawner>();

        if (spawner != null)
        {
            spawner.StartGame();
        }
    }

    public void RegisterHeight(float worldHeight)
    {

        Debug.Log(
            "REGISTER HEIGHT: world=" +
            worldHeight.ToString("F2") +
            " relative=" +
            (worldHeight - baseTopY).ToString("F2")
        );
        float relativeHeight = worldHeight - baseTopY;

        if (relativeHeight > highestHeight)
        {
            highestHeight = relativeHeight;
            PlayerPrefs.SetFloat(
                HighScoreKey,
                highestHeight
            );

            PlayerPrefs.Save();

            UpdateHighScoreUI();

            Debug.Log(
                "NEW HEIGHT: " +
                highestHeight.ToString("F2")
            );

            heightText.text = "HEIGHT: " + highestHeight.ToString("F2");

            CameraController cameraController =
                Camera.main.GetComponent<CameraController>();

            if (cameraController != null)
            {
                cameraController.SetTargetHeight(worldHeight);
            }
        }
    }

    public void ShowRestartButton()
    {
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        CameraController cameraController =
            Camera.main.GetComponent<CameraController>();

        if (cameraController != null)
        {
            cameraController.ShowGameOverView();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void UpdateCurrentHeight(float worldHeight)
    {
        currentHeight = Mathf.Max(
            0f,
            worldHeight - baseTopY
        );

        // Best height never decreases.
        if (currentHeight > highestHeight)
        {
            highestHeight = currentHeight;

            PlayerPrefs.SetFloat(
                HighScoreKey,
                highestHeight
            );

            PlayerPrefs.Save();

            UpdateHighScoreUI();

            Debug.Log(
                "NEW BEST HEIGHT: " +
                highestHeight.ToString("F2")
            );
        }

        if (heightText != null)
        {
            heightText.text =
                "HEIGHT: " +
                currentHeight.ToString("F2");
        }

        // Camera follows CURRENT tower height,
        // not the best height ever reached.
        CameraController cameraController =
            Camera.main.GetComponent<CameraController>();

        if (cameraController != null)
        {
            cameraController.SetTargetHeight(worldHeight);
        }
    }

    public void RecalculateCurrentHeight()
    {
        float highestWorldY = baseTopY;

        Piece[] pieces = FindObjectsByType<Piece>(
            FindObjectsSortMode.None
        );

        foreach (Piece piece in pieces)
        {
            PieceController controller =
                piece.GetComponent<PieceController>();

            if (controller == null)
                continue;
            /*
            if (!controller.IsSettled)
                continue;

            Rigidbody2D rb =
                piece.GetComponent<Rigidbody2D>();

            if (rb == null)
                continue;

            // Don't count a block that is currently falling or moving.
            if (rb.linearVelocity.magnitude > 0.2f ||
                Mathf.Abs(rb.angularVelocity) > 10f)
            {
                continue;
            }*/
            Rigidbody2D rb =
    piece.GetComponent<Rigidbody2D>();

            if (rb == null)
                continue;

            Collider2D collider =
                piece.GetComponent<Collider2D>();

            if (collider == null)
                continue;

            if (collider.bounds.max.y > highestWorldY)
            {
                highestWorldY = collider.bounds.max.y;
            }
        }

        UpdateCurrentHeight(highestWorldY);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private void UpdateMusicButtonText()
    {
        string text = isMuted ? "MUSIC: OFF" : "MUSIC: ON";

        if (startMusicButtonText != null)
        {
            startMusicButtonText.text = text;
        }

        if (gameMusicButtonText != null)
        {
            gameMusicButtonText.text = text;
        }
    }

    private void UpdateHighScoreUI()
    {
        string text =
            "HIGH SCORE: " +
            highestHeight.ToString("F2");

        if (startHighestScoreText != null)
        {
            startHighestScoreText.text = text;
        }

        if (gameHighestScoreText != null)
        {
            gameHighestScoreText.text = text;
        }
    }

    public void OpenDifficultyMenu()
    {
        if (difficultyCanvas != null)
        {
            difficultyCanvas.SetActive(true);
        }
    }

    public void SelectDifficulty(string difficulty)
    {
        Debug.Log("DIFFICULTY SELECTED: " + difficulty);

        selectedDifficulty = difficulty;

        if (difficultyText != null)
        {
            difficultyText.text = "DIFFICULTY: " + selectedDifficulty;
        }

        if (difficultyCanvas != null)
        {
            difficultyCanvas.SetActive(false);
        }
    }
}