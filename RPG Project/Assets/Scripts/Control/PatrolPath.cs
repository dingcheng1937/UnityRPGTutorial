using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        void Start()
        {

        }
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Vector3 thisPos = GetWaypoint(i);
                Vector3 nextPos = transform.GetChild(GetNextIndex(i)).position;

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(thisPos, 1);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(thisPos, nextPos);

            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            if (i == transform.childCount - 1)
            {
                // nextPos = transform.GetChild(0).position;
                return 0;

            }

            return i + 1;
        }
    }
}
