using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace Code.MapGeneration.Handlers
{
    public class AllBlocksHandler : MonoBehaviour
    {
        #region Variables
        
        #region serializedHalls

        public List<GameObject> UHalls;
        public List<GameObject> RHalls;
        public List<GameObject> DHalls;
        public List<GameObject> LHalls;

        #endregion serializedHalls
        
        
        #region serializedRooms

        public List<GameObject> URooms;
        public List<GameObject> RRooms;
        public List<GameObject> DRooms;
        public List<GameObject> LRooms;

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

            //Invoke(nameof(CullExcess), 1.5f);
            Invoke(nameof(CreateEnemies),2f);
            
        }

        private void Awake()
        {
            InitiateBlocks();
        }

        private void CullExcess()
        {
            List<GameObject> allTiles = GameObject.FindGameObjectsWithTag("FloorTile").ToList();

            foreach (var tile in allTiles)
            {
                var position = tile.transform.position;
                if (position.x>30 || position.x<-30 || position.y>20 || position.y<-20)
                {
                    Destroy(tile);
                }
            }
        }
        
        private void InitiateBlocks()
        {
            foreach (string blockType in new []{"Halls","Rooms"})
            {
                foreach (string letter in new[] {"U", "R", "D", "L"})
                {
                    string path = $"Prefabs/Map/Blocks/{blockType}/{letter}";

                    List<GameObject> holder = new List<GameObject>();
                    
                    foreach (var g in Resources.LoadAll(path,typeof(GameObject)))
                    {
                        holder.Add(g as GameObject);
                    }
                    
                    typeof(AllBlocksHandler).GetField($"{letter}{blockType}").SetValue(this,holder);
                }
            }

            
            
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
                }
                else
                {
                    int randomEnemy = Random.Range(0, Enemies.Count);

                    GameObject enemy = Instantiate(Enemies[randomEnemy], tile.transform.position, Quaternion.identity);
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
                }
                else
                {
                    int randomItem = Random.Range(0, Items.Count);

                    GameObject item = Instantiate(Enemies[randomItem], tile.transform.position, Quaternion.identity);
                }  
            }
        }
    }

}
