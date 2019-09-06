using Shop3.Data.Enums;
using Shop3.Data.Interfaces;
using Shop3.Infrastructure.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop3.Data.Entities
{
    [Table("SupportOnlines")]
    public class SupportOnline : DomainEntity<int>, IDateTracking
    {
        public SupportOnline()
        {

        }

        public SupportOnline(string name, string skype, string faceBook, string yahoo, string pinterest, string twitter, string google
            , string mobile, string email, string instagram, string youtube, string linkedin, string zalo, string timeOpenWindow,
            string other, Status status, DateTime dateCreated, DateTime dateModified, int displayOrder)
        {
            Name = name;
            Skype = skype;
            FaceBook = faceBook;
            Yahoo = yahoo;
            Pinterest = pinterest;
            Twitter = twitter;
            Google = google;
            Mobile = mobile;
            Email = email;
            Instagram = instagram;
            Youtube = youtube;
            Linkedin = linkedin;
            Zalo = zalo;
            TimeOpenWindow = timeOpenWindow;
            Other = other;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DisplayOrder = displayOrder;
        }
        public SupportOnline(int id, string name, string skype, string faceBook, string yahoo, string pinterest, string twitter, string google
            , string mobile, string email, string instagram, string youtube, string linkedin, string zalo,
            string timeOpenWindow, string other, Status status, DateTime dateCreated, DateTime dateModified, int displayOrder)
        {
            Id = id;
            Name = name;
            Skype = skype;
            FaceBook = faceBook;
            Yahoo = yahoo;
            Pinterest = pinterest;
            Twitter = twitter;
            Google = google;
            Mobile = mobile;
            Email = email;
            Instagram = instagram;
            Youtube = youtube;
            Linkedin = linkedin;
            Zalo = zalo;
            TimeOpenWindow = timeOpenWindow;
            Other = other;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
            DisplayOrder = displayOrder;
        }

        [StringLength(128)]
        public string Name { set; get; }
        [StringLength(128)]
        public string Skype { get; set; }
        [StringLength(128)]
        public string FaceBook { get; set; }
        [StringLength(128)]
        public string Yahoo { get; set; }
        [StringLength(128)]
        public string Pinterest { get; set; }
        public string Twitter { get; set; }
        [StringLength(128)]
        public string Google { get; set; }
        [StringLength(128)]
        public string Mobile { get; set; }
        [StringLength(128)]
        public string Email { get; set; }
        [StringLength(128)]
        public string Instagram { get; set; }
        [StringLength(128)]
        public string Youtube { get; set; }
        [StringLength(128)]
        public string Linkedin { get; set; }
        [StringLength(128)]
        public string Zalo { get; set; }
        [StringLength(128)]
        public string TimeOpenWindow { get; set; }
        [StringLength(500)]
        public string Other { get; set; }
        public int DisplayOrder { get; set; }
        public Status Status { set; get; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
