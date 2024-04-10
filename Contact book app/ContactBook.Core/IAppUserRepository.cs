using ContactBook.Modell.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Core.DTO;
using ContactBook.Infratsructure;
using ContactBook.Infratsructure.Helper;

namespace ContactBook.Core
{
    public interface IAppUserRepository
    {

        Task<string> AddUser(AppUserDTO appuserdto);

        Task<List<AppUserDTO?>> GetAllUsers(PaginParameter userParameter);

        Task<AppUserDTO> GetUserById(string id);

        Task<string> UpdateAppUser(AppUser appUser);

        Task<bool> AddPhoto(PhotoDTO photo, string id);

        Task<List<AppUserDTO>> GetAllAsync(string term);

    }
}
