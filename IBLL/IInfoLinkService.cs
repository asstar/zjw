using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace IBLL
{
    public interface IInfoLinkService : IBaseService<InfoLink>
    {
        void Add(Guid propertyID, string propertyType, Guid? prev = null, Guid? next = null);
    }
}
