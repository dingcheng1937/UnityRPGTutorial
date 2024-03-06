using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        public Vector3 offset;
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                transform.position = target.position + offset;

            }
        }
    }
}
