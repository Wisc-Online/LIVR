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


    // Use this for initialization
    void Start()
    {
        InitializeSpots();

        spots[0].Activate();
    }


    void InitializeSpots()
    {
        spots.Clear();
        GameObject spot;
        SpotController spotController;

        for (int i = 0; i < spotContainer.transform.childCount; ++i)
        {
            spot = spotContainer.transform.GetChild(i).gameObject;

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
        
        Vector3 positionFixerScale = new Vector3(1, 1, 1.1f);

        for (int i = 0; i < jsonPoints.Length; ++i)
        {

            spotController = GameObject.Instantiate<SpotController>(spotPrefab, spotContainer.transform);
            spotController.transform.localPosition = Vector3.Scale(positionFixerScale, jsonPoints[i].Position);

            spotController.frame = jsonPoints[i].Frame;
            spotController.videoPlayer = this.videoPlayer;
            spotController.AllSpots = spots;
            spotController.NeighborIndexes = new HashSet<int>(jsonPoints[i].Neighbors);

            spots.Add(spotController);
        }
    }


    class JsonPointTime
    {
        public Vector3 Position { get; set; }
        public int Frame { get; set; }
        public int[] Neighbors { get; set; }
    }


    int spotIndex = 0;

    public void GoToNext()
    {
        spotIndex = (spotIndex + 1) % spots.Count;

        spots[spotIndex].Activate();
    }

}
