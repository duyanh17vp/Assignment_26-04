using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    [Route("api/RequestOrder")]
    [ApiController]
    public class RequestOrderController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        public RequestOrderController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet] 
        public IActionResult GetAllRequestOrders() 
        { 
            try 
            { 
                var requestOrders = _repository.RequestOrder.GetAllRequestOrders(); 
                return Ok(requestOrders); 
            }
            catch (Exception) 
            {
                // throw ex;
                return StatusCode(500, "Internal server error");
            } 
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet("{id}", Name = "RequestOrderById")]
        public ActionResult<string> GetRequestOrderById(int id)
        {
            try
            {
                var requestOrder = _repository.RequestOrder.GetRequestOrderById(id);
                if (requestOrder == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(requestOrder);
                }
            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpGet("{id}/requestorders")]
        public ActionResult<string> GetRequestOrderWithDetails(int id)
        {
            try
            {
                var requestOrder = _repository.RequestOrder.GetRequestOrderWithDetails(id);
                if (requestOrder == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(requestOrder);
                }
            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpPost]
        public IActionResult CreateRequestOrder(int userId, List<int> booksId)
        {
            try
            {
                if(booksId.Count() > 5)
                {
                    return BadRequest("Can't borrow more than 5 books in a request!");
                }

                var user = _repository.User.GetUserById(userId);
                if(user == null){
                    return BadRequest("User not found!");
                }
                if(user.RequestOrders != null){
                    var thisMonthRequestOrders = user.RequestOrders.Where(ro => ro.DateRequest.Month == DateTime.Now.Month);
                    if(thisMonthRequestOrders.Count() >= 3)
                    {
                        return BadRequest("This month's request limit reached!");
                    }
                }
                //CreateRequest
                var requestOrder = _repository.RequestOrder.CreateRequest(userId,booksId);
                //AddListRequestDetails to Request
                _repository.RequestOrder.AddListRequestDetails(requestOrder.Id,booksId);
                if (requestOrder == null)
                {
                    return BadRequest("RequestOrder object is null");
                }
                _repository.Save();
                return CreatedAtRoute("RequestOrderById", new { id = requestOrder.Id }, requestOrder);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //ApproveRequest
        [Authorize(Roles = "SuperUser")]
        [HttpPost("{id}/Approved")]
        public IActionResult ApproveRequest(int id, int superUserId)
        {
            try
            {
                var requestOrder = _repository.RequestOrder.ChangeStatus(id,superUserId,Status.Approved);
                if (requestOrder == null)
                {
                    return BadRequest("RequestOrder object is null");
                }
                _repository.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //RejectRequest
        [Authorize(Roles = "SuperUser")]
        [HttpPost("{id}/Rejected")]
        public IActionResult RejectRequest(int id, int superUserId)
        {
            try
            {
                var requestOrder = _repository.RequestOrder.ChangeStatus(id,superUserId,Status.Rejected);
                if (requestOrder == null)
                {
                    return BadRequest("RequestOrder object is null");
                }
                _repository.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = "SuperUser, NormalUser")]
        [HttpPut("{id}")]
        public IActionResult PutRequestOrder(int id,[FromBody]RequestOrder requestOrder)
        {
            try
            {
                if (requestOrder == null)
                {
                    return BadRequest("RequestOrder object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid requestOrder object sent from client.");
                }
                var requestOrderEntity = _repository.RequestOrder.GetRequestOrderById(id);
                if (requestOrderEntity == null)
                {
                    return NotFound();
                }
                requestOrderEntity.Status = requestOrder.Status;
                requestOrderEntity.DateRequest = requestOrder.DateRequest;
                requestOrderEntity.DateReturn = requestOrder.DateReturn;
                requestOrderEntity.NormalUserId = requestOrder.NormalUserId;
                requestOrderEntity.SuperUserId = requestOrder.SuperUserId;
                _repository.RequestOrder.Update(requestOrderEntity);
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
        public IActionResult DeleteRequestOrder(int id)
        {
            try
            {
                var requestOrderEntity = _repository.RequestOrder.GetRequestOrderById(id);
                if (requestOrderEntity == null)
                {
                    return NotFound();
                }
                _repository.RequestOrder.Delete(requestOrderEntity);
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