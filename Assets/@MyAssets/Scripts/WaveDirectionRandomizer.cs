using UnityEngine;

public class WaveDirectionRandomizer : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;

    [Header("Rotación aleatoria")]
    [Tooltip("Ángulo máximo de rotación aleatoria en cada episodio (en grados)")]
    [SerializeField] private float maxRotationAngle = 180f;

    // Guardamos las direcciones originales del Inspector
    private Vector2[] originalDirections;

    private void Awake()
    {
        // Memorizamos las direcciones base que tienes configuradas en el Inspector
        originalDirections = new Vector2[waveManager.waves.Length];
        for (int i = 0; i < waveManager.waves.Length; i++)
        {
            originalDirections[i] = waveManager.waves[i].direction;
        }
    }

    // Llama a este método desde AgentToDrive al inicio de cada episodio
    public void RandomizeWaveDirections()
    {
        // Un único ángulo aleatorio para todas las olas
        float randomAngle = Random.Range(-maxRotationAngle, maxRotationAngle);
        float cos = Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float sin = Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        for (int i = 0; i < waveManager.waves.Length; i++)
        {
            Vector2 original = originalDirections[i];

            // Rotamos la dirección original por el ángulo aleatorio
            waveManager.waves[i].direction = new Vector2(
                original.x * cos - original.y * sin,
                original.x * sin + original.y * cos
            );
        }

        Debug.Log($"Direcciones de olas rotadas {randomAngle:F1}°");
    }
}