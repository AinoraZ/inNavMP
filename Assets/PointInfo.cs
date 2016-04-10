using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointInfo : MonoBehaviour {

    void Start () {

	}

    public List<string> information;
    public GameObject connection;
    public GameObject snapPoint;

    // Update is called once per frame
    void Update () {
        ConnectionPointAdd();
	}

    void ConnectionPointAdd(){
        if(connection != GetComponent<DrawLine>().target)
            connection = GetComponent<DrawLine>().target;
    }

    public void SnappedOn(GameObject snap) {
        snapPoint = snap;
    }

    void SaveStart(string jsonPath) {
        GameObject.Find("Main Camera").GetComponent<JsonSave>().InfoRecieve(information, gameObject, connection, snapPoint, jsonPath);
    }

}
