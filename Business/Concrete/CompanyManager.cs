using Business.Abstract;
using DAL.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CompanyManager : ICompanyService
    {
        private readonly ICompanyDAL _companyDal;

        public CompanyManager(ICompanyDAL companyDAL)
        {
            _companyDal = companyDAL;
        }

        public async Task AddAsync(CompanyDTO dto)
        {
            var model = new Company
            {
                Adress = dto.Adress,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber
            };

            await _companyDal.AddAsync(model);
        }

        public async Task<List<CompanyDTO>> GetAllAsync()
        {
            var result = await _companyDal.GetAllAsync();
            var dto = result.Select(r => new CompanyDTO
            {
                Adress = r.Adress,
                Name = r.Name,
                PhoneNumber = r.PhoneNumber,
                Id = r.CompanyID
            }).ToList();
            return dto;
        }
    }
}
