using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace IBLL
{
    public interface IMasterService : IBaseService<Master>
    {
        void Add(object item);
        void Update(object item);
        void Delete(object item);
    }
}
