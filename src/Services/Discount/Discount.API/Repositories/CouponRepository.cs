using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;
using Dapper;

namespace Discount.API.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IConfiguration _configuration;

        public CouponRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("Insert Into Coupon (ProductName, Description, Amount) Values (@ProductName, @Description, @Amount",
                                                         new { coupon.ProductName, coupon.Description, coupon.Amount });
            if (affected == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("Delete From Coupon Where ProductName = @productName",
                                                         new { productName });
            if (affected == 0)
                return false;
            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("Select * From Coupon Where productname = @productName", new { productName });

            if (coupon == null)
                return new Coupon { ProductName = "No discount", Amount = 0, Description = "No discount desc" };

            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("Update Coupon Set ProductName = @ProductName, Description = @Description, Amount = @Amount Where Id = @Id",
                                                         new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });
            if (affected == 0)
                return false;
            return true;
        }
    }
}

