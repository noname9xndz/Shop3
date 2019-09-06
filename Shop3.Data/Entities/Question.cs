using Shop3.Data.Enums;
using Shop3.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop3.Data.Entities
{
    [Table("Questions")]
    public class Question : DomainEntity<int>
    {
        public Question()
        {

        }
        public Question(string title, string content, int displayOrder, Status status)
        {
            Title = title;
            Content = content;
            DisplayOrder = displayOrder;
            Status = status;
        }

        public Question(int id, string title, string content, int displayOrder, Status status)
        {
            Id = id;
            Title = title;
            Content = content;
            DisplayOrder = displayOrder;
            Status = status;
        }
        [StringLength(500)]
        public string Title { get; set; }
        public string Content { get; set; }
        public int DisplayOrder { get; set; }

        public Status Status { set; get; }
    }
}
