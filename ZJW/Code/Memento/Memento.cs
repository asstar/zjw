using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
namespace zjw
{
    [Serializable]
    public class Memento<T>
    {
        private T _state;

        public Memento(T state)
        {
            this._state = state;
        }

        public T State
        {
            get { return _state; }
        }
    }
}