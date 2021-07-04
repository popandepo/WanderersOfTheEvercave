using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.AStarCode
{
    public class AStarHandler : MonoBehaviour
    {
        public List<GameObject> OpenNodes;
        public List<GameObject> ClosedNodes;
        public List<GameObject> Path;
        public Vector3 Start;
        public Vector3 End;

        /// <summary>
        /// Creates a path from Player to selected tile using the A* algorithm
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>A List of GameObjects that trace the path to the end</returns>
        public List<GameObject> FindPath(Vector3 start, Vector3 end)
        {
            ResetValues();

            Start = start;
            End = end;

            OpenNodes.Add(FindStartPoint());

            return CalcPath();
        }

        /// <summary>
        /// Resets all the values needed
        /// </summary>
        private void ResetValues()
        {
            OpenNodes.Clear();
            ClosedNodes.Clear();
            Path.Clear();
        }

        /// <summary>
        /// Finds the starting GameObject by casting an OverlapCircle
        /// </summary>
        /// <returns>The GameObject of type FloorTile that the calculations should originate from</returns>
        private GameObject FindStartPoint()
        {
            Collider2D[] overlap = Physics2D.OverlapCircleAll(Start, 0.1f);
            GameObject output = overlap.First(g => g.gameObject.name.StartsWith("FloorTile")).gameObject;
            return output;
        }

        /// <summary>
        /// Looks for all the neighbours of the current tile
        /// </summary>
        /// <param name="current"></param>
        /// <returns>A list of GameObjects that contain all the neighbours</returns>
        private List<GameObject> FindNeighbors(Vector3 current)
        {
            List<GameObject> neighbors = new List<GameObject>();

            Collider2D[]
                colliders = Physics2D.OverlapCircleAll(current,
                    0.5f); //Change radius back to 1 to reintroduce diagonals

            foreach (Collider2D coll in colliders)
                if (coll.gameObject.name.StartsWith("FloorTile"))
                    neighbors.Add(coll.gameObject);

            return neighbors;
        }

        /// <summary>
        /// The megaclass where all the math happens
        /// </summary>
        /// <returns></returns>
        private List<GameObject> CalcPath()
        {
            //While there are nodes to check
            while (OpenNodes.Count > 0)
            {
                int lowestTotalScoreIndex = 0;

                //Sets the index to the entry that has the lowest total Score
                for (int i = 0; i < OpenNodes.Count; i++)
                {
                    if (OpenNodes[i].GetComponent<AStar>().TotalScore <
                        OpenNodes[lowestTotalScoreIndex].GetComponent<AStar>().TotalScore)
                        lowestTotalScoreIndex = i;
                }
                
                //Sets the current object to the one that has the lowest total score
                GameObject current = OpenNodes[lowestTotalScoreIndex];
                AStar currentScores = current.GetComponent<AStar>();

                //Clears the current node from the list of open nodes and adds it to the list of closed nodes
                OpenNodes.Remove(current);
                ClosedNodes.Add(current);

                //Loop through all the neighbors
                List<GameObject> neighbors = FindNeighbors(current.transform.position);
                foreach (GameObject neighbor in neighbors)
                {
                    //Continue to the next loop if the neighbor has been closed
                    if (ClosedNodes.Contains(neighbor)) continue;
                    
                    //Assigning a long call to a shorter variable for ease of use 
                    AStar neighborScores = neighbor.GetComponent<AStar>();
                    
                    //Set the lowest path score to be the current path score
                    float lowestPathScore = currentScores.PathScore;

                    //If the neighbor already exists in the list of open nodes
                    if (OpenNodes.Contains(neighbor))
                    {
                        //If the neighbor's PathScore is higher this time
                        if (lowestPathScore < neighborScores.PathScore)
                        {
                            //Updates the neighbor to have a lower PathScore
                            neighborScores.PathScore = lowestPathScore;
                        }
                    }
                    else
                    {
                        //Set the PathScore and add the neighbor to the list of open nodes
                        neighborScores.PathScore = lowestPathScore;
                        OpenNodes.Add(neighbor);
                    }

                    //Sets the neighbor's HeuristicScore to be the distance from the neighbor to the end
                    neighborScores.HeuristicScore = Heuristic(neighbor, End);
                    
                    //Sets the neighbor's TotalScore to be the PathScore and the HeuristicScore added together
                    neighborScores.TotalScore = neighborScores.PathScore + neighborScores.HeuristicScore;
                    
                    neighborScores.CameFrom = current;
                }

                //If this position is not the end, try again 
                if (current.transform.position != End) continue;
                
                //If a complete path has been found, return it
                Path = FindWholePath(current);
                return Path;
            }

            Debug.Log("NO PATH!");
            return null;
        }

        /// <summary>
        /// Traces a line of GameObjects from the start to the end
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private static List<GameObject> FindWholePath(GameObject current)
        {
            List<GameObject> path = new List<GameObject>();
            GameObject temp = current;

            while (temp.gameObject.GetComponent<AStar>().CameFrom)
            {
                path.Add(temp);
                temp = temp.gameObject.GetComponent<AStar>().CameFrom;
            }
            
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Gets the heuristic value of a neighbor
        ///
        /// A heuristic value is the length of a straight line from the game object to the end point
        /// </summary>
        /// <param name="neighbor"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private float Heuristic(GameObject neighbor, Vector3 end)
        {
            Vector3 neighborPoint = neighbor.transform.position;

            return Vector3.Distance(neighborPoint, end);
        }
    }
}