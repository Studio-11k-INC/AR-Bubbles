using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMeasure;

public class HeightController : measureLine {


    //check the height by using this hitpanelobj. locat at ARCamera/HitPanel
    public HitPanel hitPanelObj;

    public GameObject linePerfab;
    public List<GameObject> sLinesList;

    GameObject mCurrentLineObj = null;

    public static HeightController instance;

    int pointCount = 0;

    bool isWorking = false;
    // Use this for initialization
    void Start()
    {
        instance = this;

 //       hitPanelObj = GameObject.FindObjectOfType<HitPanel>();

        if(hitPanelObj != null)
        {
            Debug.LogError(" Can't find the HitPanel , please check if exist under the ARCamera Obj !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isWorking)
        {
            updateLine(hitPanelObj.hitPos);
        }
    }

    override public void AddPoint(Vector3 pos)
    {
        if (pointCount % 2 == 0)
        {
            AddLine(pos);
            pointCount++; 
        }
        else
        {
            isWorking = false;
            LockCurrentLine();
            pointCount = 0;
        }
       
    }

    override public void AddLine(Vector3 pos)
    {
        GameObject go = Instantiate(linePerfab, pos, Quaternion.identity) as GameObject;
        sLinesList.Add(go);

        go.GetComponent<PointLine>().setPoint(0, pos);
        mCurrentLineObj = go;

        mCurrentLineObj.GetComponent<PointLine>().StartMove();

        isWorking = true;

        hitPanelObj.setBottomPoint(pos);
        hitPanelObj.StartWork();
    }

    override public void updateLine(Vector3 pos)
    {
        if (mCurrentLineObj != null)
        {
            mCurrentLineObj.GetComponent<PointLine>().setPoint(1, pos);
        }
    }

    override public void LockCurrentLine()
    {
        hitPanelObj.StopWork();
        mCurrentLineObj.GetComponent<PointLine>().StopMove();
        mCurrentLineObj = null;
        pointCount = 0;
    }

    override public void RemoveObjs()
    {
        foreach (GameObject go in sLinesList)
        {
            Destroy(go);
        }
        hitPanelObj.StopWork();
        sLinesList.Clear();
        pointCount = 0;
    }

    public override void deleteLastObj()
    {
        if (sLinesList.Count > 0)
        {
            Destroy(sLinesList[sLinesList.Count - 1]);
        }
        else
        {
            return;
        }
        hitPanelObj.StopWork();
        sLinesList.RemoveAt(sLinesList.Count - 1);
        pointCount = 0;
    }

}
