using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using NuGet.Protocol;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BlogsController : ControllerBase
    {
        private readonly IAllService<Blog> _blogService;
        

        public BlogsController(IAllService<Blog> blogService)
        {
            _blogService = blogService;
            

        }

        // GET: api/Blogs
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBlogs()
        {
            var blogs = await _blogService.GetAllAsync();

            if (blogs == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No Blogs in database");
            }

            return StatusCode(StatusCodes.Status200OK, blogs);
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetBlog(int id)
        {
            Blog blog = await _blogService.GetByIdAsync(id, true);

            if (blog == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Blog found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, blog);
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> PutBlog(int id, Blog blog)
        {
            if (id != blog.Id)
            {
                return BadRequest();
            }

            Blog dbBlog = await _blogService.UpdateAsync(blog);

            if (dbBlog == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{blog.Title} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Blogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> PostBlog(Blog blog)
        {
            var dbBlog = await _blogService.AddAsync(blog);

            if (dbBlog == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{blog.Title} could not be added.");
            }

            return CreatedAtAction("GetBlog", new { id = blog.Id }, blog);
        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _blogService.GetByIdAsync(id, false);
            (bool status, string message) = await _blogService.DeleteAsync(blog);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, blog);
        }
    }
}
