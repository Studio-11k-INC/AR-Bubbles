using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using ARMeasure;

public class AnchorPoint : MonoBehaviour {

    bool isActive = false;
    Vector3 screenCenter = new Vector3();

    public delegate void TrackPlane();
    public event TrackPlane trackPlaneEvent;

    public ARSessionOrigin m_SessionOrigin;
    public ARPlaneManager m_PlaneMgr;
    public ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public GameObject post;

    public Material anchorPost_mtl;
    public Material openPost_mtl;
    public Material objectPost_mtl;

    private bool placingObject = false;


    // Use this for initialization
    void Start()
    { 
        screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));//screenCenter.Set(Screen.width / 2, Screen.height / 2,0);
   //    m_SessionOrigin = GameObject.FindObjectOfType<ARSessionOrigin>();
        //m_PlaneMgr = FindObjectOfType<ARPlaneManager>();
    //    m_RaycastManager = GameObject.FindObjectOfType<ARRaycastManager>();
  //      m_PlaneMgr.planeAdded += AddAnchor;
        m_PlaneMgr.planesChanged += AddAnchor;
      
    }

    void AddAnchor(ARPlanesChangedEventArgs args)
    {
        if(trackPlaneEvent != null)
        {
            trackPlaneEvent();
        }

        isActive = true;

        //clearPlanes();


    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            //     bool siresult = m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon | TrackableType.PlaneWithinBounds | TrackableType.PlaneEstimated);

            /*
            if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                measureLine.gameObject.SetActive(true);
                endPoint.SetActive(true);

                Pose hitPose = hits[0].pose;
                endPoint.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            }
            */
  
            if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinInfinity))// USE TrackableType.All for Android and TrackableType.PlaneWithinInfinity for iOS
            {
            
                Pose hitPose = s_Hits[s_Hits.Count-1].pose;
                if (placingObject)
                {
                    hitPose = s_Hits[0].pose;
                }

                this.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);

                if (ARMeasureManager.instance.mMeasureMode == MeasureMode.MeasureAngle ||
                    ARMeasureManager.instance.mMeasureMode == MeasureMode.MeasureLength)
                {
                    ARMeasureManager.instance.updateState(transform.position);
                }
            }
        }
    }

    public Vector3 AddPoint()
    {
        clearPlanes();

        Vector3 pos = this.transform.position;
      
        
        ARMeasureManager.instance.AddPoint(pos);

        return pos;
    }

    public void clearPlanes()
    {
        //m_PlaneMgr.
        m_PlaneMgr.enabled = false;
        GameObject lowestPlane = null;
        float floorY = 100000;


        foreach (var plane in m_PlaneMgr.trackables)
        {

            if (plane.gameObject.transform.position.y < floorY)
            {
                floorY = plane.gameObject.transform.position.y;
                lowestPlane = plane.gameObject;
            }

            plane.gameObject.SetActive(false);

        }

        if (lowestPlane != null)
        {
            lowestPlane.gameObject.SetActive(true);
        }

    }

    public void anchorIsCorner()
    {
        Renderer rend = post.GetComponent<Renderer>();
        rend.material = anchorPost_mtl;
        placingObject = false;

    }

    public void anchorIsOpening()
    {
        Renderer rend = post.GetComponent<Renderer>();
        rend.material = openPost_mtl;
        placingObject = false;

    }

    public void anchorIsObject()
    {
        Renderer rend = post.GetComponent<Renderer>();
        rend.material = objectPost_mtl;
        m_PlaneMgr.enabled = true;
        placingObject = true;

        foreach (var plane in m_PlaneMgr.trackables)
        {
            plane.gameObject.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("LDCStartMenu");
    }

}
