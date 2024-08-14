using DAL.Context;
using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EntityFramework.Concrete;

public class TokenRepository(PropertyDbContext dataContext) : GenericRepository<Token>(dataContext), ITokenRepository
{
    public async Task<bool> IsValidAsync(string accessToken, string refreshToken)
    {
        return await dataContext.Tokens.AnyAsync(m => m.AccessToken == accessToken);
    }

    public async Task<List<Token>> GetActiveTokensAsync(string accessToken)
    {
        return await dataContext.Tokens.Where(m => m.AccessToken == accessToken).ToListAsync();
    }

    public async Task<Token> GetAsync(Expression<Func<Token, bool>> filter)
    {
        return (await dataContext.Tokens.Include(m => m.User).FirstOrDefaultAsync(filter))!;
    }
}