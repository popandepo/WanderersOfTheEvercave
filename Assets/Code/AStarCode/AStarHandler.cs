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
        ///     Creates a path from Player to selected tile using the A* algorithm
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
        ///     Resets all the values needed
        /// </summary>
        private void ResetValues()
        {
            OpenNodes.Clear();
            ClosedNodes.Clear();
            Path.Clear();
        }

        /// <summary>
        ///     Finds the starting GameObject by casting an OverlapCircle
        /// </summary>
        /// <returns>The GameObject of type FloorTile that the calculations should originate from</returns>
        private GameObject FindStartPoint()
        {
            Collider2D[] overlap = Physics2D.OverlapCircleAll(Start, 0.1f);
            GameObject output = overlap.First(g => g.gameObject.name.StartsWith("FloorTile")).gameObject;
            return output;
        }

        /// <summary>
        ///     Looks for all the neighbours of the current tile
        /// </summary>
        /// <param name="current"></param>
        /// <returns>A list of GameObjects that contain all the neighbours</returns>
        private List<GameObject> FindNeighbors(Vector3 current)
        {
            List<GameObject> neighbors = new List<GameObject>();

            Collider2D[]
                colliders = Physics2D.OverlapCircleAll(current,
                    1f); //Change radius back to 1f to allow diagonals or 0.5f to not

            foreach (Collider2D coll in colliders)
                if (coll.gameObject.name.StartsWith("FloorTile"))
                    neighbors.Add(coll.gameObject);

            return neighbors;
        }

        /// <summary>
        ///     The megaclass where all the math happens
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
                    if (OpenNodes[i].GetComponent<AStar>().TotalScore <
                        OpenNodes[lowestTotalScoreIndex].GetComponent<AStar>().TotalScore)
                        lowestTotalScoreIndex = i;

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

                    if (IsDiagonal(current, neighbor))
                    {
                        if (!CanMoveDiagonally(current, neighbor))
                            continue;
                    }

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
                            if (IsDiagonal(current, neighbor))
                            {
                                neighborScores.PathScore += 0.4142f;
                            }
                        }
                    }
                    else
                    {
                        //Set the PathScore and add the neighbor to the list of open nodes
                        neighborScores.PathScore = lowestPathScore;
                        if (IsDiagonal(current, neighbor))
                        {
                            neighborScores.PathScore += 0.4142f;
                        }
                        
                        OpenNodes.Add(neighbor);
                    }

                    //Sets the neighbor's HeuristicScore to be the distance from the neighbor to the end
                    neighborScores.HeuristicScore = Heuristic(neighbor, End);
                    
                    if (neighborScores.HeuristicScore > currentScores.HeuristicScore)
                    {
                        neighborScores.PathScore += Heuristic(current, neighbor.transform.position);
                    }

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
        ///     Checks if it's possible to move diagonally
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private bool CanMoveDiagonally(GameObject start, GameObject end)
        {
            float sX = start.transform.position.x;
            float sY = start.transform.position.y;
            float eX = end.transform.position.x;
            float eY = end.transform.position.y;


            GameObject top = sY > eY ? start : end;
            GameObject right = sX > eX ? start : end;
            GameObject bottom = sY < eY ? start : end;
            GameObject left = sX < eX ? start : end;

            GameObject? topRight = top == right ? top : null;
            GameObject? topLeft = top == left ? top : null;
            GameObject? bottomRight = bottom == right ? bottom : null;
            GameObject? bottomLeft = bottom == left ? bottom : null;

            try
            {
                if (topRight == null && bottomLeft == null)
                {
                    Vector3 newPos1 = topLeft.transform.position;
                    newPos1.x += 1;
                    Collider2D obj1 = Physics2D.OverlapCircle(newPos1, 0.1f);
                    bool isFloor1 = obj1.gameObject.name.StartsWith("FloorTile");

                    Vector3 newPos2 = bottomRight.transform.position;
                    newPos2.x -= 1;
                    Collider2D obj2 = Physics2D.OverlapCircle(newPos2, 0.1f);
                    bool isFloor2 = obj2.gameObject.name.StartsWith("FloorTile");

                    if (isFloor1 && isFloor2) return true;
                }

                if (topLeft == null && bottomRight == null)
                {
                    Vector3 newPos1 = topRight.transform.position;
                    newPos1.x -= 1;
                    Collider2D obj1 = Physics2D.OverlapCircle(newPos1, 0.1f);
                    bool isFloor1 = obj1.gameObject.name.StartsWith("FloorTile");

                    Vector3 newPos2 = bottomLeft.transform.position;
                    newPos2.x += 1;
                    Collider2D obj2 = Physics2D.OverlapCircle(newPos2, 0.1f);
                    bool isFloor2 = obj2.gameObject.name.StartsWith("FloorTile");

                    if (isFloor1 && isFloor2) return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <summary>
        ///     Checks if two values are diagonal
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private bool IsDiagonal(GameObject start, GameObject end)
        {
            float sX = start.transform.position.x;
            float sY = start.transform.position.y;

            float eX = end.transform.position.x;
            float eY = end.transform.position.y;

            if (sX == eX || sY == eY) return false;
            return true;
        }

        /// <summary>
        ///     Traces a line of GameObjects from the start to the end
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private static List<GameObject> FindWholePath(GameObject current)
        {
            List<GameObject> path = new List<GameObject>();
            GameObject temp = current;

            while (temp.gameObject.GetComponent<AStar>().CameFrom)
            {
                if (GameObject.Find("TileFrame(Clone)").GetComponent<InitiateAStar>().Active)
                { 
                    path.Add(temp); 
                    temp = temp.gameObject.GetComponent<AStar>().CameFrom;
                }
                else
                {
                    return null;
                }
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        ///     Gets the heuristic value of a neighbor
        ///     A heuristic value is the length of a straight line from the game object to the end point
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