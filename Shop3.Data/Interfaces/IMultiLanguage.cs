using System;
using System.Collections.Generic;
using System.Text;

namespace Shop3.Data.Interfaces
{
    public interface IMultiLanguage<T>
    {
        T LanguageId { set; get; } // id của ngôn ngữ
    }
}
