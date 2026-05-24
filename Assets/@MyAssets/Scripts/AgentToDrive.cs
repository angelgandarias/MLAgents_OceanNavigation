using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class AgentToDrive : Agent
{
    [SerializeField] private ShipController shipController;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private ShipSpawner spawner;
    [SerializeField] private GameObject Punto1;
    [SerializeField] private GameObject Punto2;
    [SerializeField] private GameObject Punto3;
    [SerializeField] private GameObject Punto4;
    [SerializeField] private GameObject Punto5;
    [SerializeField] private GameObject Punto6;
    [SerializeField] private GameObject Punto7;
    [SerializeField] private GameObject Punto8;
    [SerializeField] private GameObject objetivo;
    
    private GameObject PuntoFinal;
    private float previousDistance;

    public override void OnEpisodeBegin()
    {
        PuntoFinal = objetivo.GetComponent<ObjectiveSpawner>().ChangeObjective();

        ResetPosition();

        previousDistance = Vector3.Distance(shipController.transform.position, PuntoFinal.transform.position);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 1. LOCAL DIRECTION TO GOAL
        // This tells the agent exactly where the goal is relative to the way it is currently facing.
        Vector3 localDirectionToGoal = shipController.transform.InverseTransformPoint(PuntoFinal.transform.position);
        sensor.AddObservation(localDirectionToGoal.normalized); // Size: 3

        // 2. DISTANCE TO GOAL
        float currentDistance = Vector3.Distance(shipController.transform.position, PuntoFinal.transform.position);
        sensor.AddObservation(currentDistance); // Size: 1

        // 3. LOCAL VELOCITY
        // Tells the agent if it's moving forward, sliding sideways, or reversing relative to itself
        Vector3 localVelocity = shipController.transform.InverseTransformDirection(shipController.GetComponent<Rigidbody>().linearVelocity);
        sensor.AddObservation(localVelocity); // Size: 3

        // 4. WAVES
        sensor.AddObservation(waveManager.WaveHeight(Punto1.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto2.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto3.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto4.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto5.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto6.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto7.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto8.transform.position)); // Size: 8
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
        CheckCapsized();
        Rewards();

        // Existential penalty to force it to hurry up and not drive in circles
        if (MaxStep > 0)
        {
            AddReward(-1f / MaxStep);
        }
    }

    public void Rewards()
    {
        float currentDistance = Vector3.Distance(shipController.transform.position, PuntoFinal.transform.position);

        // DELTA DISTANCE REWARD
        // If distanceDelta is positive, the ship moved closer. If negative, it moved away.
        float distanceDelta = previousDistance - currentDistance;
        AddReward(distanceDelta * 1.0f); 

        // Update for the next frame
        previousDistance = currentDistance;

        // ARRIVAL BONUS
        if (currentDistance < 5f)
        {
            AddReward(10f);
            Debug.Log("¡Objetivo alcanzado! Episodio "+CompletedEpisodes+ ". El objetivo alcanzado ha sido "+ PuntoFinal.gameObject.name);
            EndEpisode();
        }
    }

    public void MoveAgent(ActionSegment<int> vectorAction)
    {
        int direction = (int)vectorAction[0]; 
        if (direction == 0) shipController.TurnLeft();
        else if (direction == 1) shipController.TurnRight();
        else if (direction == 2) shipController.ResetSteeringAngle();

        int shipForward = (int)vectorAction[1]; 
        if (shipForward == 0) shipController.GoForward();
        else if (shipForward == 1) shipController.GoReverse();
        else if (shipForward == 2) shipController.Brakes();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W)) actions[1] = 0;
        else if (Input.GetKey(KeyCode.S)) actions[1] = 1;
        else actions[1] = 2;

        if (Input.GetKey(KeyCode.A)) actions[0] = 0;
        else if (Input.GetKey(KeyCode.D)) actions[0] = 1;
        else actions[0] = 2;
    }

    public void ResetPosition()
    {
        spawner.ResetShipPosition();
    }
    void CheckCapsized()
{
    //Si el eje Y del barco apunta hacia abajo (y con cierto ángulo), está volcado
    if (shipController.transform.up.y > 0.4f)
    {
        
        AddReward(-10f);
        Debug.Log("¡Barco volcado! Reiniciando episodio.");
        EndEpisode();
        
    }
    Debug.Log(shipController.transform.up.y);
}
}