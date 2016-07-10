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
            InfoLink last = prevLink;
            if (prevLink.Next != null)
            {
                last = FindLastNodeByPropertyID((Guid)prevLink.PropertyID);
            }

            InfoLink current = new InfoLink();
            current.ID = Guid.NewGuid();
            current.UserID = info.UserID;
            current.Status = info.CurrentStatus;
            current.Prev = last.ID;
            current.PropertyID = prevLink.PropertyID;
            current.PropertyType = prevLink.PropertyType;
            current.IsDeleted = false;
            //dest.Active = true;
            last.Next = current.ID;
            if (info.PrevStatus != null)
            {
                prevLink.Status = info.PrevStatus;
            }
            Update(prevLink);
            Update(last);
            Add(current);
            return current;
        }

        public void MakeOutLink(InfoLink current,Transfer info)
        {
            if (current.Prev != null)
            {

                var prevNode = Find((Guid)current.Prev);
                while (prevNode.Status == "提交审理")
                {
                    prevNode = Find((Guid)prevNode.Prev);
                }
                current.Status = info.CurrentStatus;

                    prevNode.Status = info.PrevStatus;
                    Update(prevNode);
                
                
                Update(current);
                
            }
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

        public InfoLink FindLastTransferLink(Guid PropertyID)
        {
            var sql = "Select * from InfoLink Where PropertyID='" + PropertyID + "' and Next is null";
            var result = SqlQuery(sql).FirstOrDefault();
            while(result.Status=="提交审理"){
                result = Find((Guid)result.Prev);
            }
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
