using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinimapHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _cornerPoint;
    [SerializeField]
    private GameObject _openPoint;
    [SerializeField]
    private GameObject _cornerLine;
    [SerializeField]
    private GameObject _openLine;
    [SerializeField]
    private GameObject _objectLine;
    [SerializeField]
    private GameObject _objectPoint;
    private Vector3 _pointSpawn = new Vector3();
    private Vector3 _objectPointSpawn = new Vector3();
    private Vector3 _previousPointSpawn;
    private Vector3 _previousObjectPointSpawn;
    private Vector3 _startingPoint = new Vector3(0f, 0.0f, 0f);
    private Vector3 _differenceBetweenPoints;
    private Vector3 _maxPoint;
    private Vector3 _minPoint;
    [SerializeField]
    private GameObject _miniMapCamera;
    private float tempx;
    private float tempz;
    private float tempSlope;
    private List<GameObject> pointList;
    private List<GameObject> lineList;
    private List<GameObject> objectLineList;
    private float distanceBetweenPoints;

    public void Start()
    {
        _differenceBetweenPoints = new Vector3();
        _previousPointSpawn = new Vector3();
        _previousObjectPointSpawn = new Vector3();
        _minPoint = new Vector3();
        _maxPoint = new Vector3();
        pointList = new List<GameObject>();
        lineList = new List<GameObject>();
        objectLineList = new List<GameObject>();
    }
    public void AddFirstPoint(string pointType, Vector3 pointLocation)
    {
        _differenceBetweenPoints = pointLocation - _startingPoint;
        _differenceBetweenPoints.y = 0f;
        if (pointType == "Corner")
        {
            //GameObject point = Instantiate(_cornerPoint, _startingPoint,Quaternion.identity);
            //pointList.Add(point);
        }
        else if (pointType == "Opening")
        {
            //GameObject point = Instantiate(_openPoint, _startingPoint, Quaternion.identity);
            //pointList.Add(point);
        }
        _previousPointSpawn = _startingPoint;
    }
    public void AddFirstObject(Vector3 pointLocation)
    {
        _objectPointSpawn = pointLocation - _differenceBetweenPoints;
        _objectPointSpawn.y = 0f;
        _previousObjectPointSpawn = _objectPointSpawn;
    }
    public void AddObject(Vector3 pointLocation)
    {
        _objectPointSpawn = pointLocation - _differenceBetweenPoints;
        _objectPointSpawn.y = 0f;
        AddObjectLine(_previousObjectPointSpawn, _objectPointSpawn);
        _previousObjectPointSpawn = _objectPointSpawn;
    }
    public void AddObjectLine(Vector3 previousPoint, Vector3 CurrentPoint)
    {
        GameObject line = Instantiate(_objectLine);
        line.GetComponent<LineRenderer>().SetPosition(0, previousPoint);
        line.GetComponent<LineRenderer>().SetPosition(1, CurrentPoint);

        float lineWidth = .008f * distanceBetweenPoints;
        line.GetComponent<LineRenderer>().startWidth = lineWidth;
        line.GetComponent<LineRenderer>().endWidth = lineWidth;

        objectLineList.Add(line);
    }
    public void AddPoint(string pointType, Vector3 pointLocation, int pointCounter)
    {
        _pointSpawn = pointLocation - _differenceBetweenPoints;
        _pointSpawn.y = 0f;

        if (_pointSpawn.x > _maxPoint.x)
        {
            _maxPoint.x = _pointSpawn.x;
        }
        else if (_pointSpawn.x < _minPoint.x)
        {
            _minPoint.x = _pointSpawn.x;
        }
        if (_pointSpawn.z > _maxPoint.z)
        {
            _maxPoint.z = _pointSpawn.z;
        }
        else if (_pointSpawn.z < _minPoint.z)
        {
            _minPoint.z = _pointSpawn.z;
        }
        distanceBetweenPoints = Vector3.Distance(_minPoint, _maxPoint);
        if (pointCounter == 1)
        {
            tempx = (_startingPoint.x + _pointSpawn.x) / 2f;
            tempz = (_startingPoint.z + _pointSpawn.z) / 2f;
            _miniMapCamera.transform.position = new Vector3(tempx, 50f, tempz);
            tempSlope = ((_pointSpawn.z - _startingPoint.z) / (_pointSpawn.x - _startingPoint.x));
            double rotationAngleRad = System.Math.Atan(tempSlope);
            float rotationAngle = (float)((180 / System.Math.PI) * rotationAngleRad);
            _miniMapCamera.transform.eulerAngles = new Vector3(90f, -rotationAngle, 0);
            
        }
        else
        {
            tempx = (_minPoint.x+_maxPoint.x) / 2f;
            tempz = (_minPoint.z + _maxPoint.z) / 2f;
            _miniMapCamera.transform.position = new Vector3(tempx, 50f, tempz);
        }

        if (pointType == "Corner")
        {
            //GameObject point = Instantiate(_cornerPoint, _pointSpawn, Quaternion.identity);
            //pointList.Add(point);
            if (pointCounter > 0)
            {
                AddCornerLine(_previousPointSpawn, _pointSpawn);
            }
        }
        else if (pointType == "Opening")
        {
            //GameObject point = Instantiate(_openPoint, _pointSpawn, Quaternion.identity);
            //pointList.Add(point);
            if (pointCounter > 0)
            {
                AddCornerLine(_previousPointSpawn, _pointSpawn);
            }
        }
        else if (pointType == "Closing")
        {
            //GameObject point = Instantiate(_openPoint, _pointSpawn, Quaternion.identity);
            //pointList.Add(point);
            AddOpeningLine(_previousPointSpawn, _pointSpawn);
        }

        _miniMapCamera.GetComponent<Camera>().orthographicSize = distanceBetweenPoints * .6f; //1.1.f

        float lineWidth = .008f * distanceBetweenPoints;

        foreach(var line in lineList)
        {
            line.GetComponent<LineRenderer>().startWidth =lineWidth;
            line.GetComponent<LineRenderer>().endWidth = lineWidth;
        }
        _previousPointSpawn = _pointSpawn;
    }
    public void AddCornerLine(Vector3 previousPoint,Vector3 CurrentPoint)
    {
        GameObject line = Instantiate(_cornerLine);

        line.GetComponent<LineRenderer>().SetPosition(0, previousPoint);
        line.GetComponent<LineRenderer>().SetPosition(1, CurrentPoint);

        bool finished_space = false; // NEED TO ADD FUNCTIONALITY TO CHECK PROXIMITY AND PREVIOUSLY DRAWN LINES!

       /* foreach (var corner in lineList)
        {
            if (Vector3.SqrMagnitude(corner.GetComponent<LineRenderer>().GetPosition(0)-CurrentPoint)<proximity)
            {
                finished_space = true;
            }
        }*/

        if (!finished_space)
        {
            lineList.Add(line);
        }


    }
    public void AddOpeningLine(Vector3 previousPoint, Vector3 CurrentPoint)
    {
        GameObject line =  Instantiate(_openLine);
        line.GetComponent<LineRenderer>().SetPosition(0, previousPoint);
        line.GetComponent<LineRenderer>().SetPosition(1, CurrentPoint);
        lineList.Add(line);
    }
    public void deleteLines()
    {
        int numLines = lineList.Count;
        for(int i = numLines -1 ; i >= 0 ; i--) 
        {
            GameObject todelete = lineList[i].gameObject;
            GameObject.Destroy(todelete);
            lineList.RemoveAt(i);
        }
        int numObjects = objectLineList.Count;
        for (int i = numObjects - 1; i >= 0 ; i--)
        {
            GameObject todelete = objectLineList[i].gameObject;
            GameObject.Destroy(todelete);
            objectLineList.RemoveAt(i);
        }
    }
    public void undoWall()
    {
        int lastLine = lineList.Count - 1;
        if (lastLine >= 0) { 
            GameObject todelete = lineList[lastLine].gameObject;
            GameObject.Destroy(todelete);
            lineList.RemoveAt(lastLine);
        }
    }
    public void undoObject()
    {
        int lastObject = objectLineList.Count - 1;
        if (lastObject >= 0)
        {  
            GameObject todelete = objectLineList[lastObject].gameObject;
            GameObject.Destroy(todelete);
            objectLineList.RemoveAt(lastObject);
        }
    }
    public void ResetMapCameraAngle()
    {
        _miniMapCamera.transform.eulerAngles = new Vector3(90f, 0f, 0f);
    }
}
