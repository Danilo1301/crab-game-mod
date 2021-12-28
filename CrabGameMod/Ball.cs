using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace CrabGameMod
{
    class Ball : PhysicsObject
    {
        private float kickTime = 0f;
        private float respawnTime = 0f;
        private Player playerKicked = null;


        public Ball(int objectId) : base(objectId)
        {

        }

        public override void Update()
        {
            base.Update();

            kickTime -= Time.deltaTime;
            respawnTime -= Time.deltaTime;

            var rb = GetRigidBody();

            if (rb == null) return;

            Player playerToKick = null;

            foreach (var pair in Server.GetPlayers())
            {
                var player = pair.Value;


                var distance = Vector3.Distance(player.position, GetPosition());
                if (distance > 2.5f) continue;


                playerToKick = player;

                break;
            }

            if (playerToKick != null)
            {
                var pos = GetPosition();

                // Server.SendServerMessage("BALL " + pos.x + ", " + pos.y + ", " + pos.z);

                Vector3 delta = new Vector3(playerToKick.position.x - pos.x, 0, playerToKick.position.z - pos.z);
                delta.Normalize();

                //Server.SendServerMessage("DELTA " + delta.x + ", " + delta.y + ", " + delta.z);

                Vector2 ballPos = new Vector2(pos.x, pos.z);
                Vector2 playerPos = new Vector2(playerToKick.position.x, playerToKick.position.z);

                //var distance = Vector3.Distance(playerToKick.position, GetPosition());
                //var angle = Vector2.Angle(playerPos, ballPos);

                //Server.SendServerMessage("angle= " + angle);



                if (playerToKick != playerKicked) kickTime = 0;


                if (kickTime <= 0)
                {
                    //Server.SendServerMessage("kickTime <= 0? " + kickTime);




                    //angle += (float)(Math.PI / 2);


                    //var lDirection = new UnityEngine.Vector3(UnityEngine.Mathf.Sin(angle), 0, UnityEngine.Mathf.Cos(angle));



                    var playerVel = new Vector3(playerToKick.rigidbody.velocity.x, 0, playerToKick.rigidbody.velocity.z);

                    //Server.SendServerMessage("playerVel " + playerVel.x + ", " + playerVel.y + ", " + playerVel.z);
                    //Server.SendServerMessage("playerVel " + playerVel.magnitude);

                    //SetPosition(0, -2.0f, 0);
                    SetVelocity(0, 0, 0);
                    rb.velocity = Vector3.zero;


                    var coll = rb.GetComponent<Collider>();
                    coll.material.bounciness = 1f;

                    //var e = 1f;
                    //if (playerVel.magnitude < 1) e = 3f;

                    //var pv = playerVel.magnitude * 0.5f;

                    var speed = playerVel.magnitude;

         
                    rb.AddForce(-delta * 1000f * speed * 0.4f);
                    rb.AddForce(Vector3.up * 1000f * speed * 0.2f);


                    if (speed < 2)
                    {
                        rb.AddForce(Vector3.up * 2500f);

                    }

                    //Server.SendServerMessage("KICK " + angle + " " + kickTime);


                    kickTime = 0.3f;
                    playerKicked = playerToKick;
                }
            }




            //UpdateRigidBodyData();

            var v = GetVelocity();
            SetVelocity(v.x * 0.98f, v.y, v.z * 0.98f);

            SendSync();
        }

        public override bool OnInteract(Player player)
        {
            base.OnInteract(player);

            if(respawnTime <= 0 && player.clientId == Mod.GetLobbyOwnerSteamId())
            {
                respawnTime = 1f;
                SetPosition(0, 0, 0);
                SendSync();
            }

            return false;
        }
    }

   
}
