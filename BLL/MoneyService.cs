using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class MoneyService : BaseService<Money>, IMoneyService
    {
        IPropertyService propertyService = new PropertyService();
        public void Add(Money item)
        {
            propertyService.Add(item);
            base.Add(item);
        }
        public void Update(Money item)
        {
            propertyService.Update(item);
            base.Update(item);
        }
        public void Delete(Money item)
        {
            propertyService.Delete(item);
            base.Delete(item);
        }
    }
}
