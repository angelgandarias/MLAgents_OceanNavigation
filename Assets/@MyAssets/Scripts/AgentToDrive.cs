using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class AgentToDrive : Agent {

    [SerializeField] private ShipController shipController;
    [SerializeField] private ShipSpawner spawner;

    //Called each time it has timed-out or has reached the goal
    public override void OnEpisodeBegin()    {

        //ResetCar();

        //_checkpointManager.ResetCheckpoints();
        ResetPosition();

    }


    

    // Lógica para recibir acciones del agente
    public override void OnActionReceived(ActionBuffers actionBuffers)    {

        // Move the agent using the action.
        MoveAgent(actionBuffers.DiscreteActions);

        //funcion de recompensa

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

    public override void Heuristic(in ActionBuffers actionsOut){

        
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
