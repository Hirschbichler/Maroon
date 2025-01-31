﻿using UnityEngine;
using static MazeElement;

public class PathFindingStep
{
    private bool complete;

    // Tells the logic if this was the final step and no more are to be triggered
    public bool Complete
    {
        get { return complete; }
        set { complete = value; }
    }

    private float nextStepDelay;

    // Tells the logic how many ms it should wait until triggering the next step
    public float NextStepDelay
    {
        get { return nextStepDelay; }
        set { nextStepDelay = value; }
    }

    private MazeElementType[,] mazeElements;

    public MazeElementType[,] Layout
    {
        get { return mazeElements; }
        set { mazeElements = value; }
    }

    private string[,] mazeInfos;

    public string[,] MazeInfos
    {
        get { return mazeInfos; }
        set { mazeInfos = value; }
    }

    private Vector2Int[,] parents;

    public Vector2Int[,] Parents
    {
        get { return parents; }
        set { parents = value; }
    }

    private int stepID;

    // Field the PF algo can assign to build an internal state machnine
    public int StepID
    {
        get { return stepID; }
        set { stepID = value; }
    }

    private int pseudoCodeLine = -1;

    public int PseudoCodeLine
    {
        get { return pseudoCodeLine; }
        set { pseudoCodeLine = value; }
    }


    public PathFindingStep()
    { }

    public PathFindingStep(PathFindingStep other)
    {
        Layout = (MazeElement.MazeElementType[,])other.Layout.Clone();
        MazeInfos = (string[,])other.MazeInfos.Clone();
        parents = (Vector2Int[,])other.parents.Clone();
    }
}
