using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolGame
{
    class Question
    {
        public readonly List<Answer> Answers;
        public readonly string Text;

        public Question(string text_)
        {
            this.Text = text_;
            Answers = new List<Answer>(5);
        }

        public void AddAnswer(Answer a_)
        {
            Answers.Add(a_);
        }
    }
}
