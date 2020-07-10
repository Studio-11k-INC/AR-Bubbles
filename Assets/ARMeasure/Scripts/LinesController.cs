using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMeasure;
using System;

public class LinesController : measureLine {

    public GameObject linePrefab;
    public GameObject openeningPrefab;
    public GameObject objectPrefab;
    public GameObject suggestedWallPrefab;

    private bool placeWall = true;
    private bool placeOpening = false;
    private bool placeObject = false;

    public GameObject finishButton;

    public List<GameObject> sLinesList;
    public double proximity;

    GameObject mCurrentLineObj = null;

    public static LinesController instance;

    Vector3 firstCorner = new Vector3();
    public GameObject anchorPointController;

    int pointCount = 0;
	// Use this for initialization
	void Awake () {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {

		if (pointCount == 2)
        {
            finishButton.SetActive(true);
        }

    }

    /// <summary>
    /// Adds the point.
    /// </summary>
    /// <param name="pos">Position.</param>
    override  public void AddPoint(Vector3 pos)
    {

        if (pointCount == 0)
        {
            firstCorner = pos;
        }


        if (pointCount == 0 || Vector3.SqrMagnitude(pos - firstCorner) > proximity)
        {
            AddLine(pos);
            pointCount++;

            Debug.Log("SqrMagnitude = " + Vector3.SqrMagnitude(pos - firstCorner));
        }
        else
        {
            AddLine(firstCorner);
            LockCurrentLine();
            pointCount = 0;
        }

    }

    private void suggestedWall(Vector3 pos)
    {
        GameObject sw = Instantiate(suggestedWallPrefab, pos, Quaternion.identity) as GameObject;
       // goto.GetCompo


    }

    /// <summary>
    /// Add a line start with the pos .
    /// </summary>
    /// <param name="pos">Position.</param>
    override public void AddLine(Vector3 pos)
    {

        GameObject curr_Prefab = linePrefab;

        if (placeOpening)
        {
            curr_Prefab = openeningPrefab;
        }
        else if (placeObject)
        {
            curr_Prefab = objectPrefab;
        }

        GameObject go = Instantiate(curr_Prefab, pos, Quaternion.identity) as GameObject;

        sLinesList.Add(go);
        

        //LineRenderer lr = go.GetComponent<LineRenderer>();
        //lr.alignment = LineAlignment.TransformZ;

        go.GetComponent<PointLine>().setPoint(0,pos);
        mCurrentLineObj = go;
       

        mCurrentLineObj.GetComponent<PointLine>().StartMove();
    }

    override public void placingOpening()
    {
        placeWall = false;
        placeObject = false;
        placeOpening = true;
    }
    override public void placingWall()
    {
        placeWall = true;
        placeObject = false;
        placeOpening = false;
    }
    public void placingObject()
    {
        placeWall = false;
        placeObject = true;
        placeOpening = false;
    }

    /// <summary>
    /// Updates the length of the line.
    /// </summary>
    /// <param name="pos">Position.</param>
    override  public void updateLine(Vector3 pos)
    {
        if(mCurrentLineObj != null)
        {
            mCurrentLineObj.GetComponent<PointLine>().setPoint(1, pos);
        }
    }

    /// <summary>
    /// Locks the current line.
    /// </summary>
    override public void LockCurrentLine()
    {
        if(mCurrentLineObj != null)
        {
            mCurrentLineObj.GetComponent<PointLine>().StopMove();
            mCurrentLineObj = null;
        }
        pointCount = 0;
        //Debug.Log("current point is count is " + pointCount);
    }
    public void finishWall()
    {
        mCurrentLineObj = null;
        pointCount = 0;
    }

    /// <summary>
    /// Removes the objects.
    /// </summary>
    override public void RemoveObjs()
    {
        foreach(GameObject go in sLinesList)
        {
            Destroy(go);
        }
        sLinesList.Clear();
        pointCount = 0;
    }


    /// <summary>
    /// Deletes the last object.
    /// </summary>
    public override void deleteLastObj()
    {
        if(sLinesList.Count>0)
        {
            Destroy(sLinesList[sLinesList.Count - 1]);
        }
        else
        {
            return;
        }
        sLinesList.RemoveAt(sLinesList.Count - 1);

        pointCount = 0;
    }

}
