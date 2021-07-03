using System;
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
        public int Height;
        public int Width;

        private void Start()
        {
            TilesToGenerate = GetComponentsInChildren<TileGenerator>().ToList();
            RoomsToGenerate = GetComponentsInChildren<RoomGenerator>().ToList();

            Invoke(nameof(StartGeneration), 0.05f);
        }

        private void OnValidate()
        {
            foreach (Transform node in transform)
            {
                Vector3 position = node.position;

                int nodeHeight = (int) Math.Abs(position.y);
                int nodeWidth = (int) Math.Abs(position.x);

                if (Height < nodeHeight) Height = nodeHeight;

                if (Width < nodeWidth) Width = nodeWidth;
            }
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