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

    private void ResetShipPosition()
    {
        ship.transform.position = originalSpawn.position;
        //El barco tiene mal los ejes...
        ship.transform.rotation = Quaternion.Euler(0, 0, 180);
    }
}
