using System.Collections.Generic;
using System.Linq;
using Code.MapGeneration.Generators;
using UnityEngine;

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

            Invoke(nameof(StartGeneration), 0.05f);
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

            Destroy(gameObject);
        }
    }
}