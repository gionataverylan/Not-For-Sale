using UnityEngine;
using UnityEngine.UIElements;

public class UIWaveCounter : MonoBehaviour
{
    private UIDocument document;
    private EnemySpawner enemySpawner;
    private Label wave;
    private Label enemy;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
    }

    private void OnEnable()
    {
        wave = document.rootVisualElement.Q<Label>("WaveLabel");
        enemy = document.rootVisualElement.Q<Label>("EnemiesLabel");

        enemySpawner.WaveStarted += HandleChangeWave;
        enemySpawner.AliveEnemiesChanged += HandleChangeEnemy;
    }
    private void OnDisable()
    {
        enemySpawner.WaveStarted -= HandleChangeWave;
        enemySpawner.AliveEnemiesChanged -= HandleChangeEnemy;
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
