using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class Chain : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        [SerializeField] private ChainElement _characterJoint;
        [SerializeField] private Transform _start;
        [SerializeField] private DistanceJoint3D _end;

        public float RigidbodyMass = 1f;
        public float ColliderRadius = 0.1f;
        public float JointSpring = 0.1f;
        public float JointDamper = 5f;
        public Vector3 RotationOffset;
        public Vector3 PositionOffset;

        protected List<Transform> CopySource;
        protected List<Transform> CopyDestination;

        protected static GameObject RigidBodyContainer;

        private readonly Stack<ChainElement> _pool = new Stack<ChainElement>();
        private readonly Stack<ChainElement> _activeJointStack = new Stack<ChainElement>();

        private ChainElement _constantChainElement;

        [SerializeField] private float _length = 0;

        public float Legth
        {
            get => _length;
            set
            {
                _length = value;
                SetLength(value);
            }
        }

        private void SetLength(float value)
        {
            var newCount = Mathf.FloorToInt(value / _characterJoint.CharacterJoint.Distance);
            for (int i = 0; i < newCount - _activeJointStack.Count; i++)
            {
                var newChainElement = GetChainelement();
                newChainElement.gameObject.SetActive(true);
                _end.ConnectedRigidbody = newChainElement.Rigidbody.transform;
                newChainElement.CharacterJoint.ConnectedRigidbody =
                    _activeJointStack.Count == 0 ? _start : _activeJointStack.Peek().Rigidbody.transform;
                newChainElement.Rigidbody.position =
                    (_start.transform.position + newChainElement.CharacterJoint.ConnectedRigidbody.position) / 2f;
                _activeJointStack.Push(newChainElement);
            }
            for (int i = 0; i < _activeJointStack.Count - newCount; i++)
            {
                var toDeleteJoint = _activeJointStack.Pop();
                _end.ConnectedRigidbody = _activeJointStack.Count == 0 ? _start : _activeJointStack.Peek().Rigidbody.transform;
                toDeleteJoint.gameObject.SetActive(false);
                _pool.Push(toDeleteJoint);
            }
        }

        private void Update()
        {
            var linePointCount = 2 + _activeJointStack.Count;
            _lineRenderer.positionCount = linePointCount;
            _lineRenderer.SetPosition(0, _start.transform.position);
            _lineRenderer.SetPosition(linePointCount - 1, _end.transform.position);
            var index = linePointCount - 2;
            foreach (var chainElement in _activeJointStack)
            {
                _lineRenderer.SetPosition(index, chainElement.Rigidbody.position);
                index--;
            }
        }

        private ChainElement GetChainelement()
        {
            return _pool.Count > 0 ? _pool.Pop() : Instantiate(_characterJoint, transform);
        }

        private void Awake()
        {
            for (int i = 0; i < 5; i++)
            {
                _pool.Push(Instantiate(_characterJoint, transform));
            }

            _end.ConnectedRigidbody = _start;
            SetLength(_length);
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                SetLength(_length);
            }
        }
    }
}