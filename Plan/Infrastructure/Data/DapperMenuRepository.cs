using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class DapperMenuRepository : IRepository<Menu>
    {
        private readonly IDbConnection _dbConnection;

        public DapperMenuRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddAsync(Menu entity)
        {
            var sql = "INSERT INTO Menus (Title, Url) VALUES (@Title, @Url)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public void Delete(Menu entity)
        {
            var sql = "DELETE FROM Menus WHERE Id = @Id";
            _dbConnection.Execute(sql, new { entity.Id });
        }

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            var sql = "SELECT * FROM Menus";
            return await _dbConnection.QueryAsync<Menu>(sql);
        }

        public async Task<Menu> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Menus WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Menu>(sql, new { Id = id });
        }

        public void Update(Menu entity)
        {
            var sql = "UPDATE Menus SET Title = @Title, Url = @Url WHERE Id = @Id";
            _dbConnection.Execute(sql, entity);
        }
    }
}
