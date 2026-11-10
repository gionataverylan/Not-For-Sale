using UnityEngine;
using UnityEngine.UIElements;

public class UIWaveCounter : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    private UIDocument document;
    private Label wave;
    private Label enemy;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (enemySpawner == null)
        {
            Debug.LogError("UIWaveCounter: не назначен enemySpawner", this);
            return;
        }

        wave = document.rootVisualElement.Q<Label>("WaveLabel");
        enemy = document.rootVisualElement.Q<Label>("EnemiesLabel");

        enemySpawner.WaveStarted += HandleWaveStarted;
        enemySpawner.AliveEnemiesChanged += HandleAliveEnemiesChanged;
    }
    private void OnDisable()
    {
        if (enemySpawner != null)
        {
            enemySpawner.WaveStarted -= HandleWaveStarted;
            enemySpawner.AliveEnemiesChanged -= HandleAliveEnemiesChanged;
        }
    }

    private void HandleWaveStarted(int count)
    {
        wave.text = count.ToString();
    }

    private void HandleAliveEnemiesChanged(int count)
    {
        enemy.text = count.ToString();
    }
}
