using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ObjectiveSpawner : MonoBehaviour
{

    [SerializeField]private GameObject objectiveA;
    [SerializeField]private GameObject objectiveB;
    [SerializeField]private GameObject objectiveC;



    public GameObject ChangeObjective()
    {
        int indiceRandom = Random.Range(0, 3);
        switch (indiceRandom)
        {
            
            case 0: 
                objectiveA.SetActive(true);
                objectiveB.SetActive(false);
                objectiveC.SetActive(false);
                return objectiveA;
            case 1:
                objectiveA.SetActive(false);
                objectiveB.SetActive(true);
                objectiveC.SetActive(false);
                return objectiveB;
            case 2:
                objectiveA.SetActive(false);
                objectiveB.SetActive(false);
                objectiveC.SetActive(true);
                return objectiveC;
            default:
            return null;



        }
    }
}
