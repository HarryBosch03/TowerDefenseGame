using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime
{
    public class PlayerController : MonoBehaviour
    {
        public Robit robit;
        
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (robit)
            {
                var input = Vector2.zero;
                input.x = Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue();
                input.y = Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue();
                
                var orientation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
                robit.moveDirection = orientation * new Vector3(input.x, 0f, input.y);

                CameraController.instance.target = robit.transform;
            }
        }
    }
}