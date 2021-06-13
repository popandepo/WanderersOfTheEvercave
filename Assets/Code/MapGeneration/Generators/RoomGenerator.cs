using Code.MapGeneration.Handlers;
using UnityEngine;

namespace Code.MapGeneration.Generators
{
    public class RoomGenerator : MonoBehaviour
    {
        private Collider2D _isColliding;

        public int Direction = 0; //u0 r1 d2 l3
        public int CreateHall;

        public void SpawnCheck()
        {
            _isColliding = Physics2D.OverlapCircle((Vector2) transform.position, 0.2f);

            if (_isColliding)
            {
                Destroy(this.gameObject);
                return;
            }

            AllBlocksHandler handler =
                GameObject.FindGameObjectWithTag("GameController").GetComponent<AllBlocksHandler>();

            if (handler.RoomSize <= 0)
            {
                Destroy(this.gameObject);
                return;
            }

            GameObject block = null;
            
            if (CreateHall == 0)
            {
                int spawnRandom;
                switch (Direction)
                {
                    case 0:
                        spawnRandom = Random.Range(0, handler.URooms.Count);
                        block = Instantiate(handler.URooms[spawnRandom], transform.position, Quaternion.identity);
                        break;
                    
                    case 1:
                        spawnRandom = Random.Range(0, handler.RRooms.Count);
                        block = Instantiate(handler.RRooms[spawnRandom], transform.position, Quaternion.identity);
                        break;
                    
                    case 2:
                        spawnRandom = Random.Range(0, handler.DRooms.Count);
                        block = Instantiate(handler.DRooms[spawnRandom], transform.position, Quaternion.identity);
                        break;
                    
                    case 3:
                        spawnRandom = Random.Range(0, handler.LRooms.Count);
                        block = Instantiate(handler.LRooms[spawnRandom], transform.position, Quaternion.identity);
                        break;
                }
            }
            else
            {
                int spawnRandom;
                switch (Direction)
                {
                    case 0:
                        spawnRandom = Random.Range(0, handler.UHalls.Count);
                        block = Instantiate(handler.UHalls[spawnRandom], transform.position, Quaternion.identity);
                        break;
                    
                    case 1:
                        spawnRandom = Random.Range(0, handler.RHalls.Count);
                        block = Instantiate(handler.RHalls[spawnRandom], transform.position, Quaternion.identity);
                        break;
                    
                    case 2:
                        spawnRandom = Random.Range(0, handler.DHalls.Count);
                        block = Instantiate(handler.DHalls[spawnRandom], transform.position, Quaternion.identity);
                        break;
                    
                    case 3:
                        spawnRandom = Random.Range(0, handler.LHalls.Count);
                        block = Instantiate(handler.LHalls[spawnRandom], transform.position, Quaternion.identity);
                        break;
                }
            }

            handler.RoomSize -= 1;
            Destroy(this.gameObject);
        }
    }
}
