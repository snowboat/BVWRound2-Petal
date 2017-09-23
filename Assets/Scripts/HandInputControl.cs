using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class HandInputControl : MonoBehaviour, IInputHandler {

    public static Vector3 InputOffset = new Vector3(-0.06f,0.05f,0f);


    GameObject cam;

    IInputSource inputSource;
    uint inputSourceId;
    GameObject interacted;
    bool isDown;

    public void OnInputDown(InputEventData eventData)
    {
        isDown = false;
        Vector3 pos;
        inputSource = eventData.InputSource;
        inputSourceId = eventData.SourceId;
        inputSource.TryGetPosition(inputSourceId, out pos);
        pos += cam.transform.rotation * InputOffset;
        //Debug.Log(pos);
        Ray inputRay = new Ray(cam.transform.position, pos - cam.transform.position);
        RaycastHit hit;
        var colliders = Physics.OverlapSphere(pos, 0.1f);
        if (colliders.Length > 0)
        {
            Draggable draggable = colliders[0].gameObject.GetComponent<Draggable>();
            if (draggable)
            {
                interacted = colliders[0].gameObject;
                draggable.updateInputPos(pos);
                draggable.onInputDown();
                isDown = true;
            }
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        if (isDown)
        {
            interacted.GetComponent<Draggable>().onInputUp();
            isDown = false;
            interacted = null;

        }
    }

    // Use this for initialization
    void Start () {
        cam = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (isDown)
        {
            Vector3 pos;
            if (inputSource.TryGetPosition(inputSourceId, out pos))
            {
                pos += cam.transform.rotation * InputOffset;
                interacted.GetComponent<Draggable>().updateInputPos(pos);
            }
            else
            {
                interacted.GetComponent<Draggable>().onInputCanceled();
                isDown = false;
                interacted = null;
            }
        }
	}
}
