using Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICompanyService
    {
        Task<List<CompanyDTO>> GetAllAsync();
        Task AddAsync(CompanyDTO dto);
    }
}
