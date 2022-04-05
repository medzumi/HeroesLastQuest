using System;
using System.Collections;
using Fishing;
using Quest.Scrypts.MVC;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

namespace Fishing
{
    [Serializable]
    public class FishingWaitState : AbstractState
    {
        [SerializeField] private string _resetKey;
        [SerializeField] private string _nextKey;
        [SerializeField] private FisherData _fisherData;
        [SerializeField] private FishAsset _fishAsset;
        [SerializeField] private GameObjectData _gameObjectData;

        private float _time = 0;

        public override string Update()
        {
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                return _nextKey;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && QuestView.IsEnabledInput)
                {
                    return _resetKey;
                }
                else
                {
                    return Key;
                }
            }
        }

        public override void EnterHandler()
        {
            var list = _fishAsset.listFish;
            var sum = 0f;
            var catched = _gameObjectData.ReadData<int>("Catched", 0);
            foreach (var fishConfig in list)
            {
                if (!fishConfig.IsLegendary)
                {
                    sum += fishConfig.ChanceWeight;
                }
                else if (catched >= _fishAsset.LStart)
                {
                    sum += fishConfig.ChanceWeight + (catched - _fishAsset.LStart) * _fishAsset.PlusChance;
                }
            }

            var random = Random.Range(0, sum);
            int id = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].IsLegendary)
                {
                    random -= list[i].ChanceWeight;
                }
                else if (catched >= _fishAsset.LStart)
                {
                    random -= (list[i].ChanceWeight + (catched - _fishAsset.LStart) * _fishAsset.PlusChance);
                }
                if (random <= 0)
                {
                    id = i;
                }
            }
            _gameObjectData.SetData("FishReward", Random.Range(0, id));
            _time = Random.Range(_fisherData.MinNibble, _fisherData.MaxNibble);
        }

        public override void ExitHandler()
        {
            
        }
    }

    [Serializable]
    public class FishingState : AbstractState
    {
        [SerializeField] private Rigidbody _swimmer;

        [SerializeField] private string _resetKey;
        [SerializeField] private string _waitKey;
        [SerializeField] private string _catchKey;
        [SerializeField] private Fisher _fisher;
        [SerializeField] private FisherData _fisherData;

        private Coroutine _coroutine;
        private float _timer;
        private string _returnableKey;
        public override string Update()
        {
            _timer -= Time.deltaTime;
            if (_timer < 0 && _coroutine == null)
            {
                _coroutine = _fisher.StartCoroutine(UnderwaterEnumerator());
            }

            if (_timer > 0 && Input.GetKeyDown(KeyCode.Mouse0) && QuestView.IsEnabledInput)
            {
                return _resetKey;
            }
            return _returnableKey;
        }

        public override void EnterHandler()
        {
            _timer = Random.Range(_fisherData.MinTug, _fisherData.MaxTug);
            _coroutine = null;  
            _returnableKey = Key;
        }

        public override void ExitHandler()
        {
            if (_coroutine != null)
            {
                _fisher.StopCoroutine(_coroutine);
                _coroutine = null;
            }
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
            const float playTime = 0.1f;
            Vector3 underwater =_swimmer.transform.position + (Vector3.up * (-0.25f));
            Vector3 normal = _swimmer.transform.position;
            yield return SwimmerLerp(underwater, playTime);
            yield return SwimmerLerp(normal, playTime);
        }
        
        private IEnumerator UnderwaterEnumerator()
        {
            yield return FirstUnderWater();
            yield return new WaitForSeconds(Random.Range(_fisherData.MinTug, _fisherData.MaxTug));
            const float playTime = 0.5f;
            Vector3 underwater =_swimmer.transform.position + (Vector3.up * (-0.5f));
            Vector3 normal = _swimmer.transform.position;

            yield return SwimmerLerp(underwater, playTime);
            var time = 0f;
            bool isPressed = false;
            while (time < _fisherData.TUnderwater)
            {
                time += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Mouse0) && QuestView.IsEnabledInput)
                {
                    isPressed = true;
                    _returnableKey = _catchKey;
                    time += _fisherData.TUnderwater;
                }
                yield return null;
            }
            yield return SwimmerLerp(normal, playTime);

            _returnableKey = _waitKey;
            _coroutine = null;
        }
    }

    [Serializable]
    public abstract class AbstractState : IState
    {
        [SerializeField] private string _key;
        [SerializeField] private UnityEvent OnEnter;
        [SerializeField] private UnityEvent OnExit;
        public string Key => _key;
        public abstract string Update();

        void IState.Enter()
        {
            EnterHandler();
            OnEnter?.Invoke();
        }

        void IState.Exit()
        {
            ExitHandler();
            OnExit?.Invoke();
        }

        public abstract void EnterHandler();

        public abstract void ExitHandler();
    }
    
    [Serializable]
    public class CatchState : AbstractState
    {
        [SerializeField] private string _resetKey;
        [SerializeField] private string _nextKey;
        [SerializeField] private Fisher _fisher;
        [SerializeField] private Transform _aim;
        [SerializeField] private Transform _float;
        [SerializeField] private Transform _rod;
        [SerializeField] private FisherData _fisherData;
        [SerializeField] private FishAsset _fishAsset;
        [SerializeField] private string _animationKey;
        [SerializeField] private Animator _animator;
        [SerializeField] private Material _rodMaterial;
        
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _warningColor = Color.yellow;
        [SerializeField] private Color _dangerColor = Color.red;
        [SerializeField] private string _materialColorKey = "";

        private Coroutine _fishCoroutine;
        private Coroutine _fisherCoroutine;
        private Coroutine _criticalCoroutine;

        private bool _isFailed = false;
        private float _fishSpeed;
        [SerializeField] private GameObjectData _gameObjectData;
        public override string Update()
        {
            if (_isFailed)
            {
                return _resetKey;
            }
            else
            {
                var dist = _aim.transform.position - _float.position;
                dist.y = 0;
                if (dist.magnitude > _fisherData.MaxDistance)
                {
                    return _resetKey;
                }
                else
                {
                    if (dist.magnitude < _fisherData.MinDistance)
                    {
                        return _nextKey;
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0) && QuestView.IsEnabledInput && _criticalCoroutine == null)
                    {
                        _criticalCoroutine = _fisher.StartCoroutine(CriticalEnumerator());
                    }
                    else
                    {
                        if (Input.GetKeyUp(KeyCode.Mouse0) && _criticalCoroutine != null)
                        {
                            _rodMaterial.SetColor(_materialColorKey, _normalColor);
                            _fisher.StopCoroutine(_criticalCoroutine);
                            _criticalCoroutine = null;
                        }
                    }
                    
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        _fishSpeed = 0;
                        var horizontalForward = _aim.forward;
                        horizontalForward.y = 0;
                        var floatDirection = (_float.position - _aim.position).normalized;
                        floatDirection.y = 0;
                        var dot = Vector3.Dot(horizontalForward, floatDirection.normalized);
                        if (dot > -0.1f && dot < 0.1f)
                        {
                            var direction = ( _float.position - _aim.position).normalized;
                            direction.y = 0;
                            _float.position += (-direction) * Time.deltaTime * _fisherData.FishingSpeed;
                        }
                    }
                    else
                    {
                        _fishSpeed = _fisherData.FishSpeed;
                    }
                }
                return Key;
            }
        }

        public override void EnterHandler()
        {
            _isFailed = false;
            if (Input.GetKeyDown(KeyCode.Mouse0) && QuestView.IsEnabledInput && _criticalCoroutine == null)
            {
                _criticalCoroutine = _fisher.StartCoroutine(CriticalEnumerator());
            }
            _animator.SetBool(_animationKey, true);
            _fishSpeed = _fisherData.FishSpeed;
            _rodMaterial.SetColor(_materialColorKey, _normalColor);
            _fishCoroutine = _fisher.StartCoroutine(FishCoroutine());
        }

        public override void ExitHandler()
        {
            _animator.SetBool(_animationKey, false);
            _rodMaterial.SetColor(_materialColorKey, _normalColor);
            if (_fishCoroutine != null)
            {
                _fisher.StopCoroutine(_fishCoroutine);
                _fishCoroutine = null;
            }

            if (_fishCoroutine != null)
            {
                _fisher.StopCoroutine(_fisherCoroutine);
                _fishCoroutine = null;
            }

            if (_criticalCoroutine != null)
            {
                _fisher.StopCoroutine(_criticalCoroutine);
                _criticalCoroutine = null;
            }
        }

        private IEnumerator CriticalEnumerator()
        {
            var fish = _gameObjectData.ReadData<int>("FishReward");
            var conf = _fishAsset.listFish[fish];
            var normalTime = Random.Range(conf.MinHold, conf.MaxHold);
            yield return new WaitForSeconds(normalTime);
            _rodMaterial.SetColor(_materialColorKey, _warningColor);
            yield return new WaitForSeconds(conf.MaxHold - normalTime);
            _rodMaterial.SetColor(_materialColorKey, _dangerColor);
            yield return new WaitForSeconds(0.6f);
            _isFailed = true;
            _criticalCoroutine = null;

        }

        private IEnumerator FishCoroutine()
        {
            var time = 0f;
            var angle = Random.Range(-70f, 70f);
            var direction = Quaternion.Euler(0, angle, 0) * _fisher.transform.forward;
            direction = direction.normalized;
            direction.y = 0;
            while (time < _fisherData.TRotate)
            {
                time += Time.deltaTime;
                _float.position += direction * Time.deltaTime * _fishSpeed;
                yield return null;
            }

            _fishCoroutine = _fisher.StartCoroutine(FishCoroutine());
        }
    }
}

[Serializable]
public class UpState : AbstractState
{
    [SerializeField] private string _nextKey;

    [SerializeField] private Transform _aim;
    [SerializeField] private Transform _float;
    [SerializeField] private GameObjectData _gameObjectData;
    [SerializeField] private FishAsset _fishAsset;
    [SerializeField] private Transform _spawnRootTransform;
    [SerializeField] private FisherData _fisherData;

    [SerializeField] private UnityEvent ACTIVATE_E_EVENT_PLS;
    [SerializeField] private UnityEvent DEACTIVATE_E_EVENT_PLS;
    
    private GameObject _prefab;
    private FishConfig _fishConfig;
    private int _id;
    public override string Update()
    {
        var direction = _aim.position - _float.position;
        var planePos = _aim.position;
        planePos.y = _float.position.y;

        if (direction.magnitude >= Time.deltaTime * 2)
        {
            _float.position = Vector3.Lerp(_float.position, planePos, 0.5f);
            _float.position += direction.normalized * Time.deltaTime * _fisherData.FinishAnimationSpeed;
            return Key;
        }

        ACTIVATE_E_EVENT_PLS.Invoke();
        if (Input.GetKeyDown(KeyCode.E) && QuestView.IsEnabledInput)
        {
            FishPopupController.Singleton.AddFish(_fishConfig.FishId);
            QuestController.Singleton.AddFish(_fishConfig.FishId);
            _gameObjectData.SetData("Catched", _gameObjectData.ReadData("Catched", 0) + 1);
            DEACTIVATE_E_EVENT_PLS.Invoke();
            return _nextKey;
        }

        return Key;
    }

    public override void EnterHandler()
    {
        _id = _gameObjectData.ReadData<int>("FishReward");
        _fishConfig = _fishAsset.listFish[_id];
        var prefab = _fishAsset.listFish[_id].Prefab;
        _prefab = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, _spawnRootTransform);
        _prefab.transform.localPosition = Vector3.zero;
    }

    public override void ExitHandler()
    {
        GameObject.Destroy(_prefab);
    }
}