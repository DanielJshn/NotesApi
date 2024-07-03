namespace apief
{
    public interface IEfRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
        public Account? GetSingleUser(int userId);

    }
}