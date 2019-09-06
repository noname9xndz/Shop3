using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop3.WebApi.Controllers
{
    [Route("api/[controller]")] // router
    [Produces("application/json")] // dữ liệu trả về luon là json
                                   // ajax chỉ cung cấp get và post => resful cung cấp CRUD
    public class ApiControllerBase : Controller
    {

    }
    //public class ApiControllerBase : Controller
    //{
    //    private IErrorService _errorService;

    //    public ApiControllerBase(IErrorService errorService)
    //    {
    //        this._errorService = errorService;
    //    }

    //    protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage requestMessage,
    //        Func<HttpResponseMessage> function)
    //    {
    //        HttpResponseMessage response = null;
    //        try
    //        {
    //            response = function.Invoke();
    //        }
    //        catch (DbEntityValidationException ex)
    //        {
    //            foreach (var eve in ex.EntityValidationErrors)
    //            {
    //                // Trace.WriteLine khi degbug sẽ ra cửa sổ output
    //                Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
    //                foreach (var ve in eve.ValidationErrors)
    //                {
    //                    Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
    //                }
    //            }
    //            LogError(ex);
    //            response = requestMessage.CreateResponse(HttpStatusCode.BadRequest);
    //        }
    //        // bắt lỗi thao tác với Db
    //        catch (DbUpdateException dbEx)
    //        {
    //            LogError(dbEx);
    //            response = requestMessage.CreateResponse(HttpStatusCode.BadRequest);
    //        }
    //        // bắt lỗi khác
    //        catch (Exception ex)
    //        {
    //            LogError(ex);
    //           // HttpRequestMessage request = new HttpRequestMessage();
    //            response = requestMessage.CreateResponse(HttpStatusCode.BadRequest);
    //        }
    //        return response;
    //    }

    //    // log lỗi vào bảng lỗi trong Database
    //    private void LogError(Exception ex)
    //    {
    //        try
    //        {
    //            Error error = new Error
    //            {
    //                DateCreated = DateTime.Now,
    //                DateModified = DateTime.Now,
    //                Message = ex.Message,
    //                StackTrace = ex.StackTrace
    //            };

    //            _errorService.Create(error);
    //            _errorService.Save();
    //        }
    //        catch
    //        {

    //        }
    //    }
    //}
}

