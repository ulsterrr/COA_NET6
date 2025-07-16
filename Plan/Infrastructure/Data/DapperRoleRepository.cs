using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class DapperRoleRepository : IRepository<Role>
    {
        private readonly IDbConnection _dbConnection;

        public DapperRoleRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddAsync(Role entity)
        {
            var sql = "INSERT INTO Roles (Name) VALUES (@Name)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public void Delete(Role entity)
        {
            var sql = "DELETE FROM Roles WHERE Id = @Id";
            _dbConnection.Execute(sql, new { entity.Id });
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var sql = "SELECT * FROM Roles";
            return await _dbConnection.QueryAsync<Role>(sql);
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Roles WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Role>(sql, new { Id = id });
        }

        public void Update(Role entity)
        {
            var sql = "UPDATE Roles SET Name = @Name WHERE Id = @Id";
            _dbConnection.Execute(sql, entity);
        }
    }
}
