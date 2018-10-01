using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneController : MonoBehaviour
{



    public GameObject spotContainer;
    public SpotController spotPrefab;
    public string spotsJson = "points.json";

    [HideInInspector]
    private readonly List<SpotController> spots = new List<SpotController>();

    // Use this for initialization
    void Start()
    {
        InitializeSpots();
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


        foreach(var jsonPoint in jsonPoints)
        {
            spotController = GameObject.Instantiate<SpotController>(spotPrefab, spotContainer.transform);
            spotController.transform.localPosition = new Vector3(jsonPoint.X, jsonPoint.Y, jsonPoint.Z);

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


    // Update is called once per frame
    void Update()
    {

    }
}
