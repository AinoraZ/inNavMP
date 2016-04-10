﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using LitJson;

public class OpenScript : MonoBehaviour {

    GameObject active = null;
    bool timerStart = false;
    public float fixTimer = 0.08f;
    float timer = 0f;

    // Update is called once per frame
    void Update(){
        ActiveTimer();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            BrowserOpen = false;
        }
    }

    void Awake() {
    }

    FileBrowser file;
    bool BrowserOpen = false;

    public void Clicked(){
        if (active != null){
            Destroy(active);
        }
        BrowserOpen = true;
        file = new FileBrowser(Application.dataPath + "/Saves");
        file.searchPattern = "*.json";
    }

    void OnGUI()
    {
        if (BrowserOpen){
            if (file.draw())
            {
                if (file.outputFile == null)
                {
                    BrowserOpen = false;
                }
                else {
                    string jsonPath = file.outputFile.ToString();
                    BrowserOpen = false;
                    StartCoroutine(ClickedWait(jsonPath));
                }
            }
        }
    }

    GameObject ActiveCheck(){
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        for (int x = 0; x < points.Length; x++){
            if (!points[x].GetComponent<PointCreated>().isLocked){
                return points[x];
            }
        }
        return null;
    }

    private JsonData itemData;
    public List<JsonOpen> points;

    IEnumerator ClickedWait(string jsonPath){
        points = new List<JsonOpen>();
        yield return new WaitForEndOfFrame();
        string json = File.ReadAllText(jsonPath);
        itemData = JsonMapper.ToObject(json);

        for (int x = 0; x < JsonLength(itemData["Points"]); x++){
            var defaultPath = itemData["Points"][x];
            double position1 = (double)defaultPath["position"][0];
            double position2 = (double)defaultPath["position"][1];
            double[] position = { position1, position2 };

            JsonOpen add = new JsonOpen{
                pointName = (string)defaultPath["pointName"],
                teacher = (string)defaultPath["teacher"],
                lesson = (string)defaultPath["lesson"],
                position = position,
                connection = (string)defaultPath["connection"],
                snapPoint = (string)defaultPath["snapPoint"]
            };
            points.Add(add);
        }
        GameObject.Find("Main Camera").GetComponent<Open>().OpenNew(points);

    }

    int JsonLength(JsonData itemData) {
        int itemDataSize = 0;
        while (true){
            try{
                if (itemData[itemDataSize] == null) { }
                itemDataSize++;
            }
            catch{
                return itemDataSize;
            }
        }
    }

    void ActiveTimer(){
        if (Input.GetMouseButtonDown(0)){
            if (active != null && ActiveCheck() == null && !timerStart){
                timerStart = true;
            }
            if (timerStart){
                timer += Time.deltaTime;
                if (timer > fixTimer){
                    timerStart = false;
                    timer = 0f;
                    if (active != null && ActiveCheck() == null){
                        active = null;
                    }
                }
            }
            if (ActiveCheck() != null){
                active = ActiveCheck();
            }
        }
    }
}

[System.Serializable]
public class JsonOpen {
    public string pointName { get; set; }
    public string teacher { get; set; }
    public string lesson { get; set; }
    public double[] position { get; set; }
    public string connection { get; set; }
    public string snapPoint { get; set; }
}