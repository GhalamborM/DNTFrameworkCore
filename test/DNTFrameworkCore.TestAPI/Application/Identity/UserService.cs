using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DNTFrameworkCore.Application;
using DNTFrameworkCore.Cryptography;
using DNTFrameworkCore.EFCore.Application;
using DNTFrameworkCore.EFCore.Context;
using DNTFrameworkCore.EFCore.Linq;
using DNTFrameworkCore.EFCore.Querying;
using DNTFrameworkCore.Eventing;
using DNTFrameworkCore.Querying;
using DNTFrameworkCore.TestAPI.Application.Identity.Models;
using DNTFrameworkCore.TestAPI.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace DNTFrameworkCore.TestAPI.Application.Identity
{
    public interface IUserService : IEntityService<long, UserReadModel, UserModel>
    {
    }

    public class UserService : EntityService<User, long, UserReadModel, UserModel>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserPasswordHashAlgorithm _password;

        public UserService(
            IDbContext dbContext,
            IEventBus bus,
            IUserPasswordHashAlgorithm password,
            IMapper mapper) : base(dbContext, bus)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _password = password ?? throw new ArgumentNullException(nameof(password));
        }

        protected override IQueryable<User> FindEntityQueryable => base.FindEntityQueryable.Include(u => u.Roles)
            .Include(u => u.Permissions);

        public override Task<IPagedResult<UserReadModel>> FetchPagedListAsync(FilteredPagedRequest request,
            CancellationToken cancellationToken = default)
        {
            request.SortingIfNullOrEmpty("Id DESC");
            return EntitySet.AsNoTracking()
                .Select(u => new UserReadModel
                {
                    Id = u.Id,
                    IsActive = u.IsActive,
                    UserName = u.UserName,
                    DisplayName = u.DisplayName,
                    LastLoggedInDateTime = u.LastLoggedInDateTime
                }).ToPagedListAsync(request, cancellationToken);
        }

        protected override void MapToEntity(UserModel model, User user)
        {
            _mapper.Map(model, user);

            ResetSecurityToken(user, model);
            MapPasswordHash(user, model);
        }

        protected override UserModel MapToModel(User user)
        {
            return _mapper.Map<UserModel>(user);
        }

        private static void ResetSecurityToken(User user, UserModel model)
        {
            if (!model.ShouldResetSecurityToken()) return;

            user.SecurityToken = Guid.NewGuid().ToString("N");
        }

        private void MapPasswordHash(User user, UserModel model)
        {
            if (!model.ShouldMapPasswordHash()) return;

            user.PasswordHash = _password.HashPassword(model.Password);
        }
    }
}