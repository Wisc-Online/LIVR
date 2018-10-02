using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class SpotController : MonoBehaviour, IPointerClickHandler, HoloToolkit.Unity.InputModule.IInputClickHandler {

    public Material material;
    public int frame;
    public VideoPlayer videoPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
        Camera.main.transform.parent.position = new Vector3(this.transform.position.x, Camera.main.transform.parent.position.y, this.transform.position.z);

        videoPlayer.frame = frame;
        videoPlayer.Pause();
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
}
