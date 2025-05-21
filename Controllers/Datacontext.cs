using E_commerceTechnologyWebsite.KhoLuuTru;

namespace E_commerceTechnologyWebsite.Controllers
{
    public class Datacontext
    {
        public object Users { get; internal set; }

        public static implicit operator Datacontext(DataContext v)
        {
            throw new NotImplementedException();
        }
    }
}