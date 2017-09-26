﻿using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float progress;
	private bool goingForward = true;

    private bool move = false;

	private void Update () {
        if (move)
        {
            if (goingForward)
            {
                progress += Time.deltaTime / duration;
                if (progress > 1f)
                {
                    if (mode == SplineWalkerMode.Once)
                    {
                        progress = 1f;
                    }
                    else if (mode == SplineWalkerMode.Loop)
                    {
                        progress -= 1f;
                    }
                    else
                    {
                        progress = 2f - progress;
                        goingForward = false;
                    }
                }
            }
            else
            {
                progress -= Time.deltaTime / duration;
                if (progress < 0f)
                {
                    if (mode == SplineWalkerMode.Once)
                    {
                        progress = 0f;
                    }
                    else if (mode == SplineWalkerMode.Loop)
                    {
                        progress += 1f;
                    }
                    else
                    {
                        progress = -progress;
                        goingForward = true;
                    } 
                }
            }

            Vector3 position = spline.GetPoint(progress);
            transform.position = position;
            // transform.localPosition = position;
            if (lookForward)
            {
                transform.LookAt(position + spline.GetDirection(progress));
            }
        }
	}

    public void SetMove(bool m)
    {
        move = m;
    }

    public void SetGoingForward(bool f)
    {
        goingForward = f;
    }
}