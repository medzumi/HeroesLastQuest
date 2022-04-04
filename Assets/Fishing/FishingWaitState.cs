using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

namespace Fishing
{
    [Serializable]
    public class FishingWaitState : IState
    {
        [SerializeField] private string _key;
        [SerializeField] private string _resetKey;
        [SerializeField] private string _nextKey;
        [SerializeField] private float _randomWait;
        [SerializeField] private int _fishCount;
        [SerializeField] private GameObjectData _gameObjectData;
        public string Key => _key;

        private float _time = 0;

        public string Update()
        {
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                return _nextKey;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    return _resetKey;
                }
                else
                {
                    return _key;
                }
            }
        }

        public void Enter()
        {
            _time = Random.Range(0, _randomWait);
            _gameObjectData.SetData<int>("FishType", Random.Range(0, _fishCount));
        }

        public void Exit()
        {
            
        }
    }

    [Serializable]
    public class FishingState : IState
    {
        [SerializeField] private Rigidbody _swimmer;
        [SerializeField] private float _DTug;
        [SerializeField] private string _key;
        [SerializeField] private float _tUnderwater;

        [SerializeField] private string _resetKey;
        [SerializeField] private string _waitKey;
        [SerializeField] private string _catchKey;
        [SerializeField] private Fisher _fisher;

        private Coroutine _coroutine;
        private float _timer;
        private string _returnableKey;
        
        public string Key => _key;
        public string Update()
        {
            _timer -= Time.deltaTime;
            if (_timer < 0 && _coroutine == null)
            {
                _coroutine = _fisher.StartCoroutine(UnderwaterEnumerator());
            }

            if (_timer > 0 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                return _resetKey;
            }
            return _returnableKey;
        }

        public void Enter()
        {
            _timer = Random.Range(0, _DTug);
            _coroutine = null;
            _returnableKey = _key;
        }

        public void Exit()
        {
        }
        
        private IEnumerator SwimmerLerp(Vector3 normal, float playTime)
        {
            var time = 0f;
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                _swimmer.transform.position = Vector3.Lerp(_swimmer.position, normal, time / playTime);
                yield return null;
            }
        }

        private IEnumerator FirstUnderWater()
        {
            const float playTime = 0.25f;
            Vector3 underwater =_swimmer.transform.position + (Vector3.up * -2);
            Vector3 normal = _swimmer.transform.position;
            yield return SwimmerLerp(underwater, playTime);
            yield return SwimmerLerp(normal, playTime);
        }
        
        private IEnumerator UnderwaterEnumerator()
        {
            yield return FirstUnderWater();
            
            const float playTime = 0.5f;
            Vector3 underwater =_swimmer.transform.position + (Vector3.up * -2);
            Vector3 normal = _swimmer.transform.position;

            yield return SwimmerLerp(underwater, playTime);
            var time = 0f;
            bool isPressed = false;
            while (time < _tUnderwater)
            {
                time += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    isPressed = true;
                    time += _tUnderwater;
                }
                yield return null;
            }
            yield return SwimmerLerp(normal, playTime);

            _returnableKey = isPressed ? _catchKey :_waitKey;
            _coroutine = null;
        }
    }
}