using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class GiftService : BaseService<Gift>, IGiftService
    {
        IMasterService masterService = new MasterService();
        public void Add(Gift item)
        {
            masterService.Add(item);
            base.Add(item);
        }
        public void Update(Gift item)
        {
            masterService.Update(item);
            base.Update(item);
        }
        public void Delete(Gift item)
        {
            masterService.Delete(item);
            base.Delete(item);
        }
    }
}
