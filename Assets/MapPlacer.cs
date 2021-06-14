using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Code.MapGeneration.Handlers;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapPlacer : MonoBehaviour
{
    public List<GameObject> StartingRooms;
    public GameObject Player;
    
    [NonSerialized]
    public GameObject StartingRoom;
    [NonSerialized]
    public Vector3 StartingRoomPosition;
    private void Awake()
    {
        LoadStartingRooms();
        
        int rndx = Random.Range(-30, 30);
        int rndy = Random.Range(-20, 20);

        StartingRoom = Instantiate(StartingRooms[Random.Range(0, StartingRooms.Count)],new Vector3(rndx,rndy),Quaternion.identity);
        StartingRoomPosition = StartingRoom.transform.position;
        
        Invoke(nameof(InstantiatePlayer),0.1f);
    }

    private void InstantiatePlayer()
    {
        Player = Instantiate(Player,StartingRoomPosition,Quaternion.identity);
    }
    private void LoadStartingRooms()
    {
        string path = $"Prefabs/Map/Blocks/Rooms/Beginning";

        List<GameObject> holder = new List<GameObject>();
                    
        foreach (var g in Resources.LoadAll(path,typeof(GameObject)))
        {
            holder.Add(g as GameObject);
        }
                    
        typeof(MapPlacer).GetField("StartingRooms").SetValue(this,holder);
    }
}
