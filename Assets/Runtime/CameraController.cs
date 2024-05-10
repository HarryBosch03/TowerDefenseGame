using UnityEngine;

namespace Runtime
{
    public class CameraController : MonoBehaviour
    {
        public float smoothing;
        public Vector3 offset = new Vector3(0f, 15f, -10f);
        
        public Transform target { get; set; }

        private static CameraController instanceInternal;
        public static CameraController instance
        {
            get
            {
                if (!instanceInternal) instanceInternal = FindFirstObjectByType<CameraController>();
                return instanceInternal;
            }
        }
        
        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, Time.deltaTime / Mathf.Max(Time.deltaTime, smoothing));
            transform.rotation = Quaternion.LookRotation(-offset, Vector3.up);
        }
    }
}