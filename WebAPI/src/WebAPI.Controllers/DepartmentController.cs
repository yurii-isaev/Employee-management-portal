﻿using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.UserCases.Common.Dto;
using WebAPI.UserCases.Requests.Departments.Queries.GetDepartment;
using WebAPI.UserCases.Requests.Departments.Queries.GetDepartmentList;

namespace WebAPI.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        /// <summary>
        /// Gets the list of departments.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /department.
        /// </remarks>
        /// <returns>Returns department list.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">If the user is unauthorized.</response>
        [HttpGet]
        // [Authorize (Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable>> GetDepartmentList()
        {
            return Ok(await Mediator.Send(new GetDepartmentListQuery()));
        }

        /// <summary>
        /// Gets the department by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /department/D34D349E-43B8-429E-BCA4-793C932FD580.
        /// </remarks>
        /// <param name="id">Department id (guid).</param>
        /// <returns>Returns department dto.</returns>
        /// <response code="200">Success.</response>
        /// <response code="401">If the user in unauthorized.</response>
        [HttpGet("{id}")]
        // [Authorize (Roles = "Manager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(Guid id)
        {
            var request = new GetDepartmentQuery {Id = id};
            return Ok(await Mediator.Send(request));
        }

        // [HttpPost]
        // public JsonResult Post(Department dep)
        // {
        //     _depRepository.Create(dep);
        //     return new JsonResult("Created Successfully");
        // }
        //
        // [HttpPut]
        // public JsonResult Put(Department dep)
        // { 
        //     var success = true;
        //     try
        //     {
        //         _depRepository.Update(dep);
        //     }
        //     catch (Exception)
        //     {
        //         success = false;
        //     }
        //     return success
        //         ? new JsonResult("Update successful")
        //         : new JsonResult("Update was not successful");
        // }
        //
        // [HttpDelete("{id}")]
        // public JsonResult Delete(Guid id)
        // {
        //     var success = true;
        //     try
        //     {
        //         _depRepository.Delete(id);
        //     }
        //     catch (Exception)
        //     {
        //         success = false;
        //     }
        //     return success
        //         ? new JsonResult("Delete successful")
        //         : new JsonResult("Delete was not successful");
        // }
    }
}