using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
namespace zjw
{
    [Serializable]
    public class Originator<T>
    {
        private T _state;

        public T State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                Console.WriteLine("State = " + _state);
            }
        }

        public Memento<T> CreateMemento()
        {
            return (new Memento<T>(_state));
        }

        public void SetMemento(Memento<T> memento)
        {
            Console.WriteLine("Restoring state...");
            State = memento.State;
        }
    }
}