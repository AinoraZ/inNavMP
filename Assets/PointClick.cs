using UnityEngine;
using System.Collections;

public class PointClick : MonoBehaviour {

	void Start () {
	
	}

    public float sizeAdjustment = 0.8f;
    bool removing = false;

    void PointRemove() {
        removing = true;
    }

    void Update () {
        if (Input.GetMouseButtonDown(0)){
            if (DistanceCalculation() <= sizeAdjustment && GetComponent<PointCreated>().isLocked){
                if (ConnectionCheck() && SnapOnCheck() && !removing)
                    GameObject.FindGameObjectWithTag("MainCamera").SendMessage("OnLinkClick", gameObject, SendMessageOptions.DontRequireReceiver);
            }
            removing = false;
        }
    }

    float DistanceCalculation(){
        Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mPos = new Vector2(mouseWorldSpace.x, mouseWorldSpace.y);

        return Vector2.Distance(mPos, new Vector2(transform.position.x, transform.position.y));
    }

    bool ConnectionCheck() {
        for (int x = 0; x < GameObject.FindGameObjectsWithTag("Point").Length; x++)
        {
            if (!GameObject.FindGameObjectsWithTag("Point")[x].GetComponent<PointCreated>().isLocked) //Find the active gameObject (point)
            {
                GameObject active = GameObject.FindGameObjectsWithTag("Point")[x];

                if (GetComponent<DrawLine>().target == active.GetComponent<DrawLine>().target){
                    Destroy(active);
                    return false;
                }
                else if (gameObject == active.GetComponent<DrawLine>().target.GetComponent<DrawLine>().target){
                    Destroy(active);
                    return false;
                }
                for (int z = 0; z < GameObject.FindGameObjectsWithTag("Point").Length; z++){
                    GameObject found = GameObject.FindGameObjectsWithTag("Point")[z];
                    if (active.GetComponent<DrawLine>().target == found.GetComponent<DrawLine>().target && gameObject == found.GetComponent<PointInfo>().snapPoint){
                        Destroy(active);
                        return false;
                    }
                    if (gameObject == found.GetComponent<DrawLine>().target && active.GetComponent<DrawLine>().target == found.GetComponent<PointInfo>().snapPoint){
                        Destroy(active);
                        return false;
                    }
                }
            }
        }
        return true;
    }

    bool SnapOnCheck() {
        for (int x = 0; x < GameObject.FindGameObjectsWithTag("Point").Length; x++){
            if (!GameObject.FindGameObjectsWithTag("Point")[x].GetComponent<PointCreated>().isLocked){
                if (GameObject.FindGameObjectsWithTag("Point")[x].GetComponent<DrawLine>().target == gameObject){
                    Destroy(GameObject.FindGameObjectsWithTag("Point")[x]);
                    return false;
                }
                else {
                    GameObject.FindGameObjectsWithTag("Point")[x].SendMessage("OnLinkClick", gameObject, SendMessageOptions.DontRequireReceiver);
                    return true;
                }

            }
        }
        return true;
    }
}
