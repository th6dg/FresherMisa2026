using FresherMisa2026.Application.Interfaces.Services;
using FresherMisa2026.Entities;
using FresherMisa2026.Entities.Position;
using Microsoft.AspNetCore.Mvc;

namespace FresherMisa2026.WebAPI.Controllers
{
    [ApiController]
    public class PositionsController : BaseController<Position>
    {
        private readonly IPositionService _positionService;

        public PositionsController(
            IPositionService positionService) : base(positionService)
        {
            _positionService = positionService;
        }

        [HttpGet("Code/{code}")]
        public async Task<ActionResult<ServiceResponse>> GetByCode(string code)
        {
            var response = new ServiceResponse();
            try
            {
                response.Data = await _positionService.GetPositionByCodeAsync(code);
                response.IsSuccess = true;

            }
            catch
            {
                response.IsSuccess = false;
                response.DevMessage = "Không tìm thấy position";
            }
            return response;
            
        }
    }
}