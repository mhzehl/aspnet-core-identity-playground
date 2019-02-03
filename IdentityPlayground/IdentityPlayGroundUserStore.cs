using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace IdentityPlayground
{
    public class IdentityPlayGroundUserStore : IUserStore<IdentityPlaygroundUser>, IUserPasswordStore<IdentityPlaygroundUser>
    {
        public void Dispose()
        {
        }

        public Task<string> GetUserIdAsync(IdentityPlaygroundUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(IdentityPlaygroundUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(IdentityPlaygroundUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityPlaygroundUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(IdentityPlaygroundUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(IdentityPlaygroundUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "insert into IdentityPlaygroundUsers([Id]," +
                    "[UserName]," +
                    "[NormalizedUserName]," +
                    "[PasswordHash]" +
                    "Values(@id, @userName, @normalizedUserName, @passwordHash)",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    }
                );
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityPlaygroundUser user, CancellationToken cancellationToken)
            {
                using (var connection = GetOpenConnection())
                {
                    await connection.ExecuteAsync(
                        "update IdentityPlaygroundUsers" +
                        "set [Id] = @id," +
                        "set [UserName] = @userName," +
                        "set [NormalizedUserName] = @normalizedUserName," +
                        "set [PasswordHash] = @passwordHash" +
                        "where [Id] = @id",
                        new
                        {
                            id = user.Id,
                            userName = user.UserName,
                            normalizedUserName = user.NormalizedUserName,
                            passwordHash = user.PasswordHash
                        }
                    );
                }

                return IdentityResult.Success;            }

            public Task<IdentityResult> DeleteAsync(IdentityPlaygroundUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public async Task<IdentityPlaygroundUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
            {
                using (var connection = GetOpenConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<IdentityPlaygroundUser>(
                        "select * from IdentityPlayGroundUsers where Id = @id", new
                        {
                            id = userId
                        }
                    );
                }
            }

            public async Task<IdentityPlaygroundUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            {
                using (var connection = GetOpenConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<IdentityPlaygroundUser>(
                        "select * from IdentityPlayGroundUsers where NormalizedUserName = @name", new
                        {
                            name = normalizedUserName
                        }
                    );
                }            }

            public static DbConnection GetOpenConnection()
            {
                var connection = new SqlConnection("DataSource=.\\SQLEXPRESS;" + "database=IdentityPlayground" +
                                                   "trusted_connection=yes;");
                connection.Open();

                return connection;
            }

        public Task SetPasswordHashAsync(IdentityPlaygroundUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(IdentityPlaygroundUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityPlaygroundUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
    }
