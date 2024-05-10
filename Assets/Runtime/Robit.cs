using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime
{
    public class Robit : MonoBehaviour
    {
        public float moveSpeed;
        public float acceleration;
        public float rotationSpeed;
        
        [Space]
        public new Collider collider;
        
        private Vector3 lastPosition;
        private Vector3 position;
        private Vector3 velocity;
        private Vector3 force;
        
        public Vector3 moveDirection { get; set; }

        private void FixedUpdate()
        {
            lastPosition = position;
            transform.position = position;
            
            Move();
            Collide();
            
            position += velocity * Time.deltaTime;
            velocity += force * Time.deltaTime;
            force = Vector3.zero;
            
            position.y = 0.5f;
            velocity.y = 0.5f;

            RotateTowards();
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(lastPosition, position, (Time.time - Time.fixedTime) / Time.fixedDeltaTime);
        }

        private void RotateTowards()
        {
            var magnitude = moveDirection.magnitude;
            if (magnitude < float.Epsilon) return;
            var target = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * magnitude * Time.deltaTime);
        }

        private void Move()
        {
            var target = moveDirection * moveSpeed;
            force += (target - velocity) * acceleration;
        }

        private void Collide()
        {
            var broadPhase = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents);
            foreach (var other in broadPhase)
            {
                if (other.transform.IsChildOf(transform)) continue;
                
                if (Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation, other, other.transform.position, other.transform.rotation, out var direction, out var distance))
                {
                    position += direction * distance;
                    velocity += direction * Mathf.Max(0f, Vector3.Dot(direction, -velocity));
                }
            }

            CheckForGround(Vector3.left);
            CheckForGround(Vector3.right);
            CheckForGround(Vector3.forward);
            CheckForGround(Vector3.back);
        }

        private void CheckForGround(Vector3 direction)
        {
            var ray = new Ray(position + Vector3.up * 0.005f + Vector3.Project(velocity, direction) * Time.deltaTime, Vector3.down);
            if (!Physics.Raycast(ray, 0.01f))
            {
                velocity += Vector3.Project(-velocity, direction);
            }
        }
    }
}
