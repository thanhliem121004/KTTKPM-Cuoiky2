using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.KhoLuuTru.Components
{
	public class ThuongHieuViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;
		public ThuongHieuViewComponent(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.ThuongHieu.ToListAsync());
	}
}
