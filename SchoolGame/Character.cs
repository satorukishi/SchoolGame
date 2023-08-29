using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SchoolGame
{
    class Character
    {
        public int Life;
        public CharacterSituation Situation;
        public CharacterAction Action;
        public CharacterName Name;
        private CharacterRunning LastRun;

        public Character()
            : this(4, CharacterSituation.Small)
        { }

        public Character(int life_, CharacterSituation situation_)
        {
            Life = life_;
            Situation = situation_;
        }

        public Image Show()
        {
            Image img;
            if (Action == CharacterAction.Run)
            {
                switch (LastRun)
                {
                    case CharacterRunning.LeftFoot:
                        LastRun = CharacterRunning.OnFloor;
                        img = Image.FromFile(String.Format(@"img\{0}_Sit{1}_Act{2}_3.PNG", Name, (int)Situation, (int)Action));
                        break;
                    case CharacterRunning.OnFloor:
                        LastRun = CharacterRunning.RightFoot;
                        img = Image.FromFile(String.Format(@"img\{0}_Sit{1}_Act{2}_2.PNG", Name, (int)Situation, (int)Action));
                        break;
                    case CharacterRunning.RightFoot:
                        LastRun = CharacterRunning.LeftFoot;
                        img = Image.FromFile(String.Format(@"img\{0}_Sit{1}_Act{2}_1.PNG", Name, (int)Situation, (int)Action));
                        break;
                    default:
                        img = Image.FromFile(String.Format(@"img\{0}_Sit{1}_Act{2}_1.PNG", Name, (int)Situation, (int)Action));
                        LastRun = CharacterRunning.RightFoot;
                        break;
                }
            }
            else
            {
                img = Image.FromFile(String.Format(@"img\{0}_Sit{1}_Act{2}.PNG", Name, (int)Situation, (int)Action));
            }
            return img;
        }

        private enum CharacterRunning
        {
            RightFoot = 1,
            OnFloor = 2,
            LeftFoot = 3,
        }
    }

    enum CharacterName
    {
        Mario,
        Luigi,
        Peach
    }

    enum CharacterSituation
    {
        Small = 0,
        Big = 1,
        Flower = 2,
        Dead = 3
    }

    enum CharacterAction
    {
        Stop = 0,
        Jump = 1,
        Run = 2
    }
}
