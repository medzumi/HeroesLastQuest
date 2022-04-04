using System;
using Fishing;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AimingState : AbstractState
{
    [SerializeField] private string _nextStateKey;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorAimKey;
    [SerializeField] private GameObject _throwGameObjectView;
    [SerializeField] private Image _image;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private FisherData _fisherData;
    [SerializeField] private GameObjectData _gameObjectData;
    [SerializeField] private Rigidbody _swimmer;

    private float _currentThrow = 0;
    private bool _isReverse = false;

    public override string Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _gameObjectData.SetData("ThrowForce", _currentThrow);
            return _nextStateKey;
        }
        else
        {
            _currentThrow += (_isReverse? - _fisherData.ThrowSpeed : _fisherData.ThrowSpeed) * Time.deltaTime;
            SetViewValue(_currentThrow);
            if (_currentThrow > 1)
            {
                _isReverse = true;
            }
            else if (_currentThrow < 0)
            {
                _isReverse = false;
            }
            return Key;
        }
    }

    private void SetViewValue(float value)
    {
        if (_spriteRenderer)
        {
            _spriteRenderer.size = new Vector2(value, _spriteRenderer.size.y);
        }

        if (_image)
        {
            _image.fillAmount = value;
        }
    }

    public override void Enter()
    {
        _swimmer.constraints = RigidbodyConstraints.FreezePositionY;
        _throwGameObjectView.SetActive(true);
        _currentThrow = 0;
        _isReverse = false;
        _animator.SetBool(_animatorAimKey, true);
    }

    public override void Exit()
    {
        _throwGameObjectView.SetActive(false);
        _animator.SetBool(_animatorAimKey, false);
    }
}