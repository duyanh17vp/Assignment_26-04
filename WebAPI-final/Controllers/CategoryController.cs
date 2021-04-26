using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        public CategoryController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet] 
        public IActionResult GetAllCategories() 
        { 
            try 
            { 
                var categories = _repository.Category.GetAllCategories(); 
                return Ok(categories); 
            }
            catch (Exception) 
            {
                // throw ex;
                return StatusCode(500, "Internal server error");
            } 
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet("{id}", Name = "CategoryById")]
        public ActionResult<string> GetCategoryById(int id)
        {
            try
            {
                var category = _repository.Category.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(category);
                }
            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet("{id}/books")]
        public ActionResult<string> GetCategoryWithDetails(int id)
        {
            try
            {
                var category = _repository.Category.GetCategoryWithDetails(id);
                if (category == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(category);
                }
            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser")]
        [HttpPost]
        public async Task<ActionResult<Category>>CreateCategory([FromBody]Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Category object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid category object sent from client.");
                }
                await _repository.Category.Create(category);
                _repository.Save();
                return CreatedAtRoute("CategoryById", new { id = category.Id }, category);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser")]
        [HttpPut("{id}")]
        public IActionResult PutCategory(int id,[FromBody]Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Category object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid category object sent from client.");
                }
                var categoryEntity = _repository.Category.GetCategoryById(id);
                if (categoryEntity == null)
                {
                    return NotFound();
                }
                categoryEntity.Name = category.Name;
                _repository.Category.Update(categoryEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser")]
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var categoryEntity = _repository.Category.GetCategoryById(id);
                if (categoryEntity == null)
                {
                    return NotFound();
                }
                _repository.Category.Delete(categoryEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}