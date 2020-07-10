using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMeasure;

public class PointLine : MonoBehaviour {

    public Vector3[] mPoints = new Vector3[2];

    public float lineWidth = 0.1f;
    private LineRenderer lineObj;
    public bool isMoveing = false;

    public TextMesh textMesh;
    public Transform textObj;

    public GameObject placedPointObject;


    public float Length
    {
        get { return m_Length; }
        set { m_Length = value; }
    }

    float m_Length = 0.0f;

    // Use this for initialization
    void Start () {

        lineObj = GetComponent<LineRenderer>();
        lineObj.startWidth = lineWidth;
        lineObj.endWidth = lineWidth;

        textMesh.transform.localEulerAngles = new Vector3(0, 180, 0);
    }
	
	// Update is called once per frame
	void Update () {
		if(isMoveing)
        {
            lineObj.SetPosition(0, mPoints[0]);
            lineObj.SetPosition(1, mPoints[1]);
            updateTextMesh();
        }

    }

   
    public void StartMove()
    {
        textObj.gameObject.SetActive(true);
        updateTextMesh();
        isMoveing = true;
        GameObject tagSphere = Instantiate(placedPointObject); //GameObject.CreatePrimitive(PrimitiveType.Cube);
        tagSphere.name = "StartPoint";
        tagSphere.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        tagSphere.transform.position = mPoints[0];
        tagSphere.transform.parent = this.transform;
        //Material material = new Material(Shader.Find("UI/Default"));
        //material.color = Color.white;

        //tagSphere.GetComponent<Renderer>().material = material;
    }

    public void StopMove()
    {
        Vector3 tarVec = (mPoints[0] + mPoints[1]) / 2.0f;
        textObj.transform.position = tarVec;
        isMoveing = false;

        GameObject tagSphere = Instantiate(placedPointObject); //GameObject.CreatePrimitive(PrimitiveType.Cube);
        //tagSphere.name = "EndPoint";
        tagSphere.transform.localScale = new Vector3(0, 0, 0);
       // tagSphere.transform.position = mPoints[1];
       // tagSphere.transform.parent = this.transform;

        textMesh.GetComponent<TextMesh>().color = Color.white;
    }

    void updateTextMesh()
    {
        Vector3 tarVec = mPoints[1];
        float distInInches = Vector3.Distance(mPoints[0], mPoints[1]) ;
        distInInches = UnitConverter.convertToTargetUnit(distInInches);

        textObj.transform.position = tarVec;
        m_Length = distInInches;

        string s = System.String.Format("{0:0.00}", distInInches);
        textMesh.text = s + UnitConverter.unitString();
    }

    public void setPoint(int i, Vector3 pos)
    {
        mPoints[i] = pos;
    }

}
