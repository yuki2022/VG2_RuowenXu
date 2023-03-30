using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS
{
    public class Enemy : MonoBehaviour
    {
        //Outlets
        NavMeshAgent navAgent;
        Animator animator;

        //Configuraton
        public Transform target;
        public Transform patrolRoute;
        public Transform priorityTarget;

        //State Tracking
        int patrolIndex;
        public int chaseDistance;

        //Methods
        void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (patrolRoute)
            {
                //Which patrol point is active?
                target = patrolRoute.GetChild(patrolIndex);

                //How far is the patrol point?
                float distance = Vector3.Distance(transform.position, target.position);
                print("Distance: " + distance);//Debug

                //Target the next point when we are close enough
                if (distance <= 1.75f) {
                    patrolIndex++;
                    if (patrolIndex >= patrolRoute.childCount) {
                        patrolIndex = 0;
                    }
                }
            }

            if (priorityTarget) {
                //keep track of our priorty target
                float priorityTargetDistance = Vector3.Distance(transform.position, priorityTarget.position);

                //if the priority target gets too close, follow it and highlight ourselves
                if (priorityTargetDistance <= chaseDistance) {
                    target = priorityTarget;
                    //GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    //GetComponent<Renderer>().material.color = Color.white;
                }
            }

            if (target) {
                navAgent.SetDestination(target.position);
            }
            animator.SetFloat("velocity", navAgent.velocity.magnitude);
        }
    }
}