using Cloud_Mall.Application.DTOs.StoreCategory;
using Cloud_Mall.Application.StoreCategories.Command.CreateStoreCategory;
using Cloud_Mall.Application.StoreCategories.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud_Mall.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreCategoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public StoreCategoryController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await mediator.Send(new StoreCategoryQuery());
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStoreCategory([FromBody] CreateStoreCategoryDTO dto)
        {
            CreateStoreCategoryCommand command = new CreateStoreCategoryCommand()
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var result = await mediator.Send(command);
            return Created("", result);
        }


        [HttpGet("Admin/GetAllCategoriesByAdmin")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Tags("Admin - Stores")]
        public async Task<IActionResult> GetAllCategoriesByAdmin()
        {
            var categories = await mediator.Send(new StoreCategoryByAdminQuery());
            return Ok(categories);
        }
        [HttpPost("Admin/CreateStoreCategoryByAdmin")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [Tags("Admin - Stores")]
        public async Task<IActionResult> CreateStoreCategoryByAdmin([FromBody] CreateStoreCategoryDTO dto)
        {
            var command = new CreateStoreCategoryByAdminCommand()
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var result = await mediator.Send(command);
            return Created("", result);
        }



    }
}
