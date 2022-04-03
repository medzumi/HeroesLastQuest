using UnityEngine;

namespace Fishing
{
    public class ChainElement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private DistanceJoint3D _characterJoint;

        public Rigidbody Rigidbody => _rigidbody;
        public DistanceJoint3D CharacterJoint => _characterJoint;
    }
}