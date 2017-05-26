using MSTech.GestaoEscolar.Web.WebProject;
using System.Web.Mvc;

namespace GestaoEscolar.Api.Controllers.Base
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ViewBag.Version = new MotherMasterPage()._VS_versao;
            ViewBag.Title = "SGP - API";
        }
	}
}