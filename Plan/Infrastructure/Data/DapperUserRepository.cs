using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class DapperUserRepository : IRepository<User>
    {
        private readonly IDbConnection _dbConnection;

        public DapperUserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddAsync(User entity)
        {
            var sql = "INSERT INTO Users (Username, PasswordHash, Email) VALUES (@Username, @PasswordHash, @Email)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public void Delete(User entity)
        {
            var sql = "DELETE FROM Users WHERE Id = @Id";
            _dbConnection.Execute(sql, new { entity.Id });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var sql = "SELECT * FROM Users";
            return await _dbConnection.QueryAsync<User>(sql);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public void Update(User entity)
        {
            var sql = "UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, Email = @Email WHERE Id = @Id";
            _dbConnection.Execute(sql, entity);
        }
    }
}
