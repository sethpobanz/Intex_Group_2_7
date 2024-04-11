namespace SecurityLab.Models
{
    public class EFUserRecRepository : IUserRecInterface
    {
        private readonly PobanzTestDbContext _context;

        public EFUserRecRepository(PobanzTestDbContext context)
        {
            _context = context;
        }

        public IQueryable<UserPipeline> UserPipelines => _context.UserPipelines;
    }
}
