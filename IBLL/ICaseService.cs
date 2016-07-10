using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace IBLL
{
    public interface ICaseService : IBaseService<Case>
    {
        void Add(Case item);
        void Update(Case item);
        void Delete(Case item);
    }
}
