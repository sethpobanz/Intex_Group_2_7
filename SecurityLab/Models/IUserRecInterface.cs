namespace SecurityLab.Models
{
    public interface IUserRecInterface
    {
        public IQueryable<UserPipeline> UserPipelines { get; }
    }
}
