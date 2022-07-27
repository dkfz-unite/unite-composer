﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Unite.Composer.Admin.Constants;
using Unite.Identity.Entities;
using Unite.Identity.Entities.Enums;
using Unite.Identity.Services;

namespace Unite.Composer.Admin.Services;

public class UserService
{
    private readonly IdentityDbContext _dbContext;


    public UserService(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public User GetUser(int id)
    {
        return _dbContext.Set<User>()
            .Include(user => user.UserPermissions)
            .FirstOrDefault(user => user.Id == id);
    }

    public User GetUser(Expression<Func<User, bool>> predicate)
    {
        return _dbContext.Set<User>()
            .Include(user => user.UserPermissions)
            .FirstOrDefault(predicate);
    }

    public User[] GetUsers()
    {
        return _dbContext.Set<User>()
            .Include(user => user.UserPermissions)
            .ToArray();
    }

    public User[] GetUsers(Expression<Func<User, bool>> predicate)
    {
        return _dbContext.Set<User>()
            .Include(user => user.UserPermissions)
            .Where(predicate)
            .ToArray();
    }

    public User Add(string email, Permission[] permissions = null)
    {
        var user = GetUser(user => user.Email == email);

        if (user == null)
        {
            user = new User
            {
                Email = email,
                IsRoot = false,
                IsRegistered = false,
                UserPermissions = GetUserPermissions(permissions)
            };

            _dbContext.Add(user);
            _dbContext.SaveChanges();

            return user;
        }
        else
        {
            return null;
        }
    }

    public User Update(int id, Permission[] permissions = null)
    {
        var user = GetUser(id);

        if (user != null)
        {
            user.UserPermissions = GetUserPermissions(permissions);

            _dbContext.Update(user);
            _dbContext.SaveChanges();

            return user;
        }
        else
        {
            return null;
        }
    }

    public bool Delete(int id)
    {
        var user = GetUser(id);

        if (user != null)
        {
            _dbContext.Remove(user);
            _dbContext.SaveChanges();

            return true;
        }
        else
        {
            return false;
        }
    }


    private static UserPermission[] GetUserPermissions(Permission[] permissions = null)
    {
        var defaultPermissions = Permissions.DefaultPermissions;

        return permissions != null && permissions.Any()
            ? permissions.Select(permissionId => new UserPermission { PermissionId = permissionId }).ToArray()
            : defaultPermissions.Select(permissionId => new UserPermission { PermissionId = permissionId }).ToArray();
    }
}