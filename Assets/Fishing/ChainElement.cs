using UnityEngine;

namespace Fishing
{
    public class ChainElement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CharacterJoint _characterJoint;

        public Rigidbody Rigidbody => _rigidbody;
        public CharacterJoint CharacterJoint => _characterJoint;
    }
}