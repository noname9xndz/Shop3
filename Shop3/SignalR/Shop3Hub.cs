using Microsoft.AspNetCore.SignalR;
using Shop3.Application.ViewModels.System;
using System.Threading.Tasks;

namespace Shop3.SignalR
{
    public class Shop3Hub : Hub
    {
        // https://viblo.asia/p/bat-dau-voi-aspnet-core-va-signalr-Do754k73lM6
        // https://github.com/aspnet/SignalR-samples
        // https://docs.microsoft.com/vi-vn/aspnet/core/signalr/introduction?view=aspnetcore-2.2
        // https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/introduction-to-signalr
        //  nuget : Microsoft.AspNetCore.SignalR

        public async Task SendMessage(AnnouncementViewModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message); // tên method , tham số trả về
        }
    }
}