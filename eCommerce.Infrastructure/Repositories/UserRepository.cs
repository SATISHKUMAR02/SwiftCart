using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Infrastructure.DBContext;

namespace eCommerce.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly DapperDbContext _dbContext;
        public UserRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApplicationUser?> AddUser(ApplicationUser user)
        {
            // generate new userId for user 
            user.UserId = Guid.NewGuid();
             // since we are using Dapper we will be using SQL Queries 

            return user;
            
        }

        public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? Email, string? Password)
        {
            return new ApplicationUser()
            {
                UserId = Guid.NewGuid(),
                Email = Email,
                Password = Password,
                PersonName = "Dummy ",
                Gender = GenderOptions.Male.ToString(),
            };
        }
    }
}
