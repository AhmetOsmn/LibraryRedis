using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Abstract;
using UI.DTOs;
using UI.Models;

namespace UI.Managers
{
    public class RequestManager : IRequestService
    {
        private readonly IWebRequestService _webRequestService;
        private readonly IConfiguration _configuration;

        public RequestManager(IWebRequestService webRequestService, IConfiguration configuration)
        {
            _webRequestService = webRequestService;
            _configuration = configuration;
        }

        public async Task<string> AddBook(BookDTO dto, TokenDTO tokenDto)
        {
            string serviceUrl = _configuration.GetValue<string>("ServiceUrl");
            string loginUrl = serviceUrl + "api/book/add";
            var result = await _webRequestService.CreatePostRequest(loginUrl, dto, tokenDto.Token);
            if (!String.IsNullOrEmpty(result))
                return result;
            return null;
        }

        public async Task AddCompany(CompanyDTO dto, TokenDTO tokenDTO)
        {
            string serviceUrl = _configuration.GetValue<string>("ServiceUrl");
            string loginUrl = serviceUrl + "api/company/add";
            var result = await _webRequestService.CreatePostRequest(loginUrl, dto, tokenDTO.Token);
        }

        public async Task<string> AddRegister(RegisterDTO dto)
        {
            string serviceUrl = _configuration.GetValue<string>("ServiceUrl");
            string loginUrl = serviceUrl + "api/auth/register";
            var result = await _webRequestService.CreatePostRequest(loginUrl, dto, null);
            return result;

        }

        public async Task<TokenDTO> CreateLoginRequest(UserLoginDTO dto)
        {
            string serviceUrl = _configuration.GetValue<string>("ServiceUrl");
            string loginUrl = serviceUrl + "api/auth/login";
            var result = await _webRequestService.CreatePostRequest(loginUrl, dto, null);
            if (!String.IsNullOrEmpty(result))
                return JsonConvert.DeserializeObject<TokenDTO>(result);
            return null;
        }

        public async Task<List<Book>> GetAllBooks(TokenDTO dto)
        {
            string serviceUrl = _configuration.GetValue<string>("ServiceUrl");
            string loginUrl = serviceUrl + "api/book/getall";
            var result = await _webRequestService.CreateGetRequest(loginUrl, dto.Token);
            if (!String.IsNullOrEmpty(result))
                return JsonConvert.DeserializeObject<List<Book>>(result);
            return null;
        }

        public async Task<List<CompanyDTO>> GetAllCompanies(TokenDTO dto)
        {
            string serviceUrl = _configuration.GetValue<string>("ServiceUrl");
            string loginUrl = serviceUrl + "api/company/getall";
            var result = await _webRequestService.CreateGetRequest(loginUrl, dto.Token);
            if (!String.IsNullOrEmpty(result))
                return JsonConvert.DeserializeObject<List<CompanyDTO>>(result);
            return null;
        }

        public async Task<User> GetLoggedInUserInformation(TokenDTO dto)
        {
            string serviceUrl = _configuration.GetValue<string>("ServiceUrl");
            string loginUrl = serviceUrl + "api/user/info";
            var result = await _webRequestService.CreateGetRequest(loginUrl, dto.Token);
            if (!String.IsNullOrEmpty(result))
                return JsonConvert.DeserializeObject<User>(result);
            return null;
        }

        public async Task<List<ReservationDTOUI>> GetReservations(TokenDTO dto,int userId)
        {
            string serviceUrl = _configuration.GetValue<string>("ServiceUrl");
            string url = serviceUrl + "api/reservation/getByUserId?userId="+userId;
            var result = await _webRequestService.CreateGetRequest(url, dto.Token);
            if (!String.IsNullOrEmpty(result))
                return JsonConvert.DeserializeObject<List<ReservationDTOUI>>(result);
            return null;
        }

        public async Task UpdateBook(TokenDTO dto, UpdateBookDTO book)
        {
            string serviceUrl = _configuration.GetValue<string>("ServiceUrl");
            string loginUrl = serviceUrl + "api/book/update";
            var result = await _webRequestService.CreatePostRequest(loginUrl,book, dto.Token);
            //Güncelleme döünüşü bildirim ekranı
        }
    }
}
