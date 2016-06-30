using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Model;
using IBLL;
using System.Reflection;
using System.Diagnostics;
using System.Data;
namespace BLL
{
    public class BaseService<T> : IBaseService<T> where T : class, new()
    {

        public void Add(T item)
        {
            using (var db = new DBEntities())
            {
                Type itemType = typeof(T);
                PropertyInfo[] pi = itemType.GetProperties();
                foreach (PropertyInfo property in pi)
                {
                    //给属性赋值
                    if (property.Name == "TimeStamp")
                    {
                        property.SetValue(item, DateTime.Now);
                    }
                }
                db.Set<T>().Add(item);
                db.SaveChanges();
            }
        }
        public void Update(T item)
        {
            using (var db = new DBEntities())
            {
                Type itemType = typeof(T);
                PropertyInfo[] pi = itemType.GetProperties();
                foreach (PropertyInfo property in pi)
                {
                    //给属性赋值
                    if (property.Name == "TimeStamp")
                    {
                        property.SetValue(item, DateTime.Now);
                    }
                }
                db.Entry(item).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void Delete(Guid guid)
        {
            using (var db = new DBEntities())
            {

                T item = Find(guid);
                Type itemType = typeof(T);
                PropertyInfo[] pi = itemType.GetProperties();
                foreach (PropertyInfo property in pi)
                {
                    //给属性赋值
                    if (property.Name == "IsDeleted")
                    {
                        property.SetValue(item, true);
                    }
                }
                Update(item);
                //db.Entry(item).State = EntityState.Deleted;
                //db.SaveChanges();

            }
        }

        public void Delete(T item)
        {
            using (var db = new DBEntities())
            {
                db.Entry(item).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public IEnumerable<T> SqlQuery(string sql)
        {
            using (var db = new DBEntities())
            {
                //new LogTools().Insert("查询", sql);
                return db.Database.SqlQuery<T>(sql).ToList();
            }
        }
        public int SqlCommand(string sql)
        {
            using (var db = new DBEntities())
            {
                return db.Database.ExecuteSqlCommand(sql);
            }
        }
        public T Find(Guid guid)
        {
            using (var db = new DBEntities())
            {
                return db.Set<T>().Find(guid);//db.ClueInfo.Find(guid);
            }
        }

        public List<T> List(ListModel listModel, String TableName, ref int total)
        {
            using (var db = new DBEntities())
            {
                string sql = "select * FROM " + TableName + " where isDeleted=false " + listModel.StatusString + listModel.QueryString + listModel.AuthString + " order by " + listModel.Sort + " " + listModel.Direction;
                IEnumerable<T> temp = SqlQuery(sql);
                total = temp.Count();
                var users = temp.Skip<T>((listModel.PageIndex - 1) * listModel.PageSize).Take<T>(listModel.PageSize);
                var result = users.ToList<T>();
                return result;
            }
        }
    }
}
