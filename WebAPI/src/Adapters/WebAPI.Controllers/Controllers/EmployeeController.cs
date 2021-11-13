﻿using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructure.Interfaces.Interfaces;
using WebAPI.UserCases.Cases.Employees.Queries.EmployeeList;
using WebAPI.Utils.Constants;

namespace WebAPI.Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly ICrudRepository<Employee> _empRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public EmployeeController(
            ICrudRepository<Employee> empRepository, IWebHostEnvironment env, ILogger logger)
        {
            _empRepository = empRepository;
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// Gets the list of Employee
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /employee
        /// </remarks>
        /// <returns>Returns EmployeeListViewModel</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        // [Authorize (Roles = "Manager")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EmployeeListViewModel>> GetEmployeeList()
        {
            var query = new EmployeeListQuery() { EmployeeId = EmployeeId };
            var view = await Mediator.Send(query);
            return Ok(view);
        }
        
        [HttpPost]
        [Authorize (Roles = "Manager")]
        public JsonResult Post(Employee emp)
        {
            _empRepository.Create(emp);
            return new JsonResult("Created Successfully");
        }

        [HttpPut]
        [Authorize (Roles = "Manager")]
        public JsonResult Put(Employee emp)
        {
            var success = true;
            try
            {
                _empRepository.Update(emp);
            }
            catch (Exception)
            {
                success = false;
            }
            return success
                ? new JsonResult("Update successful")
                : new JsonResult("Update was not successful");
        }
        
        [HttpDelete("{id}")]
        [Authorize (Roles = "Manager")]
        public JsonResult Delete(int id)
        {
            var success = true;
            try
            {
                _empRepository.Delete(id);
            }
            catch (Exception)
            {
                success = false;
            }
            return success
                ? new JsonResult("Delete successful")
                : new JsonResult("Delete was not successful");
        }
        
        [HttpPost]
        [Authorize (Roles = "Manager")]
        [Route("UploadPhoto")]
        public JsonResult UploadPhoto()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                var filename = postedFile.FileName;
                var selectPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(selectPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
        
        [HttpPost]
        [Authorize (Roles = "Manager")]
        [Route("{Id}/UpdatePhoto")]
        public JsonResult UpdatePhoto(int id)
        {
            try
            {
                var photoName = _empRepository.GetFileName(id);
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                var filename = postedFile.FileName;
                var selectPath = _env.ContentRootPath + "/Photos/" + filename;
                var storagePath = PathTypes.StoragePath + photoName;

                if (System.IO.File.Exists(selectPath))
                    System.IO.File.Copy(storagePath, selectPath, true);

                using (var stream = new FileStream(selectPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    if (System.IO.File.Exists(selectPath))
                        System.IO.File.Delete(storagePath);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
        
        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            return new JsonResult(_empRepository.ReadAll());
        }
    }
}