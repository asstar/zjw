using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zjw
{
    [Serializable]
    public class Caretaker<T>
    {
        private Memento<T> _memento;

        public Memento<T> Memento
        {
            get
            {
                return _memento;
            }
            set
            {
                _memento = value;
            }
        }
    }
}