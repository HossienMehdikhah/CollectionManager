using CollectionManagerWebService.Models;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
namespace CollectionManagerWebService.Endpoints;

[Route("api/[controller]")]
[ApiController]
public class DownloadLinkController(Context context) : ControllerBase
{
    [HttpPost("SendDownloadLink")]
    public async Task<Result> SendDownloadLinkAsync([FromBody] DownloadGameLinksDTO game)
    {
        context.Games.Add(new Game 
        { 
            Name = game.GameName,
            Praiority = game.Praiority,
            Links = game.Links.Select(x=>new GameLink { Link = x}).ToList(),
        });
        var result = await context.SaveChangesAsync();
        return Result.OkIf(result > 0,"Game Added To Download Queue.");
    }
}