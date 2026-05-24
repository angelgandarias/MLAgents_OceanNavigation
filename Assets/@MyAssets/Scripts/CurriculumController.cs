using UnityEngine;
using Unity.MLAgents;

public class CurriculumController : MonoBehaviour
{
    [SerializeField] private WeatherController weatherController;
    
    private EnvironmentParameters envParams;

    void Start()
    {
        envParams = Academy.Instance.EnvironmentParameters;
    }

    void Update()
    {
        // Lee el valor que manda el trainer. Si no hay trainer (modo Heuristic),
        // usa el valor que tengas puesto en el Inspector como fallback.
        float stormValue = envParams.GetWithDefault("storm_intensity", 
                           weatherController.stormIntensity);
        
        weatherController.stormIntensity = stormValue;
    }
}