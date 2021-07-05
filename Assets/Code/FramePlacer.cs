using System.Collections.Generic;
using System.Linq;
using Code.AStarCode;
using UnityEngine;

namespace Code
{
    public class FramePlacer : MonoBehaviour
    {
        public GameObject Frame;

        private void Update()
        {
            List<Collider2D> colliders =
                Physics2D.OverlapCircleAll(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f).ToList();
            
            Collider2D? floor = colliders.FirstOrDefault(c => c.gameObject.name.StartsWith("FloorTile"));
            bool frame = colliders.FirstOrDefault(c => c.gameObject.name.StartsWith("TileFrame"));

            if (frame) return;
            if (floor)
            {
                Frame.transform.position = floor.transform.position;
                Frame.GetComponent<InitiateAStar>().Active = true;
            }
            else Frame.GetComponent<InitiateAStar>().Active = false;
        }

        private void Start()
        {
            string path = "Prefabs/Map/Visuals/Frames";
            GameObject holder = null;

            foreach (Object g in Resources.LoadAll(path, typeof(GameObject))) holder = g as GameObject;
            typeof(FramePlacer).GetField("Frame").SetValue(this, holder);

            Frame = Instantiate(Frame, transform.position, Quaternion.identity);
        }
    }
}