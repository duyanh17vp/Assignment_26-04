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
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        public BookController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet] 
        public IActionResult GetAllBooks() 
        { 
            try 
            { 
                var books = _repository.Book.GetAllBooks(); 
                return Ok(books); 
            }
            catch (Exception) 
            {
                // throw ex;
                return StatusCode(500, "Internal server error");
            } 
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet("{id}", Name = "BookById")]
        public ActionResult<string> GetBookById(int id)
        {
            try
            {
                var book = _repository.Book.GetBookById(id);
                if (book == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(book);
                }
            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet("{id}/requestorders")]
        public ActionResult<string> GetBookWithDetails(int id)
        {
            try
            {
                var book = _repository.Book.GetBookWithDetails(id);
                if (book == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(book);
                }
            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser")]
        [HttpPost]
        public async Task<ActionResult<Book>>CreateBook([FromBody]Book book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest("Book object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid book object sent from client.");
                }
                await _repository.Book.Create(book);
                _repository.Save();
                return CreatedAtRoute("BookById", new { id = book.Id }, book);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "SuperUser")]
        [HttpPut("{id}")]
        public IActionResult PutBook(int id,[FromBody]Book book)
        {
            try
            {
                if (book == null)
                {
                    return BadRequest("Book object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid book object sent from client.");
                }
                var bookEntity = _repository.Book.GetBookById(id);
                if (bookEntity == null)
                {
                    return NotFound();
                }
                bookEntity.Name = book.Name;
                bookEntity.Title = book.Title;
                bookEntity.CategoryId = book.CategoryId;
                _repository.Book.Update(bookEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "SuperUser")]
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                var bookEntity = _repository.Book.GetBookById(id);
                if (bookEntity == null)
                {
                    return NotFound();
                }
                _repository.Book.Delete(bookEntity);
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
