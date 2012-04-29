using System.Web.Mvc;
using NGM.OpenAuthentication.Core;
using Orchard.Mvc.Filters;

namespace NGM.OpenAuthentication.Filters {
    public class StateFilter : FilterProvider, IActionFilter, IResultFilter {
        private const string TempDataState = "openauthenticationstate";

        private readonly IStateBag _stateBag;

        public StateFilter(IStateBag stateBag) {
            _stateBag = stateBag;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.Controller == null)
                return;

            var tempData = filterContext.Controller.TempData;

            object parameters;
            if (!tempData.TryGetValue(TempDataState, out parameters)) {
                return;
            }

            var castedParameters = parameters as OpenAuthenticationParameters;

            if (castedParameters == null)
                return;

            _stateBag.Parameters = castedParameters;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) {
            var tempData = filterContext.Controller.TempData;

            // don't touch temp data if there's no work to perform
            if (_stateBag.Parameters == null) {
                if (tempData.ContainsKey(TempDataState))
                    tempData.Remove(TempDataState);

                return;
            }

            // assign values into temp data
            // string data type used instead of complex array to be session-friendly
            tempData[TempDataState] = _stateBag.Parameters;

            filterContext.Controller.TempData.Keep(TempDataState);
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            //var viewResult = filterContext.Result as ViewResultBase;

            //// if it's not a view result, a redirect for example
            //if (viewResult == null)
            //    return;

            //var parameters = viewResult.TempData[TempDataState] as OpenAuthenticationParameters;
            //if (parameters == null)
            //    return;// nothing to do, really

            //_stateBag.Parameters = parameters;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
        }
    }
}