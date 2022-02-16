using ResturantDewanSoft.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResturantDewanSoft.Repository
{
    class UnitOfWork : IDisposable
    {
        DewanSoftEntities dbEntity = new DewanSoftEntities();

        public Repository<Entity> GetRepositoryInstance <Entity>() where Entity : class 
        {
            return new Repository<Entity>(dbEntity);

        }

        public void SaveChanges()
        {
            dbEntity.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbEntity.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

    }
}
