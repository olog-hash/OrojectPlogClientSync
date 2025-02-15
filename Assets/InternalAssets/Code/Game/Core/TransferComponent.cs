using UnityEngine;

namespace ProjectOlog.Code.Game.Core
{
    [DisallowMultipleComponent]
    public class TransferComponent : MonoBehaviour
    {
        public Vector3 LinearVelocity;
        public Vector3 AngularVelocity;

        private void Update()
        {
            transform.position += LinearVelocity * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(AngularVelocity * Time.deltaTime);
        }
    }
}