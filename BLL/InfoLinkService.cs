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
        ICaseService caseService = new CaseService();
        IGiftService giftService = new GiftService();
        IMoneyService moneyService = new MoneyService();
        IUserService userService = new UserService();
        IGoodsService goodsService = new GoodsService();
        public void Add(Guid propertyID, string propertyType, Guid? prev = null, Guid? next = null)
        {
            
            InfoLink info = new InfoLink();
            info.ID = Guid.NewGuid();
            if (propertyType == "物品")
            {
                var item = goodsService.Find(propertyID);
                info.UserID = userService.FindByMasterID((Guid)item.MasterID).ID;
            }
            if (propertyType == "款项")
            {
                var item = moneyService.Find(propertyID);
                info.UserID = userService.FindByMasterID((Guid)item.MasterID).ID;
            }
            //info.UserID = ((BaseInfo)HttpContext.Current.Session["User"]).user.ID;
            info.Prev = prev;
            info.Next = next;
            info.PropertyID = propertyID;
            info.PropertyType = propertyType;
            info.IsDeleted = false;
            Add(info);
        }
    }
}
