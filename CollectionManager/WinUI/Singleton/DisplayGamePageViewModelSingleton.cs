using CollectionManager.Core.Models;
using Microsoft.Extensions.Options;
namespace CollectionManager.WinUI.Singleton;

public class DisplayGamePageViewModelSingleton(IOptions<CollectionManagerOption> options)
{
	private readonly List<(Uri, GamePageDTO)> _cashlist = [];
	public GamePageDTO? GetPage(Uri uri)
	{
		return _cashlist.SingleOrDefault(x=>x.Item1.Equals(uri)).Item2;
	}
    public void SetPage(GamePageDTO gamePageDTO)
    {
		var maxCash = options.Value.MaxPageCash;
        if (_cashlist.Count <=maxCash) 
			_cashlist.Add((gamePageDTO.URL!, gamePageDTO));
		else
		{
			_cashlist.RemoveAt((int)maxCash);
			_cashlist.Insert(0, (gamePageDTO.URL!, gamePageDTO));			
		}
    }
}
