using System;
using Code.MapGeneration.Handlers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.MapGeneration.Generators
{
    public class TileGenerator : MonoBehaviour
    {
        public enum Tile
        {
            Floor,
            Wall,
            HalfObstacle,
            FullObstacle
        }

        public Tile TileType; //0 is floor, 1 is wall

        private Collider2D _collidedObject;

        public void SpawnCheck()
        {
            bool hitUp = Physics2D.OverlapCircle(transform.position, 0.1f);
            if (hitUp)
            {
                Destroy(gameObject);
                return;
            }

            AllBlocksHandler handler =
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AllBlocksHandler>();

            int spawnRandom;
            GameObject block;
            switch (TileType)
            {
                case Tile.Floor:
                    spawnRandom = Random.Range(0, handler.FloorTileBlocks.Count);
                    block = Instantiate(handler.FloorTileBlocks[spawnRandom], transform.position,
                        Quaternion.identity);
                    break;

                case Tile.Wall:
                    spawnRandom = Random.Range(0, handler.WallTileBlocks.Count);
                    block = Instantiate(handler.WallTileBlocks[spawnRandom], transform.position,
                        Quaternion.identity);
                    break;

                case Tile.HalfObstacle:
                    spawnRandom = Random.Range(0, handler.HalfObstacleTileBlocks.Count);
                    block = Instantiate(handler.HalfObstacleTileBlocks[spawnRandom], transform.position,
                        Quaternion.identity);
                    break;

                case Tile.FullObstacle:
                    spawnRandom = Random.Range(0, handler.FullObstacleTileBlocks.Count);
                    block = Instantiate(handler.FullObstacleTileBlocks[spawnRandom], transform.position,
                        Quaternion.identity);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Destroy(gameObject);
        }
    }
}