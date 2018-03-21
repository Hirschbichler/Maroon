﻿//-----------------------------------------------------------------------------
// FieldLineManager.cs
//
// Controller class to manage the field lines
//
//
// Authors: Michael Stefan Holly
//          Michael Schiller
//          Christopher Schinnerl
//-----------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller class to manage the field lines
/// </summary>
public abstract class FieldLineManager : MonoBehaviour
{
    /// <summary>
    /// Lists of field lines
    /// </summary>
    protected HashSet<FieldLine> fieldLines = new HashSet<FieldLine>();

    /// <summary>
    /// Initialization
    /// </summary>
    public void Start()
    {
        GameObject[] sensedObjects = GameObject.FindGameObjectsWithTag("FieldLine");

        foreach (GameObject sensedTag in sensedObjects)
        {
            GameObject parent = sensedTag.transform.parent.gameObject;
            AddFieldLine(parent.GetComponent<FieldLine>());
        }
    }

    /// <summary>
    /// Sets the field line visibility
    /// </summary>
    /// <param name="visibility">The visibility value</param>
    public void SetFieldLinesVisible(bool visibility)
    {
        foreach (FieldLine fL in fieldLines)
            fL.setVisibility(visibility);
    }

    /// <summary>
    /// Adds the given field line to list
    /// </summary>
    /// <param name="fL">The field line</param>
    public void AddFieldLine(FieldLine fL)
    {
        fieldLines.Add(fL);
    }

    /// <summary>
    /// Removes the given field line from list
    /// </summary>
    /// <param name="fL">The field line</param>
    public void RemoveFieldLine(FieldLine fL)
    {
        fieldLines.Remove(fL);
    }

    /// <summary>
    /// This function is called every fixed framerate frame and draw fild lines
    /// </summary>
    public void FixedUpdate()
    {
        DrawFieldLines();
    }

    protected abstract void DrawFieldLines();
}
