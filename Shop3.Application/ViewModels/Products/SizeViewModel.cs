using System.ComponentModel.DataAnnotations;

namespace Shop3.Application.ViewModels.Products
{
    public class SizeViewModel
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string Name
        {
            get; set;
        }
    }
}
