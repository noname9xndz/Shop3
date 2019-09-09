using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Shop3.Utilities.Dtos;
using Shop3.Utilities.Enum;

namespace Shop3.Utilities.Extensions
{
    public static class ApiResultExtensions
    {


        #region ErrorResult
        //public static IActionResult ErrorResult(this Controller controller, ErrorCode errorCode, HttpStatusCode statusCode)
        //{
        //    return JsonResult(
        //        new ApiResponse<object>((int)errorCode, ErrorResources.ResourceManager.GetString(errorCode.ToString())), statusCode);
        //}

        //public static IActionResult ErrorResult(this Controller controller, ErrorCode errorCode)
        //{
        //    return JsonResult(new ApiResponse<object>(
        //        (int)errorCode,
        //        ErrorResources.ResourceManager.GetString(errorCode.ToString())),
        //        HttpStatusCode.BadRequest);
        //}

        public static IActionResult ErrorResult(this Controller controller, ErrorCode errorCode)
        {
           //var strError = ((ErrorCode[])System.Enum.GetValues(typeof(ErrorCode)))
           //     .Select(c => new 
           //     {
           //         Value = (int)c,
           //         Name = c.GetDescription()
           //     })
           //     .Where(x=>x.Value == EnumExtensions.GetEnumDescription(errorCode)).ToString();

           string description = EnumExtensions.GetEnumDescription(errorCode);

           //var description = from ErrorCode n in System.Enum.GetValues(typeof(ErrorCode))

           //    select new { ID = (int)errorCode, Name = EnumExtensions.GetEnumDescription(errorCode) };


            return JsonResult(new ApiResponse<object>((int)errorCode, description),HttpStatusCode.BadRequest);
        }

        public static IActionResult ErrorResult(this Controller controller, ErrorCode errorCode,object model)
        {
            var strError = ((ErrorCode[])System.Enum.GetValues(typeof(ErrorCode)))
                .Select(c => new EnumCodeModel()
                {
                    Name = c.GetDescription()
                })
                .Where(x => x.Value == errorCode.GetHashCode()).ToString();

            return JsonResult(new ApiResponse<object>((int)errorCode, strError,model), HttpStatusCode.BadRequest);
        }

        public static IActionResult ErrorResult(this Controller controller, string errorMessage)
        {
          
            return JsonResult(new ApiResponse<object>(errorMessage), HttpStatusCode.BadRequest);
        }

        public static IActionResult ErrorResult(this Controller controller, int errorCode, string errorMessage, HttpStatusCode statusCode)
        {
            return JsonResult(new ApiResponse<object>(errorCode, errorMessage), statusCode);
        }


        public static IActionResult ErrorResult(this Controller controller, int errorCode, string errorMessage)
        {
            return JsonResult(new ApiResponse<object>(errorCode, errorMessage), HttpStatusCode.BadRequest);
        }

        public static IActionResult ErrorResult(this Controller controller, string errorMessage, HttpStatusCode statusCode)
        {
            return JsonResult(new ApiResponse<object>(errorMessage), statusCode);
        }

        //public static IActionResult ErrorResult(this Controller controller, int errorCode, string errorMessage)
        //{
        //    return JsonResult(new ApiResponse<object>(errorCode, errorMessage), HttpStatusCode.BadRequest);
        //}

        #endregion




        #region OkResult

        private static IActionResult JsonResult(object result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiJsonResult(result, statusCode);
        }

        public static IActionResult OkResult<T>(this Controller controller, T result)
        {
            return JsonResult(new ApiResponse<T>(result));
        }

        public static IActionResult OkResult<T>(this Controller controller, T result,string message)
        {
            return JsonResult(new ApiResponse<T>(result, message));
        }
        public static IActionResult OkResult<T>(this Controller controller, T result,SuccessCode successCode)
        {
            var strSuccess = ((ErrorCode[])System.Enum.GetValues(typeof(ErrorCode)))
                .Select(c => new EnumCodeModel()
                {
                    Name = c.GetDescription()
                })
                .Where(x => x.Value == successCode.GetHashCode()).ToString();

            return JsonResult(new ApiResponse<T>(result, strSuccess));
        }

        public static IActionResult OkResult(this Controller controller)
        {
            return JsonResult(new ApiResponse<object>(true));
        }

        public static IActionResult OkResult(this Controller controller, SuccessCode successCode)
        {
            var strSuccess = ((ErrorCode[])System.Enum.GetValues(typeof(ErrorCode)))
                .Select(c => new EnumCodeModel()
                {
                    Name = c.GetDescription()
                })
                .Where(x => x.Value == successCode.GetHashCode()).ToString();
            return JsonResult(new ApiResponse<object>(true, strSuccess));
        }

        //public static IActionResult OkResult(this Controller controller, object result)
        //{
        //    return JsonResult(new ApiResponse<object>(result));
        //}

        //public static IActionResult OkResult(this Controller controller, object result,string message)
        //{
        //    return JsonResult(new ApiResponse<object>(result,message));
        //}

       
        #endregion
    }
}
