using System.Threading.Tasks;

namespace Shop3.Services
{
    // tạo ra 1 chuỗi html từ ViewName và model tương tự như Mustache truyền vào 1 template html và data => tạo ra view
    // sử dụng cơ chế render của asp.net mvc để binding dữ liệu
    public interface IViewRenderService
    {
        /// <summary>
        ///     Render Razor View as String 
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model">   </param>
        /// <returns></returns>
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
