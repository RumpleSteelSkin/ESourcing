using ESourcing.Core.Entities;
using ESourcing.Core.Repositories;
using ESourcing.Infrastructure.Data;
using ESourcing.Infrastructure.Repository.Base;

namespace ESourcing.Infrastructure.Repository;

public class UserRepository(WebAppContext dbContext) : Repository<AppUser>(dbContext), IUserRepository
{
    private readonly WebAppContext _dbContext = dbContext;
}