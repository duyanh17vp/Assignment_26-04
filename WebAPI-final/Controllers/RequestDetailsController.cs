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
    [Route("api/RequestDetails")]
    [ApiController]
    public class RequestDetailsController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        public RequestDetailsController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet] 
        public IActionResult GetAllRequestDetails() 
        { 
            try 
            { 
                var requestDetails = _repository.RequestDetails.GetAllRequestDetails(); 
                return Ok(requestDetails); 
            }
            catch (Exception) 
            {
                // throw ex;
                return StatusCode(500, "Internal server error");
            } 
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet("{id}", Name = "RequestDetailsById")]
        public ActionResult<string> GetRequestDetailsById(int id)
        {
            try
            {
                var requestDetails = _repository.RequestDetails.GetRequestDetailsById(id);
                if (requestDetails == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(requestDetails);
                }
            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpPost]
        public async Task<ActionResult<RequestDetails>>CreateRequestDetails([FromBody]RequestDetails requestDetails)
        {
            try
            {
                if (requestDetails == null)
                {
                    return BadRequest("RequestDetails object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid requestDetails object sent from client.");
                }
                await _repository.RequestDetails.Create(requestDetails);
                _repository.Save();
                return CreatedAtRoute("RequestDetailsById", new { id = requestDetails.Id }, requestDetails);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = "SuperUser")]
        [HttpPut("{id}")]
        public IActionResult PutRequestDetails(int id,[FromBody]RequestDetails requestDetails)
        {
            try
            {
                if (requestDetails == null)
                {
                    return BadRequest("RequestDetails object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid requestDetails object sent from client.");
                }
                var requestDetailsEntity = _repository.RequestDetails.GetRequestDetailsById(id);
                if (requestDetailsEntity == null)
                {
                    return NotFound();
                }
                requestDetailsEntity.BookId = requestDetails.BookId;
                requestDetailsEntity.RequestOrderId = requestDetails.RequestOrderId;
                _repository.RequestDetails.Update(requestDetailsEntity);
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
        public IActionResult DeleteRequestDetails(int id)
        {
            try
            {
                var requestDetailsEntity = _repository.RequestDetails.GetRequestDetailsById(id);
                if (requestDetailsEntity == null)
                {
                    return NotFound();
                }
                _repository.RequestDetails.Delete(requestDetailsEntity);
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