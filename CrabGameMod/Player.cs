using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CrabGameMod
{
    public class Player
    {
        public ulong clientId { get; private set; }
        public int numberId { get; private set; }
        public int uid { get; private set; }
        public string username { get; private set; }

        public bool canBeBanned = false;

        public bool canMessageBypass = false;
        public bool dontStreamMessage = false;
        public bool isActive = false;
        public bool autoDie = false;

        public Rigidbody rigidbody = null;

        public Vector3 position = Vector3.zero;

        private bool isAlive = false;

        public float respawnTime = 0;
        public int kills = 0;

        public Player(ulong clientId, int numberId)
        {
            this.clientId = clientId;
            this.numberId = numberId;
            this.uid = MonoBehaviourPublicCSDi2UIInstObUIloDiUnique.steamIdToUID[clientId];
        }

        public void SetAlive(bool alive)
        {
            if(isAlive != alive)
            {
                isAlive = alive;

                if(isAlive)
                {
                    OnSpawn();
                } else
                {
                    OnDie();
                }
            }
        }

        public void OnSpawn()
        {
            
        }

        public void OnDie()
        {
            Server.SendServerMessage("die");


            //Server.SendServerMessage("die, respawn 1s");
            //respawnTime = 1f;
        }

        public void Update()
        {
            if(respawnTime > 0)
            {
                respawnTime -= UnityEngine.Time.deltaTime;

                //Server.SendServerMessage("t " + respawnTime);

                if(respawnTime < 0)
                {
                    respawnTime = 0;

                    //Server.SendServerMessage("r");

                    Server.RespawnPlayer(this);
                }
            }
        }

        

        public string GetTestMessage()
        {
            var str = username + " (#" + (uid + 1) + ") [" + numberId + "]" + ", v=" + dontStreamMessage;

            return str;
        }

        public void SetUsername(string username)
        {
            this.username = username;
        }

        public string GetSelector()
        {
            return "#" + (uid + 1); ;
        }

        public void Respawn(Vector3 position)
        {
            MonoBehaviourPublicInInUnique.RespawnPlayer(clientId, position);
        }
    }
}
