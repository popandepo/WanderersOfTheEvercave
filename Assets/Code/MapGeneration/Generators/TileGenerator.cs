using Code.MapGeneration.Handlers;
using UnityEngine;

namespace Code.MapGeneration.Generators
{
    public class TileGenerator : MonoBehaviour
    {
        public int Type; //0 is floor, 1 is wall

        private Collider2D _collidedObject;

        public void SpawnCheck()
        {
            bool hitUp = Physics2D.OverlapCircle((Vector2) transform.position, 0.1f);
            if (hitUp)
            {
                Destroy(this.gameObject);
                return;
            }

            AllBlocksHandler handler =
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AllBlocksHandler>();

            int spawnRandom;
            GameObject block;
            switch (Type)
            {
                case 0:
                    spawnRandom = Random.Range(0, handler.FloorTileBlocks.Count);
                    block = Instantiate(handler.FloorTileBlocks[spawnRandom], transform.position,
                        Quaternion.identity);
                    break;
                
                case 1:
                    spawnRandom = Random.Range(0, handler.WallTileBlocks.Count);
                    block = Instantiate(handler.WallTileBlocks[spawnRandom], transform.position,
                        Quaternion.identity);
                    break;
            }
            
            Destroy(this.gameObject);
        }
    }
}
