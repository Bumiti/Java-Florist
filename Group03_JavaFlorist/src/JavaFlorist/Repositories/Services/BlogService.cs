using AutoMapper;
using AutoMapper.QueryableExtensions;
using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JavaFlorist.Repositories.Services
{
    public class BlogService :IAllService<Blog>
    {
        private readonly JavaFloristDbContext _context;

        public BlogService(JavaFloristDbContext context)
        {
            _context = context;
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            try
            {
                return await _context.Blogs
                    .Include(b => b.Status).Include(b => b.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Blog> GetByIdAsync(int id, bool includes = false)
        {
            try
            {
                if (includes == true) // blogs should be included
                {
                    return await _context.Blogs.Include(b => b.Status).Include(b => b.User)
                        .FirstOrDefaultAsync(i => i.Id == id);
                }
                // Blogs should be excluded
                return await _context.Blogs.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Blog> AddAsync(Blog blog)
        {
            try
            {
                await _context.Blogs.AddAsync(blog);
                await _context.SaveChangesAsync();
                return blog; // Auto ID from DB
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Blog> UpdateAsync(Blog blog)
        {
            try
            {
                _context.Entry(blog).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return blog;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteAsync(Blog blog)
        {
            try
            {
                var dbBlog = await _context.Blogs.FindAsync(blog.Id);

                if (dbBlog == null)
                {
                    return (false, "Blog could not be found");
                }

                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();

                return (true, "Blog got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }
    }
}
