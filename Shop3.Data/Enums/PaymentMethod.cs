using System.ComponentModel;

namespace Shop3.Data.Enums
{
    public enum PaymentMethod  // phương thức thanh toán hiển thị cho người dùng
    {
        [Description("Cash on delivery")] CashOnDelivery,//0
        [Description("Online Banking")] OnlinBanking,//1
        [Description("Payment Gateway")] PaymentGateway,//2
        [Description("Visa")] Visa,//3
        [Description("Master Card")] MasterCard,//4
        [Description("PayPal")] PayPal,//5
        [Description("Atm")] Atm//6


    }
}
