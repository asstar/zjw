using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
using System.Data;
using System.Reflection;
namespace BLL
{
    public class MasterService : BaseService<Master>, IMasterService
    {
        public void Add(object item)
        {
            var target = new Master();
            Type itemType = item.GetType();
            PropertyInfo[] pi = itemType.GetProperties();
            foreach (PropertyInfo property in pi)
            {
                Type targetType = target.GetType();
                PropertyInfo[] dt = targetType.GetProperties();
                foreach (PropertyInfo d in dt)
                {
                    if (d.Name == property.Name)
                    {
                        d.SetValue(target, property.GetValue(item));
                    }
                }
            }
            base.Add(target);
        }
        public void Update(object item)
        {
            Type itemType = item.GetType();
            var target = new Master();
            PropertyInfo[] pi = itemType.GetProperties();
            foreach (PropertyInfo property in pi)
            {
                Type targetType = target.GetType();
                PropertyInfo[] dt = targetType.GetProperties();
                foreach (PropertyInfo d in dt)
                {
                    if (d.Name == property.Name)
                    {
                        d.SetValue(target, property.GetValue(item));
                    }
                }
            }
            base.Update(target);
        }
        public void Delete(object item)
        {
            Type itemType = item.GetType();
            var target = new Master();
            PropertyInfo[] pi = itemType.GetProperties();
            foreach (PropertyInfo property in pi)
            {
                Type targetType = target.GetType();
                PropertyInfo[] dt = targetType.GetProperties();
                foreach (PropertyInfo d in dt)
                {
                    if (d.Name == property.Name)
                    {
                        d.SetValue(target, property.GetValue(item));
                    }
                }
            }
            base.Delete(target);
        }
    }
}
