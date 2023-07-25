using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Scripts
{
    [ExecuteAlways]
    public class BezierLineView : MonoBehaviour
    {
        public Vector3 startPoint = Vector3.zero;
        public Vector3 controlPoint;
        public Vector3 endPoint;

        private List<Vector3> controlPoints = new List<Vector3>();
        private List<Vector3> velocities = new List<Vector3>();

        public bool useDampening = true;
        public float dampening = 0.1f;

        private void OnEnable()
        {
            ResetChildren();
        }

        private void OnDisable()
        {
        }

        private void Update()
        {
            Refresh();
        }

        public void Refresh()
        {
            //Make sure we have 3 points in our list
            while (controlPoints.Count < 3)
            {
                controlPoints.Add(Vector3.zero);
            }

            while (controlPoints.Count > 3)
            {
                controlPoints.RemoveAt(controlPoints.Count - 1);
            }

            controlPoints[0] = startPoint;
            controlPoints[1] = controlPoint;
            controlPoints[2] = endPoint;

            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                var t = i / (float) (childCount - 1);
                var pt = BezierCurve.Point(t, controlPoints);

                if (!useDampening)
                {
                    child.transform.localPosition = pt;
                }
                else
                {
                    var v = velocities[i];
                    child.transform.localPosition = Vector3.SmoothDamp(child.transform.localPosition, pt, ref v, dampening);
                    velocities[i] = v;
                }
            }
        }

        private void ResetChildren()
        {
            velocities.Clear();
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                child.transform.localPosition = Vector3.zero;
                velocities.Add(new Vector3(0,0,0));
            }
        }

        public float GizmoRadius = 10f;

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.TransformPoint(startPoint), GizmoRadius);
            Gizmos.DrawSphere(transform.TransformPoint(controlPoint), GizmoRadius);
            Gizmos.DrawSphere(transform.TransformPoint(endPoint), GizmoRadius);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(BezierLineView))]
    public class BezierLineViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            var lineView = (BezierLineView) target;
            var transform = lineView.transform;

            lineView.controlPoint = Handles.DoPositionHandle(transform.TransformPoint(lineView.controlPoint), Quaternion.identity);
            lineView.controlPoint = transform.InverseTransformPoint(lineView.controlPoint);

            lineView.endPoint = Handles.DoPositionHandle(transform.TransformPoint(lineView.endPoint), Quaternion.identity);
            lineView.endPoint = transform.InverseTransformPoint(lineView.endPoint);

            lineView.Refresh();
        }
    }

#endif
}
