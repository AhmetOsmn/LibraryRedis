using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.DTOs;
using UI.Models;

namespace UI.Abstract
{
    public interface IRequestService
    {
        Task<string> AddBook(BookDTO dto, TokenDTO tokenDto);
        Task<string> AddRegister(RegisterDTO dto);
        Task AddCompany(CompanyDTO dto, TokenDTO tokenDTO);
        Task<string> AddReservation(ReservationDTO dto, TokenDTO tokenDto);

        Task UpdateBook(TokenDTO dto, UpdateBookDTO book);

        Task<string> DeleteReservation(Models.ReservationDeleteDTO dto, TokenDTO tokenDto);

        Task<TokenDTO> CreateLoginRequest(UserLoginDTO dto);
        Task<User> GetLoggedInUserInformation(TokenDTO dto);
        Task<List<CompanyDTO>> GetAllCompanies(TokenDTO dto);
        Task<List<Book>> GetAllBooks(TokenDTO dto);
        Task<List<ReservationDTOUI>> GetReservations(TokenDTO dto, int userId);
    }
}
