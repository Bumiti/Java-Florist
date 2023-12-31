﻿using JavaFlorist.Models;
using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Florist")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IAllService<Category> _categoryService;

        public CategoriesController(IAllService<Category> categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        [Authorize(Roles = "Admin, Florist")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllAsync();

            if (categories == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No categories in database");
            }

            return StatusCode(StatusCodes.Status200OK, categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategory(int id, bool includeProducts)
        {
            Category category = await _categoryService.GetByIdAsync(id, includeProducts);

            if (category == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Category found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, category);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            Category dbCategory = await _categoryService.UpdateAsync(category);

            if (dbCategory == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{category.Name} could not be updated");
            }

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostCategory(Category category)
        {
            var dbCategory = await _categoryService.AddAsync(category);

            if (dbCategory == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{category.Name} could not be added.");
            }

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id, false);
            (bool status, string message) = await _categoryService.DeleteAsync(category);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, category);
        }
    }
}
