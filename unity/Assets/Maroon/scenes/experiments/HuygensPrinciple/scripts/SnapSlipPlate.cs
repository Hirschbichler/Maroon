﻿using UnityEngine;
// using VRTK;

public class SnapSlipPlate : MonoBehaviour, IResetObject
{
    [SerializeField]
    private WaterPlane waterPlane;

    [SerializeField]
    private Rigidbody plateHandleBodyRight;

    [SerializeField]
    private Rigidbody plateHandleBodyLeft;

    [SerializeField]
    private Vector3 plateScale = Vector3.one;

    // private VRTK_InteractableObject snappedPlate;
    private GameObject snappedPlate;

    private Vector3 previousPlateScale;
            
    private RigidbodyConstraints previousPlateBodyConstraints;

    public GameObject SnappedPlateObject => snappedPlate.gameObject;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("SlitPlate") || snappedPlate)
            return;

        var plate = other.gameObject;
        // var plate = other.GetComponent<VRTK_InteractableObject>();
        if (!plate) // || plate.IsGrabbed())
            return;

        snappedPlate = plate;
        StorePreviousState();

        Transform trans;
        (trans = snappedPlate.transform).position = Vector3.Lerp(plateHandleBodyRight.transform.position, plateHandleBodyLeft.transform.position, 0.5f);
        trans.position = new Vector3(trans.position.x, trans.position.y - 0.3f, trans.position.z);
        trans.rotation = Quaternion.Euler(0, 90, 0);
        trans.localScale = plateScale;

        // Create Handle Joints
        var handleJointRight = snappedPlate.gameObject.AddComponent<FixedJoint>();
        handleJointRight.connectedBody = plateHandleBodyRight;

        var handleJointLeft = snappedPlate.gameObject.AddComponent<FixedJoint>();
        handleJointLeft.connectedBody = plateHandleBodyLeft;

        snappedPlate.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        // snappedPlate.InteractableObjectGrabbed += OnSnappedPlateGrabbed;
    }

    // private void OnSnappedPlateGrabbed(object sender, InteractableObjectEventArgs e)
    // {
        // UnplugPlate();
    // }

    private void RegisterPlateWaveGenerators()
    {
        // Register all plate wave generators
        var generators = snappedPlate.GetComponentsInChildren<WaveGenerator>();
        foreach (var generator in generators)
            waterPlane.RegisterWaveGenerator(generator);
    }

    private void UnregisterPlateWaveGenerators()
    {
        // Unregister all plate wave generators
        var generators = snappedPlate.GetComponentsInChildren<WaveGenerator>();
        foreach (var generator in generators)
            waterPlane.UnregisterWaveGenerator(generator);
    }

    private void StorePreviousState()
    {
        previousPlateScale = snappedPlate.transform.localScale;
        previousPlateBodyConstraints = snappedPlate.GetComponent<Rigidbody>().constraints;
    }

    private void LoadPreviousState()
    {
        snappedPlate.transform.localScale = previousPlateScale;
        snappedPlate.GetComponent<Rigidbody>().constraints = previousPlateBodyConstraints;
    }

    public void UnplugPlate()
    {
        // Destroy all current joints
        foreach (var joint in snappedPlate.GetComponents<Joint>())
            Destroy(joint);

        UnregisterPlateWaveGenerators();
        LoadPreviousState();

        // snappedPlate.InteractableObjectGrabbed -= OnSnappedPlateGrabbed;
        snappedPlate = null;
    }

    public void ResetObject()
    {
        if (!snappedPlate)
            return;

        UnregisterPlateWaveGenerators();
        LoadPreviousState();

        // snappedPlate.InteractableObjectGrabbed -= OnSnappedPlateGrabbed;
        snappedPlate = null;
    }
}
