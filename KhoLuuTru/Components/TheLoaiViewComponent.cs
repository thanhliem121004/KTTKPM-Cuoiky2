using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.KhoLuuTru.Components
{
	public class TheLoaiViewComponent : ViewComponent
	{
		private readonly DataContext _dataContext;
		public TheLoaiViewComponent(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.TheLoai.ToListAsync());
	}
}
