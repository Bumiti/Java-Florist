using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class DashBoard : IDashBoard
    {
        private static List<RevenueFilterByFlorist> revenueFilterList = new List<RevenueFilterByFlorist>();
        private readonly JavaFloristDbContext _context;

        public DashBoard(JavaFloristDbContext context)
        {
            _context = context;
        }
        public async Task<List<RevenueFilterByFlorist>> GetFilterByFlorists()
        {
            var floristOrderDetails = await _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Bouquet)
                .ThenInclude(b => b.Florist)
                .ToListAsync();

            // Sử dụng LINQ để tính toán tổng số lượng đơn hàng và tổng số tiền cho mỗi FloristId
            var groupedOrders = floristOrderDetails.GroupBy(od => od.Bouquet.FloristId);
            var revenueFilterList = new List<RevenueFilterByFlorist>();

            foreach (var group in groupedOrders)
            {
                var floristId = group.Key;
                var orderQuantity = group.Count();
                var totalMoney = group.Sum(od => od.Order.Amount);

                // Lấy tên florist từ thông tin Bouquet
                var floristName = group.First().Bouquet.Florist.Name;
                var floristLogo = group.First().Bouquet.Florist.Logo;
                var bouquetCount = _context.Bouquets.Count();
                var userCount = _context.Users.Count();
                var floristCount = _context.Florists.Count();

                // Tạo đối tượng RevenueFilterByFlorist và thêm vào danh sách kết quả
                var newItem = new RevenueFilterByFlorist
                {
                    FloristId = floristId,
                    FloristName = floristName,
                    FloristLogo = floristLogo, // Add the FloristName property
                    OrderQuantity = orderQuantity,
                    BouquetQuantity= bouquetCount,
                    UserQuantity= userCount,
                    FloristQuantity= floristCount,
                    TotalMoney = totalMoney,
                };
                revenueFilterList.Add(newItem);
            }

            return revenueFilterList;
        }


    }
}
