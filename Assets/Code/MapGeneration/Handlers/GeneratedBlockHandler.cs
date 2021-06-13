using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.MapGeneration.Generators;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.MapGeneration.Handlers
{
    public class GeneratedBlockHandler : MonoBehaviour
    {
        public List<TileGenerator> TilesToGenerate;
        public List<RoomGenerator> RoomsToGenerate;

        private void Start()
        {
            TilesToGenerate = GetComponentsInChildren<TileGenerator>().ToList();
            RoomsToGenerate = GetComponentsInChildren<RoomGenerator>().ToList();
            
            Invoke(nameof(StartGeneration),Random.Range(0.1f,0.3f));
        }

        private void StartGeneration()
        {
            foreach (TileGenerator tile in TilesToGenerate)
            {
                tile.SpawnCheck();
                tile.transform.parent = null;
            }

            foreach (RoomGenerator room in RoomsToGenerate)
            {
                room.SpawnCheck();
                room.transform.parent = null;
            }
            
            Destroy(this.gameObject);
        }
    }
}
