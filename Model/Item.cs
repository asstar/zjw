﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Item
    {
        public Guid id { get; set; }
        public string text { get; set; }
    }
}
