using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CrabGameMod
{
    class PhysicsObject
    {
        public int objectId;
        public MonoBehaviourPublicObRiSiupVeSiQuVeLiQuUnique physicsObject = null;
        public Rigidbody rigidbody = null;
        public bool canBePicked = false;

        private Vector3 position;
        private Vector3 velocity;

        public PhysicsObject(int objectId)
        {
            this.objectId = objectId;
        }

        public Vector3 GetPosition()
        {
            return rigidbody.position;
        }

        public Vector3 GetVelocity()
        {
            return rigidbody.velocity;
        }

        public void SetPosition(float x, float y, float z)
        {
            position.Set(x, y, z);
        }

        public void SetVelocity(float x, float y, float z)
        {
            velocity.Set(x, y, z);
        }


        public virtual void Update()
        {
            UpdateRigidBodyData();
        }

        public void UpdateRigidBodyData()
        {
            var rb = GetRigidBody();

            if (rb == null) return;

            position = rb.position;
            velocity = rb.velocity;
        }

        public Rigidbody GetRigidBody()
        {
            if (physicsObject == null) return null;

            var rb = physicsObject.GetComponent<UnityEngine.Rigidbody>();

            if (!rb) return null;

            return rb;
        }

        public void SendSync()
        {
            MonoBehaviourPublicInInUnique.PhysicsObjectSnapshot(0, objectId, position, velocity, Vector3.zero, Quaternion.identity);
        }

        public virtual bool OnInteract(Player player)
        {
            return canBePicked;
        }
    }
}
