using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class GoodsService : BaseService<Goods>, IGoodsService
    {
        IPropertyService propertyService = new PropertyService();
        public void Add(Goods item)
        {
            propertyService.Add(item);
            base.Add(item);
        }
        public void Update(Goods item)
        {
            propertyService.Update(item);
            base.Update(item);
        }
        public void Delete(Goods item)
        {
            propertyService.Delete(item);
            base.Delete(item);
        }
    }
}
