using BankAnalizer.Core;
using BankAnalizer.Db;
using BankAnalizer.Db.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Users
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<User> Create(User user, string password);
        Task Update(User user, string password = null);
        Task Delete(Guid id);
    }

    public class UserService : IUserService
    {
        private readonly IContextFactory contextFactory;
        private static readonly ConcurrentDictionary<Guid, User> usersCache = new ConcurrentDictionary<Guid, User>();

        public UserService(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            using var context = contextFactory.GetContext();
            var user = await context.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using var context = contextFactory.GetContext();
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            if (!usersCache.ContainsKey(id))
            {
                using var context = contextFactory.GetContext();
                var user = await context.Users.FindAsync(id);
                usersCache.TryAdd(id, user);
            }

            return usersCache[id];
        }

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new BankAnalizerException("Password is required");

            using var context = contextFactory.GetContext();
            if (await context.Users.AnyAsync(x => x.Username == user.Username))
                throw new BankAnalizerException($"Username \"{user.Username}\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task Update(User userParam, string password = null)
        {
            using var context = contextFactory.GetContext();
            var user = await context.Users.FindAsync(userParam.Id);

            if (user == null)
                throw new BankAnalizerException("User not found");

            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                if (await context.Users.AnyAsync(x => x.Username == userParam.Username))
                    throw new BankAnalizerException("Username " + userParam.Username + " is already taken");

                user.Username = userParam.Username;
            }

            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            using var context = contextFactory.GetContext();
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}