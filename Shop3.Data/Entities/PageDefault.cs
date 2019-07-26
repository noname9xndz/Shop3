using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Shop3.Data.Enums;
using Shop3.Infrastructure.SharedKernel;

namespace Shop3.Data.Entities
{
    public class PageDefault : DomainEntity<string>
    {
        public PageDefault()
        {

        }

        public PageDefault(string id, string title,string content,Status status)
        {
            Id = id;
            Title = title;
            Content = content;
            Status = status;
        }
        public PageDefault(string title, string content, Status status)
        {
            Title = title;
            Content = content;
            Status = status;
        }

        [Required]
        [MaxLength(256)]
        public string Title { set; get; }

        [Required]
        public string Content { set; get; }

        public Status Status { set; get; }
    }
}
