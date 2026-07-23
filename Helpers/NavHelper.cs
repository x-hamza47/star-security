using Microsoft.AspNetCore.Mvc.Rendering;

namespace Star_Security.Helpers
{
    public class NavHelper
    {
        public static bool IsRoute(ViewContext viewContext, string controller, string action = "")
        {
            var routeController = viewContext.RouteData.Values["Controller"]?.ToString();
            var routeAction = viewContext.RouteData.Values["Action"]?.ToString();

            if (!string.IsNullOrEmpty(action))
                return controller == routeController && action == routeAction;

            return controller == routeController;
        }

        public static bool IsAnyRoute(ViewContext viewContext, params (string controller, string action)[] items)
        {
            var routeController = viewContext.RouteData.Values["Controller"]?.ToString();
            var routeAction = viewContext.RouteData.Values["Action"]?.ToString();

            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.action))
                {
                    if (item.controller == routeController && item.action == routeAction)
                        return true;
                }
                else
                {
                    if (item.controller == routeController)
                        return true;
                }
            }
            return false;
        }

    }
}
