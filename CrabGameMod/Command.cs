using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrabGameMod
{
    class Command
    {
        private string[] m_Args;
    
        public string GetCmd()
        {
            return m_Args[0].Replace("!", "");
        }

        public Command(string str)
        {
            m_Args = str.Split(' ');
        }

        public int GetArgInt(int index)
        {
            return Int32.Parse(m_Args[index + 1]);
        }

        public string GetArgString(int index)
        {
            return m_Args[index + 1];
        }

        public ulong GetArgUlong(int index)
        {
            return (ulong)GetArgInt(index);
        }

        public float GetArgFloat(int index)
        {
            return float.Parse(GetArgString(index));
        }


        public bool HasArg(int index)
        {
            return (m_Args.Length - 1) >= index + 1;
        }
    }

}