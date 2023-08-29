using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolGame
{
    public class Team
    {
        public string Name;
        public string[] Players;
        public int Turn = -1;
        public bool IsTurnChanged = false;

        public Team(string name_, string[] players_)
        {
            this.Name = name_;
            this.Players = players_;
        }

        public void NextTurn()
        {
            if (Turn < Players.Length - 1)
            {
                Turn++;
            }
            else
            {
                Turn = 0;
            }
            IsTurnChanged = true;
        }

        public void RestartTurns()
        {
            Turn = -1;
            IsTurnChanged = true;
        }
    }
}
