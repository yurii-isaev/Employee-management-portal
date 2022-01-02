using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Authentication.Controllers
{
  [ApiController]
  [Route("api/[controller]/[action]")]
  public abstract class BaseController : ControllerBase
  {
    private IMediator _mediator = null!;

    protected IMediator Mediator => _mediator ??= HttpContext
      .RequestServices
      .GetRequiredService<IMediator>();
  }
}
