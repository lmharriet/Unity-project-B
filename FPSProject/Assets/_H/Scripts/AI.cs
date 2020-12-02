﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField]
    Transform Target;

    private void Awake()
    {
        agent =GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(Target.transform.position);
    }
}
