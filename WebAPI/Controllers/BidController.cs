using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class BidController : BaseController
    {
        public BidController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var installerId = await GetAssociatedId();

                var bids = await ToListAsync<Bid>(x => !x.IsDeleted && x.InstallerId == installerId);
                var result = bids.Select(x => new BidModel
                {
                    Id = x.Id,
                    InstallerId = x.InstallerId,
                    Comments = x.Comments,
                    Price = x.Price,
                    SiteId = x.SiteId,
                    Status = x.Status
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(uint id)
        {
            try
            {
                var bid = await FirstOrDefaultAsync<Bid>(x => x.Id == id);
                if (bid is null)
                {
                    return NotFound();
                }
                else
                {
                    var result = new BidModel
                    {
                        Id = bid.Id,
                        InstallerId = bid.InstallerId,
                        Comments = bid.Comments,
                        Price = bid.Price,
                        SiteId = bid.SiteId,
                        Status = bid.Status
                    };
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BidModel model)
        {
            try
            {
                var installerId = await GetAssociatedId();

                var bid = new Bid
                {
                    Id = 0,
                    Comments = model.Comments,
                    Price = model.Price,
                    Status = BidStatus.Pending,
                    InstallerId = installerId,
                    SiteId = model.SiteId,
                    CreatedBy = UserClaims.UserId,
                    CreatedDate = UserClaims.DateTime,
                    IsDeleted = false,
                    TZOSCreatedBy = UserClaims.TimeZoneOffset
                };

                await InsertAsync(bid);
                return Ok();
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(uint id, [FromBody] BidModel model)
        {
            try
            {
                var bid = await FirstOrDefaultAsync<Bid>(x => x.Id == id);
                if (bid is null)
                {
                    return NotFound();
                }
                else
                {
                    bid.Price = model.Price;
                    bid.Comments = model.Comments;
                    bid.ModifiedBy = UserClaims.UserId;
                    bid.ModifiedDate = UserClaims.DateTime;
                    bid.TZOSModifiedBy = UserClaims.TimeZoneOffset;

                    await UpdateAsync(bid);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }

        [HttpPost("Page")]
        public async Task<IActionResult> PostPage([FromBody] BidPaginationModel paginationModel)
        {
            try
            {
                var installerId = await GetAssociatedId();
                Expression<Func<Bid, bool>> filter = x => !x.IsDeleted && x.InstallerId == installerId;

                var page = await GetPageAsync(filter, paginationModel, x => new BidModel {
                    Id = x.Id,
                    Price = x.Price,
                    Comments = x.Comments
                });

                return Ok(page.MainData, page.OtherData);
            }
            catch (Exception ex)
            {
                return MethodFailure(ex);
            }
        }
    }
}
