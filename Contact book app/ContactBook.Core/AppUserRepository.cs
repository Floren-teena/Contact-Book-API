using ContactBook.Data;
using ContactBook.Modell.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Core.DTO;
using ContactBook.Infratsructure.Helper;
using Microsoft.EntityFrameworkCore;
using ContactBook.Infratsructure.Interfaces;
using ContactBook.Infratsructure.Services;
using Microsoft.AspNetCore.Identity;
using ContactBook.Infratsructure;

namespace ContactBook.Core
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;
        public AppUserRepository(AppDbContext context, IPhotoService photoService, UserManager<AppUser> userManager)
        {
            _context = context;
            _photoService = photoService;
            _userManager = userManager;
        }

        public async Task<string> AddUser(AppUserDTO appUserDto)
        {
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(e => e.Email == appUserDto.Email);
            if (existingUser != null)
            {
                return "User already exist";
            }

            var newAppUser = new AppUser()
            {
                FirstName = appUserDto.FirstName,
                LastName = appUserDto.LastName,
                Email = appUserDto.Email,
                PhoneNumber = appUserDto.PhoneNumber,
            };
            _context.AppUsers.Add(newAppUser);
            var saveChanges = await _context.SaveChangesAsync();
            if (saveChanges > 0)
            {
                return "User added Successfully";
            }

            return "User could not be added";
        }

        /* public async Task<List<AppUserDTO>> GetAllUsers(PaginParameter userParameter)
         {
             var contacts = _context.AppUsers
                 .OrderBy(p => p.FirstName)
                 .Skip((userParameter.PageNumber - 1) * userParameter.PageSize)
                 .Take(userParameter.PageSize)
                 .ToList();


             var data = new List<AppUserDTO>();
             foreach (var userData in contacts)
             {
                 data.Add(new AppUserDTO
                 {
                     FirstName = userData.FirstName,
                     LastName = userData.LastName,
                     Email = userData.Email,
                     PhoneNumber = userData.PhoneNumber,
                     City = userData.City,
                     Country = userData.Country,
                     ImageUrl = userData.ImageUrl,
                     State = userData.State,
                     FacebookUrl = userData.FacebookUrl,
                     TwitterUrl = userData.TwitterUrl
                 });
             }
             return data;
         }*/

        public async Task<List<AppUserDTO>> GetAllUsers(PaginParameter userParameter)
        {
            var contacts = _context.AppUsers
                .OrderBy(p => p.FirstName)
                /*.Skip((userParameter.PageNumber - 1) * userParameter.PageSize)
                .Take(userParameter.PageSize)*/
                .ToList();


            var data = new List<AppUserDTO>();
            foreach (var userData in contacts)
            {
                data.Add(new AppUserDTO
                {
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                    City = userData.City,
                    Country = userData.Country,
                    ImageUrl = userData.ImageUrl,
                    State = userData.State,
                    FacebookUrl = userData.FacebookUrl,
                    TwitterUrl = userData.TwitterUrl
                });
            }
            return data.Skip((userParameter.PageNumber - 1) * userParameter.PageSize)
                .Take(userParameter.PageSize).ToList();
        }

        public async Task<AppUserDTO> GetUserById(string id)
        {
            var data = await _context.AppUsers.FirstOrDefaultAsync(x => x.Id == id);
            var newuser = new AppUserDTO
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                City = data.City,
                Country = data.Country,
                ImageUrl = data.ImageUrl,
                State = data.State,

            };
            return newuser;

        }

        public async Task<string> UpdateAppUser(AppUser appUser)
        {
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(e => e.Id == appUser.Id);
            if (existingUser != null)
            {
                _context.AppUsers.Update(existingUser);
                return "User updated successfully";
            }

            return "No User found";
        }

        public async Task<bool> AddPhoto(PhotoDTO photoDTO, string id)
        {
            var result = await _photoService.AddPhotoAsync(photoDTO.ImageUrl);
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(p => p.Id == id);
            existingUser.ImageUrl = result.Url.AbsolutePath;
            await _userManager.UpdateAsync(existingUser);
            return true;
        }

        public async Task<List<AppUserDTO>> GetAllAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return new List<AppUserDTO>();
            }

            var users = await _userManager.Users
                .Where(p => p.Email.Contains(term)
                            || p.FirstName.Contains(term)
                            || p.LastName.Contains(term)
                            || p.City.Contains(term)
                            || p.State.Contains(term)
                            || p.Country.Contains(term)
                ).ToListAsync();
            var AppUserDTO = users.Select(item => new AppUserDTO
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                ImageUrl = item.ImageUrl,
                City = item.City,
                State = item.State,
                Country = item.Country,
                FacebookUrl = item.FacebookUrl,
                TwitterUrl = item.TwitterUrl
            }).ToList();
            return AppUserDTO;

          
        }



       /* public async Task<string> UpdateAppUser(AppUser appUser)
        {
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(e => e.Id == appUser.Id);
            if (existingUser != null)
            {
                _context.AppUsers.Update(existingUser);
                return "User updated successfully";
            }

            return "No User found";
        }*/

      
    }
}
