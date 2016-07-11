using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
using System.Data;
using System.Web;
namespace BLL
{
    public class InfoLinkService : BaseService<InfoLink>, IInfoLinkService
    {
        IMasterService masterService = new MasterService();

        IUserService userService = new UserService();
        IPropertyService propertyService = new PropertyService();



        public InfoLink MakeLink(InfoLink prevLink, Transfer info)
        {
            InfoLink current = new InfoLink();
            current.ID = Guid.NewGuid();
            current.UserID = info.UserID;
            current.Status = info.CurrentStatus;
            current.Prev = prevLink.ID;
            current.PropertyID = prevLink.PropertyID;
            current.PropertyType = prevLink.PropertyType;
            current.IsDeleted = false;
            prevLink.Next = current.ID;
            Update(prevLink);
            Add(current);
            return current;
        }

        public InfoLink MakeNextNode(InfoLink prevLink, Transfer info)
        {
            if (prevLink.Next != null)
            {
                prevLink = FindLastNodeByPropertyID((Guid)prevLink.PropertyID);
            }
            InfoLink current = new InfoLink();
            current.ID = Guid.NewGuid();
            current.UserID = info.UserID;
            current.Status = info.CurrentStatus;
            current.Prev = prevLink.ID;
            current.PropertyID = prevLink.PropertyID;
            current.PropertyType = prevLink.PropertyType;
            current.IsDeleted = false;
            prevLink.Next = current.ID;
            Update(prevLink);
            Add(current);
            return current;
        }
        public InfoLink FindLastNodeByStatus(InfoLink Link,string status)
        {
            if (Link.Next != null)
            {
                Link = FindLastNodeByPropertyID((Guid)Link.PropertyID);
            }
            while (Link.Status != status)
            {
                Link = Find((Guid)Link.Prev);
            }
            return Link;
        }
        public void SetActive(Guid LinkID)
        {
            InfoLink current = Find(LinkID);
            while (current.Next != null)
            {
                current = Find((Guid)current.Next);
            }
            List<Guid?> users = new List<Guid?>();
            current.Active = true;
            Update(current);
            users.Add(current.UserID);
            while (current.Prev != null)
            {
                current = Find((Guid)current.Prev);
                if (users.Contains<Guid?>(current.UserID))
                {
                    current.Active = false;
                    Update(current);
                }
                else
                {
                    users.Add(current.UserID);
                    current.Active = true;
                    Update(current);
                }
            }
        }

        public InfoLink FindLastNodeByPropertyID(Guid PropertyID)
        {
            var sql="Select * from InfoLink Where PropertyID='"+PropertyID+"' and Next is null";
            var result = SqlQuery(sql).FirstOrDefault();
            return result;
        }

        public InfoLink FindLastLink(Guid PropertyID)
        {
            var sql = "Select * from InfoLink Where PropertyID='" + PropertyID + "' and Next is null";
            var result = SqlQuery(sql).FirstOrDefault();
            return result;
        }

        public void Add(Guid propertyID, string propertyType, Guid? prev = null, Guid? next = null)
        {
            
            InfoLink info = new InfoLink();
            info.ID = Guid.NewGuid();
            info.Status = "暂扣";
            if (propertyType == "物品")
            {
                var item = propertyService.Find(propertyID);
                info.UserID = userService.FindByMasterID((Guid)item.MasterID).ID;
            }
            if (propertyType == "款项")
            {
                var item = propertyService.Find(propertyID);
                info.UserID = userService.FindByMasterID((Guid)item.MasterID).ID;
            }
            //info.UserID = ((BaseInfo)HttpContext.Current.Session["User"]).user.ID;
            info.Prev = prev;
            info.Next = next;
            info.PropertyID = propertyID;
            info.PropertyType = propertyType;
            info.Active = true;
            info.IsDeleted = false;
            Add(info);
        }
    }
}
