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
    [SerializeField] private GameObject PuntoFinal;
    private float distanceToGoal;
    private float distanceMinimum;

    //Called each time it has timed-out or has reached the goal
    public override void OnEpisodeBegin()
    {

        PuntoFinal = objetivo.GetComponent<ObjectiveSpawner>().ChangeObjective();
       
        

        //_checkpointManager.ResetCheckpoints();
        ResetPosition();
        distanceToGoal = Vector3.Distance(shipController.transform.position, PuntoFinal.transform.position);
        distanceMinimum = distanceToGoal;

    }

    public void Rewards()
{
    float distanceToGoal = Vector3.Distance(shipController.transform.position, PuntoFinal.transform.position);

    // Usamos un offset de 1.0f para evitar divisiones por cero 
    // y para que la recompensa máxima no tienda al infinito.
    float rewardValue = 1.0f / (distanceToGoal + 1.0f); 
    
    // Aplicamos la recompensa en cada paso (Continuous Reward)
    AddReward(rewardValue * 0.1f); 

    // Bonus por llegada
    if (distanceToGoal < 5f)
    {
        AddReward(100f);
        EndEpisode();
    }
}


    // Lógica para recibir acciones del agente
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        // Move the agent using the action.
        MoveAgent(actionBuffers.DiscreteActions);

        //funcion de recompensa
        Rewards();

    }



    public void MoveAgent(ActionSegment<int> vectorAction)
    {


        int direction = (int)vectorAction[0]; //La rama 0 ( es decir, la primera de las ramas)

        if (direction == 0)
        {
            shipController.TurnLeft();
        }
        else if (direction == 1)
        {
            shipController.TurnRight();
        }
        else if (direction == 2)
        {
            shipController.ResetSteeringAngle();
        }




        int shipForward = (int)vectorAction[1]; //La rama 1 ( es decir, la segunda de las ramas)
        if (shipForward == 0)
        {
            shipController.GoForward();
        }
        else if (shipForward == 1)
        {
            shipController.GoReverse();
        }
        else if (shipForward == 2)
        {
            shipController.Brakes();
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {

        sensor.AddObservation(shipController.transform.position);
        sensor.AddObservation(shipController.transform.rotation);
        sensor.AddObservation(PuntoFinal.transform.position);
        sensor.AddObservation(shipController.GetComponent<Rigidbody>().linearVelocity);
        sensor.AddObservation(waveManager.WaveHeight(Punto1.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto2.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto3.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto4.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto5.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto6.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto7.transform.position));
        sensor.AddObservation(waveManager.WaveHeight(Punto8.transform.position));

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {


        var actions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        {
            actions[1] = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[1] = 1;
        }
        if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
        {
            actions[1] = 2;
        }


        if (Input.GetKey(KeyCode.A))
        {
            actions[0] = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[0] = 1;
        }

        if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
        {
            actions[0] = 2;
        }

    }

    public void ResetPosition()
    {
        spawner.ResetShipPosition();
    }

}