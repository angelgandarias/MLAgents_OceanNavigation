using System;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] GameObject ship;
    [SerializeField] Transform originalSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetShipPosition();
    }

    public void ResetShipPosition()
    {
        //El barco tiene mal los ejes...
        ship.transform.SetPositionAndRotation(originalSpawn.position, Quaternion.Euler(0, 0, 180));
    }
}
