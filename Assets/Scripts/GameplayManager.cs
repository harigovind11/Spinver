using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameplayManager : MonoBehaviour
{
    #region START
    public static GameplayManager Instance;

    private bool hasGameFinished;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        hasGameFinished = false;

        score = 0;
        _scoreText.text = score.ToString();

        scoreToSpawn = 1;
        totalTime = 10f;
        currentTime = totalTime;
        StartCoroutine(CountTime());

        SpawnCoin();

        GameManager.Instance.IsInitialized = true;
    }
    #endregion

    #region SCORE

    [SerializeField] private TMP_Text _scoreText;
    private int score;
    private int scoreToSpawn;
    private int remainingScore;

    [SerializeField] private GameObject _scorePrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemyParent; 

    private List<GameObject> _spawnedEnemies = new List<GameObject>();
 
    [SerializeField] private float _spawnX, _spawnY;

    private void SpawnCoin()
    {
        remainingScore = scoreToSpawn;

        for (int i = 0; i < remainingScore; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-_spawnX, _spawnX), Random.Range(-_spawnY, _spawnY), 0);

            RaycastHit2D hit = Physics2D.CircleCast(spawnPos, 1f, Vector2.zero);

            bool canSpawn = hit;

            while (canSpawn)
            {
                spawnPos = new Vector3(Random.Range(-_spawnX, _spawnX), Random.Range(-_spawnY, _spawnY), 0);
                hit = Physics2D.CircleCast(spawnPos, 1f, Vector2.zero);
                canSpawn = hit;
            }

            Instantiate(_scorePrefab, spawnPos, Quaternion.identity);
        }
    }
    private void SpawnEnemy()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogError("❌ _enemyPrefab not assigned!");
            return;
        }

        int enemyCount = scoreToSpawn; // Example: scale with level

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-_spawnX, _spawnX), Random.Range(-_spawnY, _spawnY), 0);

            RaycastHit2D hit = Physics2D.CircleCast(spawnPos, 1f, Vector2.zero);

            int attempts = 0;
            while (hit && attempts < 50)
            {
                spawnPos = new Vector3(Random.Range(-_spawnX, _spawnX), Random.Range(-_spawnY, _spawnY), 0);
                hit = Physics2D.CircleCast(spawnPos, 1f, Vector2.zero);
                attempts++;
            }

            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);

            if (_enemyParent != null)
                newEnemy.transform.SetParent(_enemyParent);

            _spawnedEnemies.Add(newEnemy);
        }
    }

    private void ResetEnemies()
    {
        foreach (GameObject enemy in _spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        _spawnedEnemies.Clear();
    }

    public void UpdateScore()
    {
        score++;
        _scoreText.text = score.ToString();

        remainingScore--;

        if(remainingScore == 0)
        {
            scoreToSpawn++;

            totalTime = scoreToSpawn * 5f;
            currentTime = totalTime;

            SpawnCoin();
            ResetEnemies();
            SpawnEnemy();

        }
    }

    [SerializeField] private Transform _timerImage;

    private float totalTime;
    private float currentTime;

    private IEnumerator CountTime()
    {
        while(!hasGameFinished)
        {
            currentTime -= Time.deltaTime;
            Vector3 temp = _timerImage.localScale;
            temp.x = currentTime / totalTime;
            _timerImage.localScale = temp;

            if(currentTime <= 0f)
            {
                GameEnded();
            }

            yield return null;
        }
    }

    #endregion

    #region GAME_OVER

    public UnityAction GameEnd;

    [SerializeField] private AudioClip _loseClip;

    public void GameEnded()
    {
        hasGameFinished = true;
        GameEnd?.Invoke();
        GameManager.Instance.CurrentScore = score;
        SoundManager.Instance.PlaySound(_loseClip);

        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GoToMainMenu();
    }

    #endregion
}
