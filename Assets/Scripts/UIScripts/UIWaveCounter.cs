using UnityEngine;
using UnityEngine.UIElements;

public class UIWaveCounter : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    private Label wave;
    private Label enemy;
    private UIDocument document;

    private void Awake()
    {
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
        document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        wave = document.rootVisualElement.Q<Label>("WaveLabel");
        enemy = document.rootVisualElement.Q<Label>("EnemiesLabel");

        Debug.Log(wave);

        enemySpawner.onWaveStarted += HandleChangeWave;
        enemySpawner.onEnemyStarted += HandleChangeEnemy;
    }
    private void OnDisable()
    {
        enemySpawner.onWaveStarted -= HandleChangeWave;
        enemySpawner.onEnemyStarted -= HandleChangeEnemy;
    }

    private void HandleChangeWave(int count)
    {
        wave.text = count.ToString();
    }

    private void HandleChangeEnemy(int count)
    {
        enemy.text = count.ToString();
    }
}
