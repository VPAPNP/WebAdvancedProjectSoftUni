using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EShopWebApp.Core.DataConstants.GeneralApplicationConstants.Identity;

namespace EShopWebApp.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class BaseAdminController : Controller
    {

    }
}
