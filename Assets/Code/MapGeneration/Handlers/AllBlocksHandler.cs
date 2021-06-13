using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Code.MapGeneration.Handlers
{
    public class AllBlocksHandler : MonoBehaviour
    {
        #region Variables
        
        #region serializedHalls

        public List<GameObject> LHalls;
        public List<GameObject> RHalls;
        public List<GameObject> UHalls;
        public List<GameObject> DHalls;

        #endregion serializedHalls
        
        
        #region serializedRooms

        public List<GameObject> LRooms;
        public List<GameObject> RRooms;
        public List<GameObject> URooms;
        public List<GameObject> DRooms;

        #endregion serializedRooms
        
        
        #region serializedBlocks

        public List<GameObject> FloorTileBlocks;
        public List<GameObject> WallTileBlocks;
        public List<GameObject> WallTileLargeBlocks;

        #endregion serializedBlocks

        
        #region genericVariables

        public LayerMask ActorLayer;

        public List<GameObject> Enemies;
        public List<GameObject> Items;

        public int RoomSize;
        
        public int MaxEnemies = 12;
        public int MinEnemies = 3;
        private int _enemiesToPlace;

        public int MaxItems = 5;
        public int MinItems = 0;
        private int _itemsToPlace;
        
        #endregion genericVariables
        
        #endregion Variables
        private void Start()
        {
            _enemiesToPlace = Random.Range(MinEnemies, MaxEnemies);
            _itemsToPlace = Random.Range(MinItems, MaxItems);
            
            Invoke(nameof(CreateEnemies),2f);
        }
        
        private void CreateEnemies()
        {
            List<GameObject> tiles = GameObject.FindGameObjectsWithTag("FloorTile").ToList();

            for (int i = 0; i < _enemiesToPlace; i++)
            {
                //get random tile
                GameObject tile = tiles[Random.Range(0, tiles.Count)];
                
                //check if tile is open
                Collider2D isOccupied = Physics2D.OverlapCircle(tile.transform.position, 0.1f, ActorLayer);
                if (isOccupied)
                {
                    Debug.Log($"Tile occupied (Called from {this.gameObject.name})");
                }
                else
                {
                    int randomEnemy = Random.Range(0, Enemies.Count);

                    GameObject enemy = Instantiate(Enemies[randomEnemy], tile.transform.position, Quaternion.identity);
                    
                    Debug.Log($"Placed \"{enemy.name}\" at {tile.transform.position}. (Called from {this.gameObject.name})");
                }  
            }
            CreateItems();
        }
        private void CreateItems()
        {
            List<GameObject> tiles = GameObject.FindGameObjectsWithTag("FloorTile").ToList();

            for (int i = 0; i < _itemsToPlace; i++)
            {
                //get random tile
                GameObject tile = tiles[Random.Range(0, tiles.Count)];
                
                //check if tile is open
                Collider2D isOccupied = Physics2D.OverlapCircle(tile.transform.position, 0.1f, ActorLayer);
                if (isOccupied)
                {
                    Debug.Log($"Tile occupied (Called from {this.gameObject.name})");
                }
                else
                {
                    int randomItem = Random.Range(0, Items.Count);

                    GameObject item = Instantiate(Enemies[randomItem], tile.transform.position, Quaternion.identity);
                    
                    Debug.Log($"Placed \"{item.name}\" at {tile.transform.position}. (Called from {this.gameObject.name})");
                }  
            }
        }
    }

}
