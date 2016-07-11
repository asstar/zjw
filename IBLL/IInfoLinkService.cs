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
        InfoLink FindLastNodeByPropertyID(Guid PropertyID);
        void SetActive(Guid LinkID);
        InfoLink FindLastLink(Guid PropertyID);
        InfoLink MakeLink(InfoLink prevLink, Transfer info);
        InfoLink MakeNextNode(InfoLink prevLink, Transfer info);
        InfoLink FindLastNodeByStatus(InfoLink Link, string status);
    }
}
