using System.Collections.Generic;
using UnityEngine;

namespace Code.AStarCode
{
    public class InitiateAStar : MonoBehaviour
    {
        public GameObject AStarHandler;
        public GameObject Player;
        public List<GameObject> Path;
    
        void Start()
        {
            AStarHandler = GameObject.Find("AStarHandlerObj");
            Player = GameObject.Find("Player(Clone)");
        
            Path = AStarHandler.GetComponent<AStarHandler>().FindPath(Player.transform.position,transform.position);

            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("FloorTile"))
            {
                obj.GetComponent<SpriteRenderer>().color = Color.green;
            }
            foreach (GameObject obj in Path)
            {
                obj.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }
}
