using Microsoft.AspNetCore.Http.HttpResults;

namespace apief
{

    public class Repository : IEfRepository
    {
        DataContextEF _entityFramework;
        public Repository(DataContextEF entityFramework, IConfiguration config)
        {
            _entityFramework = entityFramework;
            _entityFramework = new DataContextEF(config);
        }





        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }


        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }


        public Account? GetSingleUser(int id)
        {
            Account? user = _entityFramework.Accounts.Find(id);
            return user;
        }



        public void RemoveEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Remove(entityToAdd);
            }
        }
       

    }
}