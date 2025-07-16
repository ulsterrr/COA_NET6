using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class DapperPermissionRepository : IRepository<Permission>
    {
        private readonly IDbConnection _dbConnection;

        public DapperPermissionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddAsync(Permission entity)
        {
            var sql = "INSERT INTO Permissions (Name) VALUES (@Name)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public void Delete(Permission entity)
        {
            var sql = "DELETE FROM Permissions WHERE Id = @Id";
            _dbConnection.Execute(sql, new { entity.Id });
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            var sql = "SELECT * FROM Permissions";
            return await _dbConnection.QueryAsync<Permission>(sql);
        }

        public async Task<Permission> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Permissions WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Permission>(sql, new { Id = id });
        }

        public void Update(Permission entity)
        {
            var sql = "UPDATE Permissions SET Name = @Name WHERE Id = @Id";
            _dbConnection.Execute(sql, entity);
        }
    }
}
