using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SchoolGame
{
    class Stage
    {
        public readonly Color BgColor;

        /// <summary>
        /// A 48 x 48 JPEG image
        /// </summary>
        public readonly Image Floor;

        public readonly StageType Type;

        public Stage() : this(StageType.Ordinary)
        { }

        public Stage(StageType stageType_)
        {
            Type = stageType_;

            Floor = Image.FromFile(String.Format("img\\floor_{0}.jpg", (int)stageType_));
            switch (Type)
            {
                case StageType.Ordinary:
                    BgColor = Color.CornflowerBlue;
                    break;
                case StageType.Boss:
                    BgColor = Color.Black;
                    break;
                default:
                    BgColor = Color.CornflowerBlue;
                    break;
            }
        }
    }

    enum StageType
    {
        Ordinary = 1,
        Underworld = 2,
        Boss = 8
    }
}
