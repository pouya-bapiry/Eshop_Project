using Eshop.Application.Extensions;
using Eshop.Application.Services.Interfaces;
using Eshop.Application.Utilities;
using Eshop.Domain.Dtos.Account;
using Eshop.Domain.Dtos.Account.User;
using Eshop.Domain.Dtos.Paging;
using Eshop.Domain.Entities.Account.Role;
using Eshop.Domain.Entities.Account.User;
using Eshop.Domain.Repository;
using MarketPlace.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Resume.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Application.Services.Implementation
{
    public class UserService : IUserService
    {

        #region Fields

        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly ISmsService _smsService;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository, IPasswordHasher passwordHasher, ISmsService smsService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _smsService = smsService;
        }


        #endregion

        #region User Methods

        #region register
        public async Task<RegisterUserResult> RegisterUser(RegisterUserDto register)
        {
            var notActive = await _userRepository
                    .GetQuery()
                    .AsQueryable()
                    .SingleOrDefaultAsync(x => x.Mobile == register.Mobile);

            try
            {
                if (!await IsUserExistByMobile(register.Mobile))
                {
                    var user = new User
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        Mobile = register.Mobile,
                        Password = _passwordHasher.EncodePasswordMd5(register.Password),
                        MobileActiveCode = new Random().Next(100000, 999999).ToString(),
                        EmailActiveCode = Guid.NewGuid().ToString("N"),
                        Avatar = null,
                        RoleId = 8,
                    };

                    await _userRepository.AddEntity(user);
                    await _userRepository.SaveChanges();

                    await _smsService.SendVerificationSms(register.Mobile, user.MobileActiveCode);

                    return RegisterUserResult.Success;
                }
                else if (notActive.IsMobileActive == true && notActive.MobileActiveCode != null)
                {
                    return RegisterUserResult.MobileExists;
                }

                var newActiveCode = new Random().Next(100000, 999999).ToString();
                if (notActive.Password == register.Password)
                {
                    if (!notActive.IsMobileActive && notActive.MobileActiveCode != null)
                    {
                        notActive.MobileActiveCode = newActiveCode;
                        await _smsService.SendVerificationSms(notActive.Mobile, newActiveCode);

                        _userRepository.EditEntity(notActive);
                        await _userRepository.SaveChanges();
                        return RegisterUserResult.MobileNotActive;
                    }
                }
                else
                {
                    return RegisterUserResult.PasswordError;
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RegisterUserResult.Error;
        }
        public async Task<bool> IsUserExistByMobile(string mobile)
        {
            return await _userRepository
                 .GetQuery()
                 .AsQueryable()
                 .AnyAsync
                  (x => x.Mobile == mobile);
        }
        #endregion

        #region Login
        public async Task<UserLoginResult> UserLogin(LoginUserDto login)
        {
            var user = await _userRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Mobile == login.Mobile);

            if (user == null)
            {
                return UserLoginResult.UserNotFound;
            }

            if (!user.IsMobileActive)
            {
                return UserLoginResult.MobileNotActivated;
            }

            return user.Password != _passwordHasher.EncodePasswordMd5(login.Password)
                ? UserLoginResult.UserNotFound : UserLoginResult.Success;
        }

        public async Task<User> GetUserByMobile(string mobile)
        {
            return await _userRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync
                (x => x.Mobile == mobile);
        }

        #endregion

        #region GetUser
        public async Task<User> GetUserById(long id)
        {
            return await _userRepository
                 .GetQuery()
                 .AsQueryable()
            .SingleOrDefaultAsync
                 (x => x.Id == id);
        }
        #endregion

        #region Activate
        public async Task<bool> ActivateMobile(ActiveMobileDto activate)
        {
            var user = await _userRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync
                (x => x.Mobile == activate.Mobile);

            if (user != null)
            {
                if (user.MobileActiveCode == activate.MobileActivationCode)
                {
                    user.IsMobileActive = true;
                    user.MobileActiveCode = new Random().Next(100000, 999999).ToString();

                    await _userRepository.SaveChanges();
                    return true;
                }

            }
            return false;
        }
        #endregion

        #region Recover and Change password
        public async Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDto forgot)
        {
            var user = await _userRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync
                (x => x.Mobile == forgot.Mobile);

            if (user == null)
            {
                return ForgotPasswordResult.NotFound;

            }
            var newPassword = PasswordGenerator.CreateRandomPassword();
            user.Password = _passwordHasher.EncodePasswordMd5(newPassword);
            _userRepository.EditEntity(user);
            await _smsService.SendRecoverPasswordSms(forgot.Mobile, newPassword);

            await _userRepository.SaveChanges();
            return ForgotPasswordResult.Success;
        }

        public async Task<ChangePasswordResult> ChangeUserPassword(ChangePasswordDto changePassword, long userId)
        {

            var user = await _userRepository.GetEntityById(userId);
            if (user == null)
                return ChangePasswordResult.Error;


            var currentPasswordHash = _passwordHasher.EncodePasswordMd5(changePassword.CurrentPassword);
            if (user.Password != currentPasswordHash)
                return ChangePasswordResult.WrongCurrentPassword;


            var newPasswordHash = _passwordHasher.EncodePasswordMd5(changePassword.NewPassword);
            if (user.Password == newPasswordHash)
                return ChangePasswordResult.NewPasswordSameAsOld;


            user.Password = newPasswordHash;
            await _userRepository.SaveChanges();
            return ChangePasswordResult.Success;





        }

        #endregion

        #region Edit User Profile
        public async Task<string?> GetUserImage(long userId)
        {
            var user = await _userRepository.GetQuery().AsQueryable().FirstOrDefaultAsync(x => x.Id == userId);
            return user?.Avatar;
        }
        public async Task<EditUserProfileDto> GetProfileForEdit(long userId)
        {
            var user = await _userRepository.GetEntityById(userId);

            if (user == null)
            {
                return null;

            }

            return new EditUserProfileDto
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Avatar = user.Avatar,
            };
        }

        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto profile, long userId, IFormFile avatarImage)
        {
            var user = await _userRepository.GetEntityById(userId);
            if (user == null)
            {
                return EditUserProfileResult.NotFound;
            }
            if (user.IsBlocked)
            {
                return EditUserProfileResult.IsBlocked;
            }
            if (!user.IsMobileActive)
            {
                return EditUserProfileResult.IsNotActive;
            }
            //user.FirstName = profile.FirstName;
            //user.LastName = profile.LastName;
            //user.Email = profile.Email;
            //user.Avatar = profile.Avatar;
            user.EditProfile(profile.FirstName, profile.LastName, profile.Email);




            if (avatarImage != null && avatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatarImage.FileName);
                avatarImage.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer,
                    100, 100, PathExtension.UserAvatarThumbServer, user.Avatar);
                user.Avatar = imageName;



            }

            _userRepository.EditEntity(user);
            await _userRepository.SaveChanges();
            return EditUserProfileResult.Success;

            return EditUserProfileResult.NotImage;

        }
        #endregion

        #region Filter
        public async Task<FilterUserDto> FilterUser(FilterUserDto filter)
        {
            var query = _userRepository
                 .GetQuery()
                 .Include(x => x.Role)
                 .AsQueryable();

            if (filter.RoleId > 0)
            {
                query = query.Where(x => x.RoleId == filter.RoleId);
            }
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                query = query.Where(x => EF.Functions.Like(x.FirstName, $"%{filter.FirstName}%"));
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                query = query.Where(x => EF.Functions.Like(x.LastName, $"%{filter.LastName}%"));
            }
            if (!string.IsNullOrEmpty(filter.Mobile))
            {
                query = query.Where(x => EF.Functions.Like(x.Mobile, $"%{filter.Mobile}%"));
            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(x => EF.Functions.Like(x.Email, $"%{filter.Email}%"));
            }

            #region Paging


            var roleCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, roleCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).OrderByDescending(x => x.Id).ToListAsync();


            #endregion

            return filter.SetPaging(pager).SetUsers(allEntities);
        }
        #endregion

        #region EditUser
        public async Task<EditUserDto> GetUserForEdit(long userId)
        {
            var user = await _userRepository.GetQuery().AsQueryable().Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return null;
            }

            return new EditUserDto
            {
                Id = user.Id,
                RoleId = user.Role.Id,
                Email = user.Email,
                Mobile = user.Mobile,
                IsBlocked = user.IsBlocked,
                IsMobileActivated = user.IsMobileActive,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public async Task<EditUserResult> EditUser(EditUserDto edit, string username)
        {
            var user = await _userRepository.GetQuery().AsQueryable().Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == edit.Id);
            if (user == null)
            {
                return EditUserResult.UserNotFound;

            }

            user.Id = edit.Id;
            user.RoleId = edit.RoleId;
            user.FirstName = edit.FirstName;
            user.LastName = edit.LastName;
            user.Mobile = edit.Mobile;
            user.Email = edit.Email;
            user.IsBlocked = edit.IsBlocked;
            user.IsMobileActive = edit.IsMobileActivated;

            _userRepository.EditEntityByUser(user, username);
            _userRepository.SaveChanges();
            return EditUserResult.Success;
        }
        #endregion



        #endregion

        #region Role Methods
        #region Filter
        public async Task<FilterRoleDto> FilterRole(FilterRoleDto filter)
        {
            var query = _roleRepository
                .GetQuery()
                .Include(x => x.Users)
                .AsQueryable();


            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                query = query.Where(x => EF.Functions.Like(x.RoleName, $"%{filter.RoleName}%"));
            }

            #region Paging

            var roleCount = await query.CountAsync();
            var pager = Pager.Build(filter.PageId, roleCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetPaging(pager).SetRoles(allEntities);
        }
        #endregion

        #region Create
        public async Task<CreateRoleResult> CreateRole(CreateRoleDto role)
        {
            var newRole = new Role
            {
                RoleName = role.RoleName,
            };
            await _roleRepository.AddEntity(newRole);
            _roleRepository.SaveChanges();
            return CreateRoleResult.Success;
        }
        #endregion


        #region Edit
        public async Task<EditRoleDto> GetRoleForEdit(long roleId)
        {
            var edit = await _roleRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == roleId);
            if (edit == null)
            {
                return null;
            }
            return new EditRoleDto
            {
                Id = edit.Id,
                RoleName = edit.RoleName,

            };
        }

        public async Task<EditRoleResult> EditRole(EditRoleDto edit, string username)
        {
            var role = await _roleRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == edit.Id);
            if (role == null)
            {
                return EditRoleResult.Error;
            }

            role.Id = edit.Id;
            role.RoleName = edit.RoleName;

            _roleRepository.EditEntityByUser(role, username);
            _roleRepository.SaveChanges();
            return EditRoleResult.Success;

                

        }
        #endregion

        #region GetAll
        public async Task<List<Role>> GetRoles()
        {
            return await _roleRepository.GetQuery().AsQueryable().Select(x=>new Role
            {
                Id=x.Id,
                RoleName=x.RoleName,
            }).ToListAsync();

        }
        #endregion




        #endregion

        #region dipose

        public async ValueTask DisposeAsync()
        {
            if (_userRepository != null)
            {

                await _userRepository.DisposeAsync();

            }
        }




        #endregion
    }
}
