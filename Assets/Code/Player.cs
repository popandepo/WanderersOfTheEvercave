using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Code
{
    public class Player : MonoBehaviour
    {
        public float MaxMovementDistance;
        public bool Counter;
        public bool Second;
        private float _elapsed = 0f;
        
        public void SetPath(List<GameObject> path)
        {
            Move(path);
        }
        

        private void Update()
        {
            if (Counter)
            {
                Count();
            }
        }

        private void Count()
        { 
            _elapsed += Time.deltaTime;
            if (_elapsed >= 0.5f) 
            {
                _elapsed %= 0.5f;
                Second = !Second;
            }
        }
        
        private void Move(List<GameObject> path)
        {
            Second = false;
            bool _oldSecond = false;
            Counter = true;

            int failedCount = 0;
            
            do
            {
                if (Second != _oldSecond || failedCount >= 10000000)
                {
                    failedCount = 0;
                    _oldSecond = Second;

                    transform.position = path[0].transform.position;
                    path.RemoveAt(0);
                }
                else
                {
                    failedCount += 1;
                }
            } while (path.Count > 0);

        }
    }
}