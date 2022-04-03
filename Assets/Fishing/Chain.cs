using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class Chain : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        [SerializeField] private ChainElement _characterJoint;
        [SerializeField] private CharacterJoint _start;
        [SerializeField] private Rigidbody _end;

        private readonly Stack<ChainElement> _pool = new Stack<ChainElement>();
        private readonly Stack<ChainElement> _activeJointStack = new Stack<ChainElement>();

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
            var newCount = Mathf.FloorToInt(value / _characterJoint.CharacterJoint.connectedAnchor.magnitude);
            for (int i = 0; i < newCount - _activeJointStack.Count; i++)
            {
                var newChainElement = GetChainelement();
                newChainElement.gameObject.SetActive(true);
                _start.connectedBody = newChainElement.Rigidbody;
                newChainElement.CharacterJoint.connectedBody =
                    _activeJointStack.Count == 0 ? _end : _activeJointStack.Peek().Rigidbody;
                newChainElement.Rigidbody.position =
                    (_start.transform.position + newChainElement.CharacterJoint.connectedBody.position) / 2f;
                _activeJointStack.Push(newChainElement);
            }
            for (int i = 0; i < _activeJointStack.Count - newCount; i++)
            {
                var toDeleteJoint = _activeJointStack.Pop();
                _start.connectedBody = _activeJointStack.Count == 0 ? _end : _activeJointStack.Peek().Rigidbody;
                toDeleteJoint.gameObject.SetActive(false);
                _pool.Push(toDeleteJoint);
                //todo deleting
            }
        }

        private void Update()
        {
            var linePointCount = 2 + _activeJointStack.Count;
            _lineRenderer.positionCount = linePointCount;
            _lineRenderer.SetPosition(0, _start.transform.position);
            _lineRenderer.SetPosition(linePointCount - 1, _end.position);
            var index = 1;
            foreach (var chainElement in _activeJointStack)
            {
                _lineRenderer.SetPosition(index, chainElement.Rigidbody.position);
                index++;
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