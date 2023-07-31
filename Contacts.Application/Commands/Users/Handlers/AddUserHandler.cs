using System.Net;
using Contacts.Application.Helpers;
using Contacts.Domain.Entities;
using Contacts.Domain.Repositories;
using Contacts.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace Contacts.Application.Commands.Users.Handlers;

public class AddUserHandler : ICommandHandler<AddUserRequest>
{
    #region Fileds
    
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordService _passwordService;
    
    #endregion
    
    #region Constructor

    public AddUserHandler(IUsersRepository usersRepository, IPasswordService passwordService)
    {
        _usersRepository = usersRepository;
        _passwordService = passwordService;
    }

    #endregion
    
    #region Methods
    
    public async Task HandleAsync(AddUserRequest command, HttpContext context)
    {
        var user = User.Create(command.UserName, command.Password);
        
        if (await _usersRepository.GetUserByUserNameAsync(command.UserName) != null)
        {
            await HttpHelpers.SetResponseAsync(context, HttpStatusCode.BadRequest, "UserName already taken");
            return;
        }

        user.Salt = _passwordService.CreateSalt();
        user.Password = _passwordService.HashPassword(user.Password, user.Salt);
        await _usersRepository.AddUserAsync(user);
        
        await HttpHelpers.SetResponseAsync(context, HttpStatusCode.Created, "User added successfully");
    }
    
    #endregion
}