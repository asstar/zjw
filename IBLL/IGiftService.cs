using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace IBLL
{
    public interface IGiftService : IBaseService<Gift>
    {
        void Add(Gift item);
        void Update(Gift item);
        void Delete(Gift item);
    }
}
