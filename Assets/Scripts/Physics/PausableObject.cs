﻿using UnityEngine;

public abstract class PausableObject : MonoBehaviour
{
    private Vector3 CurrentVelocity;

    private bool IsPause = false;

    private SimulationController simController;


    protected virtual void Start()
    {
        GameObject simControllerObject = GameObject.Find("SimulationController");
        if (simControllerObject)
            simController = simControllerObject.GetComponent<SimulationController>();
    }

	private void Update()
    {
        Rigidbody rigidbody_ = GetComponent<Rigidbody>();


        if(simController.SimulationRunning)
        {
            if(IsPause)
            {
                IsPause = false;
                rigidbody_.isKinematic = false;
                rigidbody_.velocity = CurrentVelocity;
            }

            HandleUpdate();
        }
        else if(!IsPause)
        {
            IsPause = true;
            CurrentVelocity = rigidbody_.velocity;
            rigidbody_.isKinematic = true;
        }
	}

    private void FixedUpdate()
    {
        if (simController.SimulationRunning)
            HandleFixedUpdate();
    }

    protected abstract void HandleUpdate();

    protected abstract void HandleFixedUpdate();

}
