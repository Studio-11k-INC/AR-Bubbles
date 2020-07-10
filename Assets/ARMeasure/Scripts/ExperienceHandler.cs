using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _minimapHandler;
    [SerializeField]
    private GameObject _anchorPoint;
    [SerializeField]
    private GameObject _linesController;
    private int _wallPointCounter;
    private int _objectPointCounter;
    private Vector3 _pointLocation = new Vector3();
    [SerializeField]
    private GameObject _uiController;
    private List<string> lastCreated;
    [SerializeField]
    private RenderTexture _miniMapRender;
    private bool _displayImage = false;
    [SerializeField]
    private GameObject _minimapSprite;
    [SerializeField]
    private Sprite tempImage;
    
    public void Start()
    {
        _wallPointCounter = 0;
        lastCreated = new List<string>();
        
    }

    public void AddCorner()
    {
        _linesController.GetComponent<LinesController>().placingWall();
        _pointLocation = (_anchorPoint.GetComponent<AnchorPoint>().AddPoint());
        _anchorPoint.GetComponent<AnchorPoint>().anchorIsCorner();
        if (_wallPointCounter == 0)
        {
            _minimapHandler.GetComponent<MinimapHandler>().AddFirstPoint("Corner", _pointLocation);
           
        }
        else
        {
            _minimapHandler.GetComponent<MinimapHandler>().AddPoint("Corner", _pointLocation, _wallPointCounter);

        }
        lastCreated.Add("wallList");
        _wallPointCounter++;
       
    }
    public void AddOpening()
    {
        _linesController.GetComponent<LinesController>().placingOpening();
        _pointLocation = (_anchorPoint.GetComponent<AnchorPoint>().AddPoint());
        _anchorPoint.GetComponent<AnchorPoint>().anchorIsOpening();
        _linesController.GetComponent<LinesController>().placingOpening();
        if (_wallPointCounter == 0)
        {
            _minimapHandler.GetComponent<MinimapHandler>().AddFirstPoint("Opening", _pointLocation);
            
        } 
        else
        {
            _minimapHandler.GetComponent<MinimapHandler>().AddPoint("Opening", _pointLocation, _wallPointCounter);
 
        }
        lastCreated.Add("wallList");
        _wallPointCounter++;
        
    }
    public void FinishOpening()
    {
        _pointLocation = (_anchorPoint.GetComponent<AnchorPoint>().AddPoint());
        _anchorPoint.GetComponent<AnchorPoint>().anchorIsCorner();
        _linesController.GetComponent<LinesController>().placingWall();
        _minimapHandler.GetComponent<MinimapHandler>().AddPoint("Closing", _pointLocation, _wallPointCounter);
        lastCreated.Add("wallList");
        _wallPointCounter++;
        
    }
    public void CompleteWall()
    {

    }
    public void UndoLast()
    {
        _uiController.GetComponent<UIController>().deleteLastObj();
        int lastInList = lastCreated.Count - 1;
        if (lastInList >= 0) {
            string toUndo = lastCreated[lastInList];
            if (toUndo == "wallList")
            {
                _minimapHandler.GetComponent<MinimapHandler>().undoWall();
                if(_wallPointCounter == 2 )
                {
                    _minimapHandler.GetComponent<MinimapHandler>().ResetMapCameraAngle();
                }
                _wallPointCounter--;
            }
            else if (toUndo == "objectList")
            {
                _minimapHandler.GetComponent<MinimapHandler>().undoObject();
                _objectPointCounter--;
            }
            lastCreated.RemoveAt(lastInList);
        }
    }
    public void AddObject()
    {
        _linesController.GetComponent<LinesController>().placingObject();
        _pointLocation = (_anchorPoint.GetComponent<AnchorPoint>().AddPoint());
        _anchorPoint.GetComponent<AnchorPoint>().anchorIsObject();
        if (_objectPointCounter==0)
        {
            _minimapHandler.GetComponent<MinimapHandler>().AddFirstObject(_pointLocation);

        }
        else
        {
            _minimapHandler.GetComponent<MinimapHandler>().AddObject(_pointLocation);

        }
        lastCreated.Add("objectList");
        _objectPointCounter++;
        
    }
    public void DeleteEverything()
    {
        _wallPointCounter = 0;
        _objectPointCounter = 0;
        _uiController.GetComponent<UIController>().deleteAllObjs();
        _minimapHandler.GetComponent<MinimapHandler>().deleteLines();
    }
    public void CreatePNG()
    {
       

        if (_displayImage == false)
        {
        
        
            RenderTexture.active = _miniMapRender;
            Texture2D _textureForPNG = new Texture2D(_miniMapRender.width, _miniMapRender.height);
            _textureForPNG.ReadPixels(new Rect(0, 0, _miniMapRender.width, _miniMapRender.height), 0, 0);
            _textureForPNG.Apply();
            Sprite _forImage = Sprite.Create(_textureForPNG, new Rect(0, 0, _miniMapRender.width, _miniMapRender.height), new Vector2(0.5f,0.5f));
            _minimapSprite.SetActive(true);
            
            //byte[] bytes;
           // bytes = _textureForPNG.EncodeToPNG();
            //System.IO.File.WriteAllBytes(file Location for PNG, bytes);
            _minimapSprite.GetComponent<Image>().sprite = _forImage;
           _displayImage = true;
        }
        else
        {
            _minimapSprite.SetActive(false);
            _displayImage = false;
        }
        
    }
}
