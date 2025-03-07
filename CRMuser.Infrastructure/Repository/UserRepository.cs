﻿using CRMuser.Application.DTO;
using CRMuser.Infrastructure.Data;
using CRMUser.domain.Interface;
using CRMUser.domain.Model;
using Microsoft.EntityFrameworkCore;

namespace CRMuser.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbcontext;
        private readonly ITokenGeneration _tokenGeneration;

        public UserRepository(UserDbContext dbContext, ITokenGeneration tokengeneration)
        {
            _dbcontext = dbContext;
            _tokenGeneration = tokengeneration;
        }
        public async Task<string> Login(LoginDTO entity)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == entity.Email && x.Password == entity.Password);

            if (user is not null)
            {
                return _tokenGeneration.GenerateToken(user.Email!, user.Name!);
            }
            return null!;
        }

        public async Task Register(User entity)
        {
            await _dbcontext.Users.AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
