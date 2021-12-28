using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrabGameMod
{
    class VoteSystem
    {
        private static Dictionary<Player, bool> m_PlayersVoted = new Dictionary<Player, bool>();
        private static float m_TimeLeft = 0;

        private static Action m_callbackOnPass;
        private static Action m_callbackOnFail;

        public static float GetTimeLeft()
        {
            return m_TimeLeft;
        }

        public static bool HasAnyVoting()
        {
            return m_TimeLeft != 0;
        }

        public static void StartVoting(int time, Action onPass, Action onFail)
        {
            m_PlayersVoted.Clear();
            m_TimeLeft = time;

            m_callbackOnPass = onPass;
            m_callbackOnFail = onFail;
        }

        public static bool RegisterVote(Player player, bool votedYes)
        {
            if (m_PlayersVoted.ContainsKey(player)) return false;

            m_PlayersVoted.Add(player, votedYes);

            if(m_PlayersVoted.Count == Server.GetPlayers().Count)
            {
                FinishVoting();
            }

            return true;
        }

        public static void Update()
        {
            if(m_TimeLeft > 0)
            {
                m_TimeLeft -= UnityEngine.Time.deltaTime;

                if(m_TimeLeft <= 0)
                {
                    FinishVoting();
                }
            }

        }

        private static void FinishVoting()
        {
            m_TimeLeft = 0;

            var result = CheckVoteResults();

            if (result)
            {
                m_callbackOnPass();
            } else
            {
                m_callbackOnFail();
            }
        }

        private static bool CheckVoteResults()
        {
            int totalVotes = m_PlayersVoted.Count;
            int yesVotes = GetTotalYesVotes();

            //if (yesVotes == Server.GetPlayers().Count) return true;
            if (totalVotes == 0) return false;

            if (yesVotes / totalVotes >= 0.50) return true;

            return false;
        }

        private static int GetTotalYesVotes()
        {
            int yesVotes = 0;

            foreach (var pair in m_PlayersVoted)
            {
                if (pair.Value) yesVotes++;
            }

            return yesVotes;
        }
    }
}
