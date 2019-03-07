using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class SpotController : MonoBehaviour, IPointerClickHandler, HoloToolkit.Unity.InputModule.IInputClickHandler
{

    public Material material;
    public int frame;
    public VideoPlayer videoPlayer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        Camera.main.transform.parent.position = new Vector3(this.transform.position.x, Camera.main.transform.parent.position.y, this.transform.position.z);

        videoPlayer.frame = frame;
        videoPlayer.Pause();

        for (int i = 0; i < AllSpots.Count; ++i)
        {
            AllSpots[i].gameObject.SetActive(NeighborIndexes.Contains(i));
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer Click!");
        Activate();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("INPUT Click!");
        Activate();
    }

    private IList<SpotController> _allSpots;

    public IList<SpotController> AllSpots
    {
        get
        {
            return _allSpots ?? (_allSpots = new SpotController[] { });
        }

        set
        {
            _allSpots = value;
        }
    }
    private HashSet<int> _neighborIndexes;

    public HashSet<int> NeighborIndexes
    {
        get { return _neighborIndexes ?? (_neighborIndexes = new HashSet<int>()); }
        set { _neighborIndexes = value; }
    }

    public bool IsCurrent { get; private set; }
}
