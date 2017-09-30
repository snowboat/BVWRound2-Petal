using UnityEngine;
using System;
using BASE;

public class SplineWalker : MonoBehaviour {
    public bool needFixHeight = true;

	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	public float progress;
	private bool goingForward = true;

    private bool move = false;

    public Vector3 rotationAdjustment = new Vector3(0, 0, 0);

    public Action onFinish;

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
                        if (onFinish != null) {
                            onFinish.Invoke();
                        }
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
            if (needFixHeight) {
                position += GameModel.Instance.heightOffset;
            }
            transform.position = position;
            // transform.localPosition = position;
            if (lookForward)
            {
                transform.LookAt(position + spline.GetDirection(progress));
                transform.Rotate(rotationAdjustment);
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