using System.Web.Mvc;

namespace WLCaxtonMvcWebPortal.Areas.UserActvity
{
    public class UserActvityAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UserActvity";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "UserActvity_default",
                "UserActvity/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}