using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.MapGeneration.Handlers
{
    public class AllBlocksHandler : MonoBehaviour
    {
        public List<GameObject> UHalls;
        public List<GameObject> RHalls;
        public List<GameObject> DHalls;
        public List<GameObject> LHalls;

        public List<GameObject> URooms;
        public List<GameObject> RRooms;
        public List<GameObject> DRooms;
        public List<GameObject> LRooms;

        public List<GameObject> FloorTileBlocks;
        public List<GameObject> WallTileBlocks;
        public List<GameObject> HalfObstacleTileBlocks;
        public List<GameObject> FullObstacleTileBlocks;

        public LayerMask ActorLayer;

        public List<GameObject> Enemies;
        public List<GameObject> Items;

        public int RoomSize;

        public int MaxEnemies = 12;
        public int MinEnemies = 3;

        public int MaxItems = 5;
        public int MinItems;

        private int _enemiesToPlace;
        private int _itemsToPlace;

        private void Awake()
        {
            InitiateBlocks();
        }

        private void Start()
        {
            _enemiesToPlace = Random.Range(MinEnemies, MaxEnemies);
            _itemsToPlace = Random.Range(MinItems, MaxItems);

            //Invoke(nameof(CullExcess), 1.5f);
            Invoke(nameof(CreateEnemies), 2f);
        }

        private void CullExcess()
        {
            List<GameObject> allTiles = GameObject.FindGameObjectsWithTag("FloorTile").ToList();

            foreach (GameObject tile in allTiles)
            {
                Vector3 position = tile.transform.position;
                if (position.x > 30 || position.x < -30 || position.y > 20 || position.y < -20) Destroy(tile);
            }
        }

        private void InitiateBlocks()
        {
            foreach (string blockType in new[] {"Halls", "Rooms"})
            foreach (string letter in new[] {"U", "R", "D", "L"})
            {
                string path = $"Prefabs/Map/Blocks/{blockType}/{letter}";

                List<GameObject> holder = new List<GameObject>();

                foreach (Object g in Resources.LoadAll(path, typeof(GameObject))) holder.Add(g as GameObject);

                typeof(AllBlocksHandler).GetField($"{letter}{blockType}").SetValue(this, holder);
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