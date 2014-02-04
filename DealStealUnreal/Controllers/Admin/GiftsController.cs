using DealStealUnreal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DealStealUnreal.Controllers.Admin
{
    public class GiftsController : Controller
    {
        public struct Model
        {
            public int page;
            public int pagesize;
            public int totalGifts;
            public List<Gift> gifts;
        };

        DSURepository db;

        public GiftsController()
        {
            db = new DSURepository();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            int pagesize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["pagesize"].ToString());
            int page = (RouteData.Values["pageNumber"] != null) ? Convert.ToInt32(RouteData.Values["pageNumber"].ToString()) : 1;
           
            Model model;
            model.page = page;
            model.totalGifts = db.Gifts.Count();
            model.gifts = db.Gifts.Skip(page * pagesize).ToList();
            model.pagesize = pagesize;

            return View("../Admin/Gifts", model);
        }

    }
}
