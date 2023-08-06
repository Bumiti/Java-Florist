using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JavaFlorist.Data;
using JavaFlorist.Models;
using Microsoft.Data.SqlClient;
using JavaFlorist.Repositories.IServices;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Blogs1Controller : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public Blogs1Controller(IUnitOfWork unitOfWork)
        {
            // Inject Unit Of Work to the contructor of the product controller
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// This endpoint returns all products from the repository
        /// </summary>
        /// <returns>List of product objects</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.Blogs.GetAllAsync();
            return Ok(data);
        }

        /// <summary>
        /// This endpoint returns a single product by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product object</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _unitOfWork.Blogs.GetByIdAsync(id);
            if (data == null) return Ok();
            return Ok(data);
        }

        /// <summary>
        /// This endpoint adds a product to the database based on Product model
        /// </summary>
        /// <param name="blog"></param>
        /// <returns>Status for creation</returns>
        [HttpPost]
        public async Task<IActionResult> Add(Blog blog)
        {
            var data = await _unitOfWork.Blogs.AddAsync(blog);
            return Ok(data);
        }

        /// <summary>
        /// This endpont deletes a product form the database by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status for deletion</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.Blogs.DeleteAsync(id);
            return Ok(data);
        }

        /// <summary>
        /// This endpoint updates a product by ID
        /// </summary>
        /// <param name="blog"></param>
        /// <returns>Status for update</returns>
        [HttpPut]
        public async Task<IActionResult> Update(Blog blog)
        {
            var data = await _unitOfWork.Blogs.UpdateAsync(blog);
            return Ok(data);
        }
    }
}
