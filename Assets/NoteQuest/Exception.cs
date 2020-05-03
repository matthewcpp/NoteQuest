using System;

namespace NoteQuest
{
    public class Exception : System.Exception
    {
        public Exception(string message)
                : base(message)
        {
        }
    }
}