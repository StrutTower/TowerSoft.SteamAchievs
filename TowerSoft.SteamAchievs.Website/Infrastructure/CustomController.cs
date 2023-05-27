using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Website.Infrastructure {
    public class CustomController : Controller {
        /// <summary>
        /// Checks if the current request was made using Ajax
        /// </summary>
        public bool IsAjaxRequest {
            get {
                if (Request != null && Request.Headers != null && Request.Headers.ContainsKey("X-Requested-With"))
                    return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                return false;
            }
        }

        /// <summary>
        /// Returns a view with a simple message
        /// </summary>
        /// <param name="message">Message to display on the returned page.</param>
        public IActionResult Message(string message) {
            return View("Message", model: message);
        }

        /// <summary>
        /// Renders the view matching the current controller action name to a string.
        /// </summary>
        /// <typeparam name="TModel">Model Type</typeparam>
        /// <param name="model">View model</param>
        /// <param name="partialView">Should the view be rendered as a partial view</param>
        public async Task<string> RenderViewAsync<TModel>(TModel model, bool partialView = true) {
            return await RenderViewAsync(null, model, partialView);
        }

        /// <summary>
        /// Renders the specified view and model to a string.
        /// </summary>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <param name="viewName">Name of the view to render</param>
        /// <param name="model">View model</param>
        /// <param name="partialView">Should the view be rendered as a partial view</param>
        public async Task<string> RenderViewAsync<TModel>(string viewName, TModel model, bool partialView = true) {
            if (string.IsNullOrEmpty(viewName)) {
                viewName = ControllerContext.ActionDescriptor.ActionName;
            }

            ViewData.Model = model;

            using (var writer = new StringWriter()) {
                IViewEngine viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(ControllerContext, viewName, !partialView);

                if (viewResult.Success == false) {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Returns a json object with a list of all of the errors in the ModelState
        /// </summary>
        /// <param name="modelState">Current ModelState</param>
        public IActionResult JsonModelErrorList(ModelStateDictionary modelState) {
            List<string> errors = new List<string>();
            foreach (var state in modelState) {
                var test = state.Value;
                foreach (var error in state.Value.Errors) {
                    errors.Add(error.ErrorMessage);
                }
            }

            return Json(new {
                success = false,
                message = "The object model is not valid:<br />" + string.Join("<br />", errors)
            });
        }
    }
}
