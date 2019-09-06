using Shop3.Data.Enums;

namespace Shop3.Data.Interfaces
{
    public interface ISwitchable
    {// thuộc  tính ,đối tượng đã được kích hoạt hay ko???
        Status Status { set; get; }
    }
}
