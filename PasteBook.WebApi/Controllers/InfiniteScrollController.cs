using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasteBook.Data;
using System.Threading.Tasks;

namespace PasteBook.WebApi.Controllers
{
    // to be revised
    [ApiController]
    [Route("infinite-scroll")]
    public class InfiniteScrollController : ControllerBase
    {
        public IUnitOfWork UnitOfWork { get; private set; }
        public InfiniteScrollController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        //[HttpGet("GetUserAccounts")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetUserAccounts([FromQuery] int pageNumber, int itemsPerScroll)
        //{
        //    var userAccounts = await UnitOfWork.UserAccountRepository.InfiniteScrollList(pageNumber, itemsPerScroll);
        //    return Ok(userAccounts);
        //}

        //[HttpGet("GetUserAccountsWithScrollInfo")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetUserAccountsWithScrollInfo([FromQuery] int pageNumber, int itemsPerScroll)
        //{
        //    var userAccounts = await UnitOfWork.UserAccountRepository.InfiniteScrollListWithInfo(pageNumber, itemsPerScroll);
        //    return Ok(userAccounts);
        //}
    }
}
