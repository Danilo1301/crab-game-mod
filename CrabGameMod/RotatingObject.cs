using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CrabGameMod
{
    class RotatingObject : PhysicsObject
    {
        public Player followPlayer = null;

        private float angle = 0f;


        public RotatingObject(int objectId) : base(objectId)
        {

        }

        public override void Update()
        {
            base.Update();

            angle += 0.1f;

            var rb = GetRigidBody();

            if (rb == null) return;

            var pos = followPlayer.position + Vector3.up * 2.5f;

            var lDirection = new UnityEngine.Vector3(UnityEngine.Mathf.Sin(angle), 0, UnityEngine.Mathf.Cos(angle));

            pos += lDirection * 2.0f;

            SetPosition(pos.x, pos.y, pos.z);
            SetVelocity(0, 0, 0);

            SendSync();
        }

        public override bool OnInteract(Player player)
        {
            return false;
        }
    }
}
