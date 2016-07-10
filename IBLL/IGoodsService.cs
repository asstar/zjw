using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace IBLL
{
    public interface IGoodsService : IBaseService<Goods>
    {
        void Add(Goods item);
        void Update(Goods item);
        void Delete(Goods item);
    }
}
