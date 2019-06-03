using System;
using System.Collections.Generic;
using System.Linq;

namespace BtoFanControl
{
    public class RunningAvg
    {
        private readonly LinkedList<int> _values;
        private readonly int _nrElements;
        public RunningAvg(int nrElements)
        {
            _values = new LinkedList<int>();
            _nrElements = nrElements;
        }

        public void Add(int val)
        {
            _values.AddFirst(val);
            if (_values.Count > _nrElements)
            {
                _values.RemoveLast();
            }
        }

        public int GetAvg()
        {
            return _values.Sum(n => n) / _values.Count;
        }

        public int GetAvg(int nrValues)
        {
            return _values.Take(nrValues).Sum(n => n) / Math.Min(_values.Count, nrValues);
        }
    }
}