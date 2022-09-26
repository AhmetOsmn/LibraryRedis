using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await _companyService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(CompanyDTO dto)
        {
            await _companyService.AddAsync(dto);
            return Ok();
        }
    }
}
