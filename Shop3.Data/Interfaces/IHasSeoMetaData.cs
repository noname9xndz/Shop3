namespace Shop3.Data.Interfaces
{
    public interface IHasSeoMetaData
    {// interface chứa các thuộc tính dành cho seo
        string SeoPageTitle { set; get; } // tiêu đề trang seo
        string SeoAlias { set; get; }
        string SeoKeywords { set; get; }
        string SeoDescription { get; set; }
    }
}
