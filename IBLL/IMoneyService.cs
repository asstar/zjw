﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace IBLL
{
    public interface IMoneyService : IBaseService<Money>
    {
        void Add(Money item);
        void Update(Money item);
        void Delete(Money item);
    }
}
