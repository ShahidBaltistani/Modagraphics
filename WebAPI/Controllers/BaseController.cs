using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using WebAPI.ActionResults;
using Database;
using WebAPI.JWT;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Database.Models;
using System.Globalization;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        protected RapidusContext Entities { get; }
        public UserClaims UserClaims { get; private set; }

        public BaseController(IHttpContextAccessor httpContext)
        {
            Entities = new RapidusContextFactory().CreateDbContext(null);
            GetUserClaims(httpContext.HttpContext.User);
        }

        protected T GetClaim<T>(Claims claim, ClaimsPrincipal user) where T : IConvertible
                => (T)Convert.ChangeType(user.FindFirst(claim.ToString())?.Value, typeof(T));

        protected void GetUserClaims(ClaimsPrincipal user)
        {
            var claims = new UserClaims()
            {
                UserId = GetClaim<uint>(Claims.UserId, user),
                TimeZoneOffset = GetClaim<int>(Claims.TimeZoneOffset, user),
            };

            claims.DateTime = DateTime.UtcNow.AddMinutes(claims.TimeZoneOffset);

            UserClaims = claims;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override OkObjectResult Ok(object value) => Success(value);

        protected OkObjectResult Ok(object value, object extra) => Success(value, extra);

        private OkObjectResult Success<T>(T value) => base.Ok(new SuccessModel<T>(value));

        private OkObjectResult Success<T, I>(T value, I extra) => base.Ok(new SuccessModel<T, I>(value, extra));

        protected IActionResult MethodFailure(Exception ex) => new MethodFailureObjectResult(ex);

        protected async Task<uint> GetAssociatedId()
        {
            var id = (await FirstOrDefaultAsync<Login>(x => x.Id == UserClaims.UserId))?.AssociatedId;
            if(id is null)
            {
                throw new NullReferenceException("No id is associated with this user.");
            }
            else
            {
                return id.Value;
            }
        }

        protected IDbContextTransaction BeginTransaction() => Entities.Database.BeginTransaction();
        protected async Task<TEntity> FirstOrDefaultAsync<TEntity>() where TEntity : class
        {
            return await Entities.Set<TEntity>().FirstOrDefaultAsync();
        }

        protected async Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await Entities.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        //Last Or Default
        
        protected async Task<TEntity> LastOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await Entities.Set<TEntity>().LastOrDefaultAsync(predicate);
        }

        protected async Task<TEntity> LastOrDefaultAsync<TEntity>() where TEntity : class
        {
            return await Entities.Set<TEntity>().LastOrDefaultAsync();
        }


        protected async Task<uint> CountAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return (uint)await Entities.Set<TEntity>().CountAsync(predicate);
        }

        protected uint Count<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return (uint)Entities.Set<TEntity>().Count(predicate);
        }

        protected async Task<List<TEntity>> ToListAsync<TEntity>() where TEntity : class
        {
            return await Entities.Set<TEntity>().ToListAsync();
        }

        protected async Task<List<TResult>> SelectAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> expression) where TEntity : class
        {
            return await Entities.Set<TEntity>().Select(expression).ToListAsync();
        }

        protected async Task<List<TResult>> SelectAsync<TEntity, TResult>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> expression) where TEntity : class
        {
            return await Entities.Set<TEntity>().Where(filter).Select(expression).ToListAsync();
        }

        protected async Task<List<TEntity>> ToListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await Entities.Set<TEntity>().Where(predicate).ToListAsync();
        }

        protected async Task<SuccessModel<List<TEntity>, int>> GetPageAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, PaginationBaseModel model) where TEntity : class
        {
            var filteredRecords = Entities.Set<TEntity>().Where(predicate);
            var count = await filteredRecords.CountAsync();
            var page = await filteredRecords.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
            return new SuccessModel<List<TEntity>, int>(page, count);
        }

        protected async Task<SuccessModel<List<TResult>, int>> GetPageAsync<TEntity, TResult>
            (Expression<Func<TEntity, bool>> predicate, PaginationBaseModel model, Expression<Func<TEntity, TResult>> selector) where TEntity : class, IIdentity
        {
            var count = await Entities.Set<TEntity>().Where(predicate).CountAsync();
            var page = await Entities.Set<TEntity>().Where(predicate).OrderByDescending(x => x.Id).Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize)
                .Select(selector).ToListAsync();
            return new SuccessModel<List<TResult>, int>(page, count);
        }

        protected async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return await Entities.Set<TEntity>().AnyAsync(predicate);
        }

        protected async Task InsertAsync<TEntity>(TEntity model) where TEntity : class
        {
            await Entities.Set<TEntity>().AddAsync(model);
            await Entities.SaveChangesAsync();
        }

        protected async Task InsertRangeAsync<TEntity>(IEnumerable<TEntity> model) where TEntity : class
        {
            await Entities.Set<TEntity>().AddRangeAsync(model);
            await Entities.SaveChangesAsync();
        }

        protected async Task UpdateAsync<TEntity>(TEntity model) where TEntity : class
        {
            Entities.Entry(model).State = EntityState.Modified;
            await Entities.SaveChangesAsync();
        }

        protected async Task RemoveAsync<TEntity>(TEntity model) where TEntity : class
        {
            Entities.Set<TEntity>().Remove(model);
            await Entities.SaveChangesAsync();
        }

        protected async Task RemoveRangeAsync<TEntity>(IEnumerable<TEntity> model) where TEntity : class
        {
            Entities.Set<TEntity>().RemoveRange(model);
            await Entities.SaveChangesAsync();
        }

        protected async Task DeleteAsync<TEntity>(TEntity model) where TEntity : class
        {
            Entities.Entry(model).State = EntityState.Modified;
            await Entities.SaveChangesAsync();
        }



        // Specify Date Formate...
        protected string ParseDate(DateTime? Entity)
        {
            // Code By : Kashif Shahzad
            var format = new string[]{


                // My Format
                "dd/MM/yyyy",
                "dd/MM/yyyy HH:mm:ss",


                // U-S Formate
                "M/d/yyyy",
                "M/d/yyyy HH:mm:ss",

                "yyyy-MM-dd",
                "yyyy-MM-ddTHH:mm:sszzzz",
                "MM/dd/yyyy",
                "MM/dd/yyyy HH:mm:ss",
                "dd.MM.yyyy HH:mm:ss",
                "yyyy-MM-ddTHH: mm: sszzzz"
            };
            DateTime result = DateTime.ParseExact(Entity.Value.ToShortDateString(), format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
            return result.ToShortDateString();

            // CB : Mam
            //return Entity.GetValueOrDefault().Date.ToString("d/M/yyyy");
        }
    }
}
