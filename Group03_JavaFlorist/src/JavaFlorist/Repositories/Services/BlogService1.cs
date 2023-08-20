using Dapper;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.Data.SqlClient;

namespace JavaFlorist.Repositories.Services
{
    public class BlogService1 : IBlogService1
    {
        private readonly IConfiguration _configuration;

        public BlogService1(IConfiguration configuration)
        {
            // Injecting Iconfiguration to the contructor of the product repository
            _configuration = configuration;
        }

        /// <summary>
        /// This method adds a new product to the database using Dapper
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>int</returns>
        public async Task<int> AddAsync(Blog entity)
        {
            // Set the time to the current moment
            entity.PublishDate = DateTime.Now;

            // Basic SQL statement to insert a product into the products table
            var sql = "INSERT INTO Blog (Title, Content, PublishDate, UserId, [StatusId]) VALUES (@Title, @Content, @PublishDate, @UserId, @[StatusId])";


            // Sing the Dapper Connection string we open a connection to the database
            using (var connection = new SqlConnection(_configuration.GetConnectionString("mycon")))
            {
                connection.Open();

                // Pass the product object and the SQL statement into the Execute function (async)
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

        /// <summary>
        /// This method deleted a product specified by an ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>int</returns>
        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Blog WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("mycon")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        /// <summary>
        /// This method returns all products in database in a list object
        /// </summary>
        /// <returns>IEnumerable Product</returns>
        public async Task<IReadOnlyList<Blog>> GetAllAsync()
        {
            var sql = "SELECT * FROM Blog";
            /* var sql = "SELECT Blog.Id, Blog.Title, Blog.Content, Blog.PublishDate, " +
           "Status.Id AS StatusId, Status.[Type] AS StatusType, " +
           "User.Id AS UserId, User.FullName AS UserFullName " +
           "FROM Blog " +
           "JOIN Status ON Blog.StatusId = Status.Id " +
           "JOIN User ON Blog.UserId = User.Id";*/

            using (var connection = new SqlConnection(_configuration.GetConnectionString("mycon")))
            {
                connection.Open();
                /*var result = await connection.QueryAsync<Blog, Status, User, Blog>(
                sql,
                (blog, status, user) =>
                {
                    blog.Status = status;
                    blog.User = user;
                    return blog;
                },
                splitOn: "StatusId,UserId"
            );*/
                // Map all products from database to a list of type Product defined in Models.
                // this is done by using Async method which is also used on the GetByIdAsync
                var result = await connection.QueryAsync<Blog>(sql);
                return result.ToList();
            }
        }

        /// <summary>
        /// This method returns a single product specified by an ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product</returns>
        public async Task<Blog> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Blog WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("mycon")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Blog>(sql, new { Id = id });
                return result;
            }
        }

        /// <summary>
        /// This method updates a product specified by an ID. Added column won't be touched.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>int</returns>
        public async Task<int> UpdateAsync(Blog entity)
        {
            entity.PublishDate = DateTime.Now;
            var sql = "UPDATE Blog SET Title = @Title, Content = @Content, PublishDate = @PublishDate, UserId = @UserId, [StatusId] = @[StatusId]  WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("mycon")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }
    }
}
