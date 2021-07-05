using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.AStarCode
{
    public class InitiateAStar : MonoBehaviour
    {
        public bool Active = false;
        public GameObject AStarHandler;
        public GameObject Player;
        public GameObject Frame;
        public List<GameObject> Path;

        private void Start()
        {
            AStarHandler = GameObject.Find("AStarHandlerObj");
            Frame = GameObject.Find("TileFrame(Clone)");
        }
        
        private void Update()
        {
            Player = GameObject.Find("Player(Clone)");
            
            gameObject.GetComponent<SpriteRenderer>().color = Active ? Color.black : Color.white;

            if (Active)
            {
                var ptf = Player.transform.position;
                var ftf = Frame.transform.position;
                Path = AStarHandler.GetComponent<AStarHandler>().FindPath(ptf,ftf);

                foreach (GameObject node in GameObject.FindGameObjectsWithTag("FloorTile"))
                {
                    node.GetComponent<SpriteRenderer>().color = Color.white;
                }
                foreach (GameObject node in Path)
                {
                    node.GetComponent<SpriteRenderer>().color = DistanceIsValid(Player, node) ? Color.green : Color.red;
                }
            }
            
            if (Input.GetMouseButtonUp(0) && Active)
            {
                List<Collider2D> CollidedObjects = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f).ToList();
                int frames = CollidedObjects.Count(c => c.gameObject.name.StartsWith("TileFrame"));
                if (frames == 1)
                {
                    if (Path.All(p => DistanceIsValid(Player, p)))
                    {
                        Player.GetComponent<Player>().SetPath(Path);
                        Active = false;
                    }
                    
                }
            }
        }

        private bool DistanceIsValid(GameObject player, GameObject node)
        {
            float distance = Vector3.Distance(player.transform.position, node.transform.position);

            return distance <= Player.GetComponent<Player>().MaxMovementDistance;
        }
    }
}