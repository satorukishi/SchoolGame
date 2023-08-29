using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolGame
{
    class Answer
    {
        public readonly bool IsCorrect;
        public readonly string Text;

        public Answer(bool isCorrect_, string text_)
        {
            IsCorrect = isCorrect_;
            Text = text_;
        }
    }
}
