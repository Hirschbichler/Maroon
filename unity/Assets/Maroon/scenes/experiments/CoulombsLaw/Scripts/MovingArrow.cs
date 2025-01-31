﻿using UnityEngine;

public class MovingArrow : MonoBehaviour
{
    private PC_ArrowMovement _arrowMovement;
    
    private void Start()
    {
        _arrowMovement = transform.parent.GetComponent<PC_ArrowMovement>();
        Debug.Assert(_arrowMovement != null);
    }

    private void OnMouseDown()
    {
        _arrowMovement.OnChildMouseDown(gameObject);
    }

    private void OnMouseDrag()
    {
        _arrowMovement.OnChildMouseDrag();
    }

    private void OnMouseUp()
    {
        _arrowMovement.OnChildMouseUp();
    }
}
