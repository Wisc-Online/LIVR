using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using System.Linq;

public class SceneController : MonoBehaviour
{



    public GameObject spotContainer;
    public SpotController spotPrefab;
    public VideoPlayer videoPlayer;
    public string spotsJson = "points.json";

    [HideInInspector]
    private readonly List<SpotController> spots = new List<SpotController>();


    Vector3 cameraParentPosition;

    // Use this for initialization
    void Start()
    {
        InitializeSpots();

        spots[0].Activate();

        cameraParentPosition = Camera.main.transform.parent.position;


    }

    void InitializeSpots()
    {
        spots.Clear();
        GameObject spot;
        SpotController spotController;

        for (int i = 0; i < spotContainer.transform.childCount; ++i)
        {
            spot = spotContainer.transform.GetChild(i).gameObject;

            spot.transform.parent = null;
            GameObject.Destroy(spot);
        }


        var jsonPath = System.IO.Path.Combine(Application.dataPath, spotsJson);

        JsonPointTime[] jsonPoints;

        using (var stream = new System.IO.FileStream(jsonPath, System.IO.FileMode.Open))
        using (var textReader = new System.IO.StreamReader(stream))
        using (var jsonReader = new Newtonsoft.Json.JsonTextReader(textReader))
        {

            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

            jsonPoints = serializer.Deserialize<JsonPointTime[]>(jsonReader);
        }


        int frameOffset = (int)(videoPlayer.frameRate / 2);
        int frameRate = (int)videoPlayer.frameRate;

        for (int i = 0; i < jsonPoints.Length; ++i)
        {

            spotController = GameObject.Instantiate<SpotController>(spotPrefab, spotContainer.transform);
            spotController.transform.localPosition = new Vector3(jsonPoints[i].X, jsonPoints[i].Y, jsonPoints[i].Z);

            spotController.frame = frameOffset + (i * frameRate);
            spotController.videoPlayer = this.videoPlayer;

            spots.Add(spotController);
        }
    }


    class JsonPointTime
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float T { get; set; }
    }


    int spotIndex = 0;

    public void GoToNext()
    {
        spotIndex = (spotIndex + 1) % spots.Count;

        spots[spotIndex].Activate();
    }

}
