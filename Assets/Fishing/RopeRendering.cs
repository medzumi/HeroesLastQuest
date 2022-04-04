using System;
using UnityEngine;

namespace Fishing
{
    public class RopeRendering : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;

        public int PointCount = 2;

        public float CoefA = 1;
        public float CoefB = 0;
        public float CoefC = 0;

        private void Update()
        {
            _lineRenderer.positionCount = PointCount;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _start.position);
            _lineRenderer.SetPosition(1, _end.position);
            // var start2 = new Vector2(_start.position.x, _start.position.z);
            // var end2 = new Vector2(_end.position.x, _end.position.z);
            // var direction2 = end2 - start2;
            // direction2 = direction2.normalized;
            // var magnitude = direction2.magnitude;
            // var delta = magnitude / PointCount;
            // var yCoef = _end.position.y - _start.position.y;
            // for (int i = 0; i < PointCount; i++)
            // {
            //     var newPos = start2 + direction2 * delta;
            //     var y = newPos.magnitude
            //     _lineRenderer.SetPosition(i, new Vector3());
            // }
        }
    }
}