using Dapper;
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
    public class UserRepository : IUserRepository
    {
        private readonly DapperDbContext _dbContext;
        public UserRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApplicationUser?> AddUser(ApplicationUser user)
        {
            // generate new userId for user 
            user.UserID = Guid.NewGuid();
            // since we are using Dapper we will be using SQL Queries 
            string query = "INSERT INTO public. \"Users\"(\"UserID\",\"Email\"," +
                "\"PersonName\",\"Gender\",\"Password\")VALUES(@UserID,@Email,@PersonName,@Gender," +
                "@Password)";
            // ExecuteAsync is used to write queries like create update and delete but not
            // select , for select there is another method called QueryAsync

           int rowsaffected =  await _dbContext.DbConnection.ExecuteAsync(query,user);
            if (rowsaffected > 0) {

                return user;
            }
            else
            {
                return null;
            }
            
            
        }


        public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? Email, string? Password)
        {

            string query = "SELECT * FROM public .\"Users\" WHERE \"Email\"=@Email AND" +
                " \"Password\" = @Password";
            var param = new { Email = Email, Password = Password};  
            ApplicationUser? user = await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query,param);
            return user;
            
        }

        public async Task<ApplicationUser?> GetUserByUserID(Guid? userId)
        {
            string query = "Select * from public.\"Users\" where \"UserID\" = @userId";
            var param = new { userId = userId };
            using var connection = _dbContext.DbConnection;
            return await connection.QueryFirstOrDefaultAsync<ApplicationUser>(query,param);
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            string query = "Select * from public.\"Users\"";
            using var connection = _dbContext.DbConnection;
            var result =  await connection.QueryAsync<ApplicationUser>(query);  
            return result.ToList();

        }

        
    }
}
