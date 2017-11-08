﻿using UnityEngine;

public class VoltageSource : MonoBehaviour
{
    [SerializeField]
    private GameObject negativeVoltagePole;

    [SerializeField]
    private GameObject positiveVoltagePole;

    [SerializeField]
    private GameObject electronPrefab;

    private GameObject plusCable;

    private GameObject minusCable;

    private void Start()
    {
        plusCable = GameObject.Find("Cable+");
        minusCable = GameObject.Find("Cable-");
    }

    public void PullTrigger(Collider other, GameObject source)
    {
        float electronSpeed = other.GetComponent<PathFollower>().maxSpeed;
        Destroy(other.gameObject);

        GameObject electron = GameObject.Instantiate(electronPrefab);
        electron.GetComponent<Charge>().JustCreated = true;

        PathFollower pathFollower = electron.GetComponent<PathFollower>();
        pathFollower.maxSpeed = electronSpeed;

        if (source == positiveVoltagePole)
        {
            electron.transform.position = negativeVoltagePole.transform.position;

            pathFollower.reverseOrder = true;
            pathFollower.SetPath(minusCable.GetComponent<IPath>());

            //Physics.IgnoreCollision(electron.GetComponent<Collider>(), negativeVoltagePole.GetComponent<Collider>());
        }

        if(source == negativeVoltagePole)
        {
            electron.transform.position = positiveVoltagePole.transform.position;

            pathFollower.SetPath(minusCable.GetComponent<IPath>());

            //Physics.IgnoreCollision(electron.GetComponent<Collider>(), positiveVoltagePole.GetComponent<Collider>());
        }
    }
}
