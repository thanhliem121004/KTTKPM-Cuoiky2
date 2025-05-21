using E_commerceTechnologyWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.KhoLuuTru
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			// Xóa toàn bộ dữ liệu hiện có trong các bảng
			//_context.SanPham.RemoveRange(_context.SanPham);
			//_context.ThuongHieu.RemoveRange(_context.ThuongHieu);
			//_context.TheLoai.RemoveRange(_context.TheLoai);
			if (!_context.SanPham.Any()) /*nếu mà trong cái SamPham chưa có dữ liệu thì thực hiện add dữ liệu vào*/
			{
				ThuongHieuModel apple = new ThuongHieuModel { Name = "Apple", Slug = "apple", Description = "Dẫn đầu công nghệ, thiết kế sang trọng, hệ sinh thái liền mạch.", Status = 1 };
				ThuongHieuModel samsung = new ThuongHieuModel { Name = "Samsung", Slug = "samsung", Description = "Đổi mới không ngừng, công nghệ tiên tiến, trải nghiệm đa dạng.", Status = 1 };
				ThuongHieuModel huawei = new ThuongHieuModel { Name = "Huawei", Slug = "huawei", Description = "Công nghệ tiên phong, camera đột phá, hệ sinh thái riêng.", Status = 1 };
				ThuongHieuModel xiaomi = new ThuongHieuModel { Name = "Xiaomi", Slug = "xiaomi", Description = "Giá cả phải chăng, cấu hình mạnh mẽ, giao diện tùy biến cao.", Status = 1 };
				ThuongHieuModel oppo = new ThuongHieuModel { Name = "Oppo", Slug = "oppo", Description = "Thiết kế thời thượng, camera selfie ấn tượng, sạc nhanh vượt trội.", Status = 1 };
				ThuongHieuModel intel = new ThuongHieuModel { Name = "Intel", Slug = "intel", Description = "Hiệu năng mạnh mẽ, công nghệ tiên tiến, ổn định.", Status = 1 };
				ThuongHieuModel amd = new ThuongHieuModel { Name = "AMD", Slug = "amd", Description = "Cạnh tranh về giá, hiệu năng ấn tượng, đa dạng lựa chọn.", Status = 1 };
				ThuongHieuModel nvidia = new ThuongHieuModel { Name = "Nvidia", Slug = "nvidia", Description = "Đồ họa đỉnh cao, công nghệ Ray Tracing, AI vượt trội.", Status = 1 };
				ThuongHieuModel vivo = new ThuongHieuModel { Name = "Vivo", Slug = "vivo", Description = "Âm thanh chất lượng, camera chụp đêm tốt, thiết kế mỏng nhẹ.", Status = 1 };
				ThuongHieuModel asus = new ThuongHieuModel { Name = "Asus", Slug = "asus", Description = "Laptop đa dạng, hiệu năng ổn định, thiết kế tinh tế.", Status = 1 };
				ThuongHieuModel dell = new ThuongHieuModel { Name = "Dell", Slug = "dell", Description = "Laptop bền bỉ, hiệu năng làm việc cao, hỗ trợ doanh nghiệp.", Status = 1 };
				ThuongHieuModel hp = new ThuongHieuModel { Name = "HP", Slug = "hp", Description = "Máy in chất lượng, laptop phổ thông, giá cả hợp lý.", Status = 1 };
				ThuongHieuModel logitech = new ThuongHieuModel { Name = "Logitech", Slug = "logitech", Description = "Phụ kiện máy tính đa dạng, thiết kế hiện đại, chất lượng tốt.", Status = 1 };
				ThuongHieuModel razer = new ThuongHieuModel { Name = "Razer", Slug = "razer", Description = "Thiết bị chơi game chuyên nghiệp, hiệu năng cao, thiết kế hầm hố.", Status = 1 };
				ThuongHieuModel corsair = new ThuongHieuModel { Name = "Corsair", Slug = "corsair", Description = "Linh kiện PC cao cấp, tản nhiệt hiệu quả, thiết kế đẹp mắt.", Status = 1 };
				ThuongHieuModel rog = new ThuongHieuModel { Name = "ROG", Slug = "rog", Description = "Laptop gaming, hiệu năng cao, tản nhiệt tốt.", Status = 1 };
				ThuongHieuModel pavilion = new ThuongHieuModel { Name = "Pavilion", Slug = "pavilion", Description = "Laptop đa dụng, giá tốt, thiết kế hiện đại.", Status = 1 };
				ThuongHieuModel basilisk = new ThuongHieuModel { Name = "Basilisk", Slug = "basilisk", Description = "Chuột gaming, DPI cao, nút bấm tùy chỉnh.", Status = 1 };
				ThuongHieuModel vengeance = new ThuongHieuModel { Name = "Vengeance", Slug = "vengeance", Description = "RAM tốc độ cao, tản nhiệt tốt, RGB đẹp mắt.", Status = 1 };
				/*-------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
				// Apple
				TheLoaiModel iphone = new TheLoaiModel { Name = "iPhone", Slug = "iphone", Description = "Điện thoại thông minh cao cấp, camera xuất sắc, hệ điều hành iOS mượt mà.", Status = 1 };
				TheLoaiModel ipad = new TheLoaiModel { Name = "iPad", Slug = "ipad", Description = "Máy tính bảng đa năng, màn hình sắc nét, hiệu năng mạnh mẽ.", Status = 1 };
				TheLoaiModel appleWatch = new TheLoaiModel { Name = "Apple Watch", Slug = "apple-watch", Description = "Đồng hồ thông minh theo dõi sức khỏe, thiết kế sang trọng, tích hợp liền mạch với iPhone.", Status = 1 };
				TheLoaiModel airpods = new TheLoaiModel { Name = "AirPods", Slug = "airpods", Description = "Tai nghe không dây âm thanh chất lượng, kết nối nhanh chóng, thiết kế nhỏ gọn.", Status = 1 };
				TheLoaiModel macbook = new TheLoaiModel { Name = "Macbook", Slug = "macbook", Description = "Hiệu năng đỉnh cao, thiết kế tinh tế, hệ điều hành mượt mà.", Status = 1 };
				// Samsung
				TheLoaiModel galaxyS = new TheLoaiModel { Name = "Galaxy S", Slug = "galaxy-s", Description = "Điện thoại flagship, màn hình Dynamic AMOLED, camera đa năng.", Status = 1 };
				TheLoaiModel galaxyNote = new TheLoaiModel { Name = "Galaxy Note", Slug = "galaxy-note", Description = "Điện thoại màn hình lớn, bút S Pen, hiệu năng mạnh mẽ.", Status = 1 };
				TheLoaiModel galaxyTab = new TheLoaiModel { Name = "Galaxy Tab", Slug = "galaxy-tab", Description = "Máy tính bảng Android, màn hình lớn, hỗ trợ bút S Pen.", Status = 1 };
				TheLoaiModel galaxyWatch = new TheLoaiModel { Name = "Galaxy Watch", Slug = "galaxy-watch", Description = "Đồng hồ thông minh theo dõi sức khỏe, thiết kế thể thao, hệ điều hành Tizen.", Status = 1 };
				TheLoaiModel galaxyBuds = new TheLoaiModel { Name = "Galaxy Buds", Slug = "galaxy-buds", Description = "Tai nghe không dây chống ồn, âm thanh chất lượng, thiết kế nhỏ gọn.", Status = 1 };
				// Huawei
				TheLoaiModel mateSeries = new TheLoaiModel { Name = "Mate Series", Slug = "mate-series", Description = "Điện thoại cao cấp, camera Leica, pin trâu, hiệu năng ổn định.", Status = 1 };
				TheLoaiModel pSeries = new TheLoaiModel { Name = "P Series", Slug = "p-series", Description = "Điện thoại tập trung vào nhiếp ảnh, thiết kế thời trang, hiệu năng mạnh mẽ.", Status = 1 };
				TheLoaiModel novaSeries = new TheLoaiModel { Name = "Nova Series", Slug = "nova-series", Description = "Điện thoại tầm trung, thiết kế đẹp, camera selfie tốt.", Status = 1 };
				TheLoaiModel matepad = new TheLoaiModel { Name = "MatePad", Slug = "matepad", Description = "Máy tính bảng HarmonyOS, hỗ trợ bút M-Pencil, hiệu năng tốt.", Status = 1 };
				TheLoaiModel freebuds = new TheLoaiModel { Name = "FreeBuds", Slug = "freebuds", Description = "Tai nghe không dây chống ồn, âm thanh chất lượng, thiết kế thoải mái.", Status = 1 };
				// Xiaomi
				TheLoaiModel miSeries = new TheLoaiModel { Name = "Mi Series", Slug = "mi-series", Description = "Điện thoại flagship, cấu hình mạnh mẽ, camera ấn tượng.", Status = 1 };
				TheLoaiModel redmiSeries = new TheLoaiModel { Name = "Redmi Series", Slug = "redmi-series", Description = "Điện thoại giá rẻ, cấu hình tốt, pin lớn.", Status = 1 };
				TheLoaiModel pocoSeries = new TheLoaiModel { Name = "POCO Series", Slug = "poco-series", Description = "Điện thoại hiệu năng cao, giá phải chăng, tập trung vào game thủ.", Status = 1 };
				TheLoaiModel miPad = new TheLoaiModel { Name = "Mi Pad", Slug = "mi-pad", Description = "Máy tính bảng MIUI, màn hình lớn, hiệu năng ổn định.", Status = 1 };
				TheLoaiModel miBand = new TheLoaiModel { Name = "Mi Band", Slug = "mi-band", Description = "Vòng đeo tay thông minh theo dõi sức khỏe, giá rẻ, nhiều tính năng.", Status = 1 };
				// Oppo
				TheLoaiModel findSeries = new TheLoaiModel { Name = "Find Series", Slug = "find-series", Description = "Điện thoại flagship, thiết kế đẹp, camera ấn tượng, sạc nhanh.", Status = 1 };
				TheLoaiModel renoSeries = new TheLoaiModel { Name = "Reno Series", Slug = "reno-series", Description = "Điện thoại tầm trung, thiết kế thời thượng, camera selfie tốt.", Status = 1 };
				TheLoaiModel aSeries = new TheLoaiModel { Name = "A Series", Slug = "a-series", Description = "Điện thoại giá rẻ, pin lớn, cấu hình ổn định.", Status = 1 };
				TheLoaiModel oppoPad = new TheLoaiModel { Name = "Oppo Pad", Slug = "oppo-pad", Description = "Máy tính bảng ColorOS, màn hình lớn, hiệu năng tốt.", Status = 1 };
				TheLoaiModel enco = new TheLoaiModel { Name = "Enco", Slug = "enco", Description = "Tai nghe không dây chống ồn, âm thanh chất lượng, thiết kế thời trang.", Status = 1 };
				// Intel & AMD
				TheLoaiModel cpu = new TheLoaiModel { Name = "CPU", Slug = "cpu", Description = "Bộ vi xử lý máy tính, quyết định hiệu năng tổng thể.", Status = 1 };
				// Nvidia
				TheLoaiModel gpu = new TheLoaiModel { Name = "GPU", Slug = "gpu", Description = "Card đồ họa, xử lý hình ảnh và video, quan trọng cho game và đồ họa.", Status = 1 };
				// Vivo
				TheLoaiModel vSeries = new TheLoaiModel { Name = "V Series", Slug = "v-series", Description = "Điện thoại tầm trung, camera chụp đêm tốt, thiết kế mỏng nhẹ.", Status = 1 };
				TheLoaiModel ySeries = new TheLoaiModel { Name = "Y Series", Slug = "y-series", Description = "Điện thoại giá rẻ, thiết kế trẻ trung, camera tốt.", Status = 1 };
				TheLoaiModel xSeries = new TheLoaiModel { Name = "X Series", Slug = "x-series", Description = "Điện thoại cao cấp, camera gimbal, thiết kế độc đáo.", Status = 1 };
				// Asus
				TheLoaiModel zenbook = new TheLoaiModel { Name = "Zenbook", Slug = "zenbook", Description = "Laptop mỏng nhẹ, thiết kế sang trọng, hiệu năng cao.", Status = 1 };
				TheLoaiModel vivobook = new TheLoaiModel { Name = "Vivobook", Slug = "vivobook", Description = "Laptop đa dụng, giá tốt, thiết kế hiện đại.", Status = 1 };
				TheLoaiModel rog1 = new TheLoaiModel { Name = "ROG", Slug = "rog", Description = "Laptop gaming, hiệu năng cao, tản nhiệt tốt.", Status = 1 };
				// Dell
				TheLoaiModel xps1 = new TheLoaiModel { Name = "XPS", Slug = "xps", Description = "Laptop mỏng nhẹ, màn hình đẹp, hiệu năng cao.", Status = 1 };
				TheLoaiModel inspiron = new TheLoaiModel { Name = "Inspiron", Slug = "inspiron", Description = "Laptop đa dụng, giá tốt, phù hợp nhiều nhu cầu.", Status = 1 };
				TheLoaiModel latitude = new TheLoaiModel { Name = "Latitude", Slug = "latitude", Description = "Laptop doanh nghiệp, bền bỉ, bảo mật cao.", Status = 1 };
				// HP
				TheLoaiModel pavilion1 = new TheLoaiModel { Name = "Pavilion", Slug = "pavilion", Description = "Laptop đa dụng, giá tốt, thiết kế hiện đại.", Status = 1 };
				TheLoaiModel envy = new TheLoaiModel { Name = "Envy", Slug = "envy", Description = "Laptop cao cấp, thiết kế đẹp, hiệu năng mạnh mẽ.", Status = 1 };
				TheLoaiModel probook = new TheLoaiModel { Name = "ProBook", Slug = "probook", Description = "Laptop doanh nghiệp, bền bỉ, hiệu năng ổn định.", Status = 1 };
				TheLoaiModel mayIn = new TheLoaiModel { Name = "Máy in", Slug = "may-in", Description = "Thiết bị in ấn văn bản và hình ảnh.", Status = 1 };
				// Logitech
				TheLoaiModel banPhim = new TheLoaiModel { Name = "Bàn phím", Slug = "ban-phim", Description = "Thiết bị nhập liệu văn bản và điều khiển máy tính.", Status = 1 };
				TheLoaiModel taiNghe = new TheLoaiModel { Name = "Tai nghe", Slug = "tai-nghe", Description = "Thiết bị nghe âm thanh từ máy tính hoặc điện thoại.", Status = 1 };
				TheLoaiModel webcam = new TheLoaiModel { Name = "Webcam", Slug = "webcam", Description = "Thiết bị ghi hình và truyền trực tiếp video.", Status = 1 };
				// Razer
				TheLoaiModel chuotGaming = new TheLoaiModel { Name = "Chuột gaming", Slug = "chuot-gaming", Description = "Chuột chuyên dụng cho chơi game, DPI cao, nút bấm tùy chỉnh.", Status = 1 };
				TheLoaiModel banPhimGaming = new TheLoaiModel { Name = "Bàn phím gaming", Slug = "ban-phim-gaming", Description = "Bàn phím cơ chuyên dụng cho chơi game, switch đa dạng, đèn LED RGB.", Status = 1 };
				TheLoaiModel taiNgheGaming = new TheLoaiModel { Name = "Tai nghe gaming", Slug = "tai-nghe-gaming", Description = "Tai nghe chuyên dụng cho chơi game, âm thanh vòm, mic chất lượng cao.", Status = 1 };
				// Corsair
				TheLoaiModel ram = new TheLoaiModel { Name = "RAM", Slug = "ram", Description = "Bộ nhớ truy cập ngẫu nhiên, ảnh hưởng đến tốc độ xử lý dữ liệu.", Status = 1 };
				TheLoaiModel tanNhiet = new TheLoaiModel { Name = "Tản nhiệt", Slug = "tan-nhiet", Description = "Thiết bị làm mát cho CPU hoặc GPU, giúp máy tính hoạt động ổn định.", Status = 1 };
				TheLoaiModel nguonMayTinh = new TheLoaiModel { Name = "Nguồn máy tính", Slug = "nguon-may-tinh", Description = "Cung cấp điện năng cho các linh kiện trong máy tính.", Status = 1 };
				/*-------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

				var sanPhammoi = new List<SanPhamModel> {
					new SanPhamModel
					{
						Name = "Apple MacBook Air M1",
						Slug = "macbook",
						Description = "Hiệu năng đỉnh cao, thiết kế tinh tế, hệ điều hành mượt mà.",
						Image = "Apple MacBook Air M1.jpg",
						TheLoai = macbook,
						ThuongHieu = apple,
						Price = 17990000
					},
					new SanPhamModel
					{
						Name = "iPhone 15 Pro Max",
						Slug = "iphone-15-pro-max",
						Description = "Điện thoại thông minh cao cấp nhất của Apple, camera xuất sắc, chip A17 Bionic mạnh mẽ.",
						Image = "IP-15-promax.jpg",
						TheLoai = iphone,
						ThuongHieu = apple,
						Price = 32990000
					},
					new SanPhamModel
					{
						Name = "Samsung Galaxy S23 Ultra",
						Slug = "galaxy-s23-ultra",
						Description = "Điện thoại flagship của Samsung, màn hình Dynamic AMOLED 2X, camera 200MP.",
						Image = "galaxy-s23-ultra.jpg",
						TheLoai = galaxyS,
						ThuongHieu = samsung,
						Price = 25990000
					},
					new SanPhamModel
					{
						Name = "iPhone 16 Pro Max",
						Slug = "iphone-16-pro-max",
						Description = "Điện thoại thông minh cao cấp nhất của Apple, camera xuất sắc, chip A17 Bionic mạnh mẽ.",
						Image = "5n1dTTOB.jpg",
						TheLoai = iphone,
						ThuongHieu = apple,
						Price = 32990000
					},
					new SanPhamModel
					{
						Name = "Samsung Galaxy Z Fold3 5G",
						Slug = "galaxy-z-fold3-5g",
						Description = "Điện thoại màn hình gập cao cấp, hỗ trợ bút S Pen.",
						Image = "galaxy-z-fold3.jpg",
						TheLoai = galaxyNote,
						ThuongHieu = samsung,
						Price = 34990000
					},
					new SanPhamModel
					{
						Name = "Samsung Galaxy Tab S8 Ultra",
						Slug = "galaxy-tab-s8-ultra",
						Description = "Máy tính bảng màn hình lớn 14.6 inch, hiệu năng mạnh mẽ.",
						Image = "galaxy-tab-s8-ultra.jpg",
						TheLoai = galaxyTab,
						ThuongHieu = samsung,
						Price = 25990000
					},
					new SanPhamModel
					{
						Name = "Huawei Mate 40 Pro",
						Slug = "huawei-mate-40-pro",
						Description = "Điện thoại cao cấp với camera Leica, chip Kirin 9000.",
						Image = "huawei-mate-40-pro.jpg",
						TheLoai = mateSeries,
						ThuongHieu = huawei,
						Price = 19990000
					},
					new SanPhamModel
					{
						Name = "Xiaomi Mi 11 Ultra",
						Slug = "xiaomi-mi-11-ultra",
						Description = "Flagship với camera 50MP, màn hình phụ ở mặt sau.",
						Image = "xiaomi-mi-11-ultra.jpg",
						TheLoai = miSeries,
						ThuongHieu = xiaomi,
						Price = 18990000
					},
					new SanPhamModel
					{
						Name = "OPPO Find X5 Pro",
						Slug = "oppo-find-x5-pro",
						Description = "Điện thoại cao cấp với camera Hasselblad, sạc siêu nhanh.",
						Image = "oppo-find-x5-pro.jpg",
						TheLoai = findSeries,
						ThuongHieu = oppo,
						Price = 22990000
					},
					new SanPhamModel
					{
						Name = "Intel Core i9-12900K",
						Slug = "intel-core-i9-12900k",
						Description = "CPU desktop hiệu năng cao với 16 nhân, 24 luồng.",
						Image = "intel-i9-12900k.jpg",
						TheLoai = cpu,
						ThuongHieu = intel,
						Price = 14990000
					},
					new SanPhamModel
					{
						Name = "AMD Ryzen 9 5950X",
						Slug = "amd-ryzen-9-5950x",
						Description = "CPU 16 nhân 32 luồng cho hiệu năng đa nhiệm mạnh mẽ.",
						Image = "amd-ryzen-9-5950x.jpg",
						TheLoai = cpu,
						ThuongHieu = amd,
						Price = 16990000
					},
					new SanPhamModel
					{
						Name = "NVIDIA GeForce RTX 3080",
						Slug = "nvidia-geforce-rtx-3080",
						Description = "Card đồ họa cao cấp với Ray Tracing và DLSS.",
						Image = "nvidia-rtx-3080.jpg",
						TheLoai = gpu,
						ThuongHieu = nvidia,
						Price = 21990000
					},
					new SanPhamModel
					{
						Name = "Vivo X70 Pro+",
						Slug = "vivo-x70-pro-plus",
						Description = "Điện thoại cao cấp với camera gimbal, chip Snapdragon 888+.",
						Image = "vivo-x70-pro-plus.jpg",
						TheLoai = xSeries,
						ThuongHieu = vivo,
						Price = 17990000
					},
					new SanPhamModel
					{
						Name = "ASUS ROG Zephyrus G15",
						Slug = "asus-rog-zephyrus-g15",
						Description = "Laptop gaming mỏng nhẹ, RTX 3080, Ryzen 9 5900HS.",
						Image = "asus-rog-zephyrus-g15.jpg",
						TheLoai = rog1,
						ThuongHieu = asus,
						Price = 44990000
					},
					new SanPhamModel
					{
						Name = "Dell XPS 15 (2021)",
						Slug = "dell-xps-15-2021",
						Description = "Laptop cao cấp với màn hình OLED, Intel Core i9.",
						Image = "dell-xps-15-2021.jpg",
						TheLoai = xps1,
						ThuongHieu = dell,
						Price = 52990000
					},
					new SanPhamModel
					{
						Name = "HP Spectre x360 14",
						Slug = "hp-spectre-x360-14",
						Description = "Laptop 2-in-1 cao cấp với màn hình 3:2, Intel Evo.",
						Image = "hp-spectre-x360-14.jpg",
						TheLoai = envy,
						ThuongHieu = hp,
						Price = 37990000
					},
					new SanPhamModel
					{
						Name = "Logitech MX Master 3",
						Slug = "logitech-mx-master-3",
						Description = "Chuột không dây cao cấp cho văn phòng và sáng tạo.",
						Image = "logitech-mx-master-3.jpg",
						TheLoai = chuotGaming,
						ThuongHieu = logitech,
						Price = 2490000
					},
					new SanPhamModel
					{
						Name = "Razer BlackWidow V3 Pro",
						Slug = "razer-blackwidow-v3-pro",
						Description = "Bàn phím cơ gaming không dây cao cấp.",
						Image = "razer-blackwidow-v3-pro.jpg",
						TheLoai = banPhimGaming,
						ThuongHieu = razer,
						Price = 4990000
					},
					new SanPhamModel
					{
						Name = "Corsair Vengeance RGB Pro 32GB",
						Slug = "corsair-vengeance-rgb-pro-32gb",
						Description = "Bộ RAM DDR4 32GB (2x16GB) với đèn RGB.",
						Image = "corsair-vengeance-rgb-pro-32gb.jpg",
						TheLoai = ram,
						ThuongHieu = corsair,
						Price = 3990000
					},
					new SanPhamModel
					{
						Name = "Apple AirPods Pro",
						Slug = "airpods-pro",
						Description = "Tai nghe true wireless với chống ồn chủ động.",
						Image = "airpods-pro.jpg",
						TheLoai = airpods,
						ThuongHieu = apple,
						Price = 5490000
					},
					new SanPhamModel
					{
						Name = "Samsung Galaxy Buds Pro",
						Slug = "galaxy-buds-pro",
						Description = "Tai nghe true wireless với chất âm cân bằng.",
						Image = "galaxy-buds-pro.jpg",
						TheLoai = galaxyBuds,
						ThuongHieu = samsung,
						Price = 3990000
					},
					new SanPhamModel
					{
						Name = "Samsung Galaxy S24 Ultra",
						Slug = "galaxy-s24-ultra",
						Description = "Điện thoại flagship của Samsung, màn hình Dynamic AMOLED 2X, camera 200MP.",
						Image = "new-samsung-s24-ultra.jpg",
						TheLoai = galaxyS,
						ThuongHieu = samsung,
						Price = 25990000
					},
					new SanPhamModel
					{
						Name = "Samsung Galaxy S22 Ultra",
						Slug = "galaxy-s22-ultra",
						Description = "Điện thoại flagship của Samsung, màn hình Dynamic AMOLED 2X, camera 200MP.",
						Image = "s22-ultra.jpg",
						TheLoai = galaxyS,
						ThuongHieu = samsung,
						Price = 17900000
					},
					};
				//.Where(p=>!_context.SanPham.Any(sp => sp.Name == p.Name))
				foreach (var sp in sanPhammoi)
				{
					if (!_context.SanPham.Any(p => p.Name == sp.Name))
					{
						_context.SanPham.Add(sp);
					}
				};
				_context.SaveChanges();
			}
		}
	}
}
