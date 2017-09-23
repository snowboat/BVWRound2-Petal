using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Draggable : MonoBehaviour {
    GameObject cam;

    Vector3 inputPos;
    bool dragging;
    Transform transform;
    Vector3 placePosition;
    float originalDistance;
	// Use this for initialization
	void Start () {
        transform = this.gameObject.transform;
        cam = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (dragging)
        {
            float distanceFromCam = Vector3.Distance(cam.transform.position, inputPos) + originalDistance;
            //Debug.Log("distanceFromCam " + distanceFromCam);
            Ray dragRay = new Ray(cam.transform.position, inputPos - cam.transform.position);
            RaycastHit hit;

            //placePosition = dragRay.direction * originalDistance + inputPos;

            placePosition = inputPos;
            this.gameObject.transform.position = placePosition;
            //this.transform.LookAt(cam.transform.position);
        }
	}
    public void onInputDown()
    {
        dragging = true;
        originalDistance = Mathf.Abs(Vector3.Distance(inputPos, gameObject.transform.position));
        //removeCollider();
    }

    public void onInputUp()
    {
        addCollider();
        dragging = false;
    }
    public void updateInputPos(Vector3 pos) {
        inputPos = pos;
    }
    public void onInputCanceled() {
        onInputUp();
    }
    void removeCollider()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
    }

    void addCollider()
    {
        this.gameObject.GetComponent<Collider>().enabled = true;
    }
}
