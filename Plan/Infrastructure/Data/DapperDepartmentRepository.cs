using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class DapperDepartmentRepository : IRepository<Department>
    {
        private readonly IDbConnection _dbConnection;

        public DapperDepartmentRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddAsync(Department entity)
        {
            var sql = "INSERT INTO Departments (Name, BranchId) VALUES (@Name, @BranchId)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public void Delete(Department entity)
        {
            var sql = "DELETE FROM Departments WHERE Id = @Id";
            _dbConnection.Execute(sql, new { entity.Id });
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var sql = "SELECT * FROM Departments";
            return await _dbConnection.QueryAsync<Department>(sql);
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Departments WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Department>(sql, new { Id = id });
        }

        public void Update(Department entity)
        {
            var sql = "UPDATE Departments SET Name = @Name, BranchId = @BranchId WHERE Id = @Id";
            _dbConnection.Execute(sql, entity);
        }
    }
}
