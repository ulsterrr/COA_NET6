using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class DapperBranchRepository : IRepository<Branch>
    {
        private readonly IDbConnection _dbConnection;

        public DapperBranchRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddAsync(Branch entity)
        {
            var sql = "INSERT INTO Branches (Name) VALUES (@Name)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public void Delete(Branch entity)
        {
            var sql = "DELETE FROM Branches WHERE Id = @Id";
            _dbConnection.Execute(sql, new { entity.Id });
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            var sql = "SELECT * FROM Branches";
            return await _dbConnection.QueryAsync<Branch>(sql);
        }

        public async Task<Branch> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Branches WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Branch>(sql, new { Id = id });
        }

        public void Update(Branch entity)
        {
            var sql = "UPDATE Branches SET Name = @Name WHERE Id = @Id";
            _dbConnection.Execute(sql, entity);
        }
    }
}
