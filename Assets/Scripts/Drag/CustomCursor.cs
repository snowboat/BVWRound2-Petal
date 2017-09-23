using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class CustomCursor :  ObjectCursor{
    bool detected;
    IInputSource inputSource;
    uint sourceId;
    GameObject cam;

    RaycastHit hitResult;
    GameObject hitObject;
    Vector3 normal;

    /// <summary>
    /// Input source detected callback for the cursor
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnSourceDetected(SourceStateEventData eventData)
    {
        base.OnSourceDetected(eventData);
        detected = true;
        inputSource = eventData.InputSource;
        sourceId = eventData.SourceId;

    }
    protected override void RegisterManagers()
    {
        base.RegisterManagers();
        cam = Camera.main.gameObject;
    }



    /// <summary>
    /// Input source lost callback for the cursor
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnSourceLost(SourceStateEventData eventData)
    {
        base.OnSourceLost(eventData);
        detected = false; 
    }
    protected override void UpdateCursorTransform()
    {
        Vector3 targetPosition;
        Vector3 targetScale;
        Quaternion targetRotation;
        if (detected)
        {
            Vector3 pos;
            bool log;
            hitObject = null;
            if (inputSource.TryGetPosition(sourceId, out pos))
            {
                pos += cam.transform.rotation * DragControl.InputOffset;
                //Debug.Log(pos);
                Ray inputRay = new Ray(cam.transform.position, pos - cam.transform.position);
                RaycastHit hit;
                normal = inputRay.direction;
                if (Physics.Raycast(inputRay, out hit, 100f))
                {
                    Draggable draggable = hit.collider.gameObject.GetComponent<Draggable>();
                    hitResult = hit;
                    hitObject = hit.collider.gameObject;
                }
            }
            else
            {
                hitObject = null;
            }
            // Get the necessary info from the gaze source
            GameObject newTargetedObject = hitObject;

            // Get the forward vector looking back at camera
            Vector3 lookForward = -normal;

            // Normalize scale on before update
            targetScale = Vector3.one;

            // If no game object is hit, put the cursor at the default distance
            if (newTargetedObject == null)
            {
                this.TargetedObject = null;
                this.TargetedCursorModifier = null;
                targetPosition = cam.transform.position + normal * DefaultCursorDistance;
                targetRotation = Quaternion.LookRotation(lookForward, Vector3.up);
            }
            else
            {
                // Update currently targeted object
                //this.TargetedObject = newTargetedObject;

                if (TargetedCursorModifier != null)
                {
                    TargetedCursorModifier.GetModifiedTransform(this, out targetPosition, out targetRotation, out targetScale);
                }
                else
                {
                    // If no modifier is on the target, just use the hit result to set cursor position
                    targetPosition = hitResult.point + (lookForward * SurfaceCursorDistance);
                    targetRotation = Quaternion.LookRotation(Vector3.Lerp(hitResult.normal, lookForward, LookRotationBlend), Vector3.up);
                }
            }

            float deltaTime = UseUnscaledTime
                ? Time.unscaledDeltaTime
                : Time.deltaTime;
        }
        else {
            targetPosition = Vector3.zero;
            targetScale = Vector3.zero;
            targetRotation = Quaternion.Euler(0f,0f,0f);
            detected = false;
        }
        // Use the lerp times to blend the position to the target position
        transform.position = targetPosition;
        transform.localScale = targetScale;
        transform.rotation = targetRotation;
    }
    /// <summary>
    /// Function for consuming the OnInputUp events
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnInputUp(InputEventData eventData)
    {
        base.OnInputUp(eventData);
    }

    /// <summary>
    /// Function for receiving OnInputDown events from InputManager
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnInputDown(InputEventData eventData)
    {
        base.OnInputDown(eventData);
        inputSource = eventData.InputSource;
        sourceId = eventData.SourceId;
    }
}
