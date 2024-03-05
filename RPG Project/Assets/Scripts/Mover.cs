using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    // [SerializeField] Transform target;
    private NavMeshAgent navMeshAgent;
    public Camera mainCamera;
    public float moveSpeed = 5f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }
        // if (target != null && navMeshAgent != null)
        // {
        //     navMeshAgent.SetDestination(target.position);
        //     // Vector3 direction = target.position - transform.position;

        //     // if (direction.magnitude > 0.1f)
        //     // {
        //     //     transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
        //     // }
        // }
    }

    private void MoveToCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            navMeshAgent.SetDestination(hit.point);
        }
    }
}
