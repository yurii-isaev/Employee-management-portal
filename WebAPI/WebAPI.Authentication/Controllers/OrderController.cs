using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Authentication.UseCases.Models.Input;
using WebAPI.Authentication.UseCases.Requests.Commands;
using WebAPI.Authentication.UseCases.Requests.Queries;

namespace WebAPI.Authentication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : BaseController
{
  /// <remarks>
  /// Sample request:
  /// POST -> http://localhost:5000/api/order/CreateOrder
  /// </remarks>
  [Authorize(Roles = "Customer")]
  [HttpPost("CreateOrder")]
  public async Task<IActionResult> CreateOrder([FromBody] OrderDto order)
  {
    var request = new OrderCommand() {OrderDto = order};
    return Ok(await Mediator.Send(request));
  }

  /// <remarks>
  /// Sample request:
  /// GET -> http://localhost:5000/api/order/GetOrderList/{userId}
  /// </remarks>
  [Authorize(Roles = "Customer")]
  [HttpGet("GetOrderList/{userId}")]
  public async Task<IActionResult> GetOrderList(string userId)
  {
    var request = new GetUserOrderListQuery() {UserId = userId};
    return Ok(await Mediator.Send(request));
  }
}
