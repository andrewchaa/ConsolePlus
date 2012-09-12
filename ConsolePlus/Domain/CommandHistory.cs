using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsolePlus.Domain
{
    public class CommandHistory
    {
        private readonly IList<string> _history;

        public CommandHistory()
        {
            _history = new List<string>();
        }

        public int Count
        {
            get { return _history.Count; }
        }

        public void Add(string command)
        {
            _history.Insert(0, command.Replace(Environment.NewLine, string.Empty));
        }

        public string Get()
        {
            string first = _history.First();
            _history.Remove(first);
            _history.Add(first);

            return first;
        }
    }
}
