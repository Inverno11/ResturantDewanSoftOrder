using ResturantDewanSoft.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResturantDewanSoft.Repository
{
    public class Repository <Entity> where Entity : class
    {

        DbSet<Entity> dbSet;
        DewanSoftEntities dbEntity;

        public Repository(DewanSoftEntities db )
        {
            dbEntity = db;
            dbSet = dbEntity.Set<Entity>();
        }

        public void Add(Entity entity)
        {
            dbSet.Add(entity);
            dbEntity.SaveChanges();
        }



        public IEnumerable<Entity> GetAllRecordes()
        {

            return dbSet.ToList();
        }

        public bool Remove(int id)
        {

            var model = dbSet.Find(id);
            if (model != null)

            {
                dbSet.Remove(model);

                return dbEntity.SaveChanges() > 0 ? true : false;
            }

            else
            {
                return false;

            }

        }


        public void Update(Entity entity) 
        {
            dbSet.Attach(entity);
            dbEntity.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            dbEntity.SaveChanges();
        }
        public int LastId()
        {
            return dbEntity.Customers.Count();
        }

        public Item FindIdByName( string Name) 
        {
          return  dbEntity.Items.FirstOrDefault(e => e.Name == Name);
            
        }


    }

}
