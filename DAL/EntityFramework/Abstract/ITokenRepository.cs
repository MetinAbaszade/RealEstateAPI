using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using System.Linq.Expressions;

namespace DAL.EntityFramework.Abstract;

public interface ITokenRepository : IGenericRepository<Token>
{
    Task<bool> IsValidAsync(string accessToken, string refreshToken);
    Task<List<Token>> GetActiveTokensAsync(string accessToken);
    Task<Token> GetAsync(Expression<Func<Token, bool>> filter);
}