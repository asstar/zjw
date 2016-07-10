using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
using System.Data;
namespace BLL
{
    public class CaseService: BaseService<Case>, ICaseService
    {
        IMasterService masterService = new MasterService();
        public void Add(Case item)
        {
            masterService.Add(item);
            base.Add(item);
        }
        public void Update(Case item)
        {
            masterService.Update(item);
            base.Update(item);
        }
        public void Delete(Case item)
        {
            masterService.Delete(item);
            base.Delete(item);
        }
    }
}
