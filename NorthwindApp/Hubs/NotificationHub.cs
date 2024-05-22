using BL.Events;
using BL.Events.IEvent;
using BL.IServices;
using BL.Models.VM;
using BL.Services;
using Core.Entities;
using Core.Interfaces.IObservice;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace NorthwindApp.Hubs
{
    public class NotificationHub : Hub<Core.Interfaces.IObservice.IObserver<ProductVM>>
    {
        private readonly IProductService _productService;
        public NotificationHub(IProductService productService)
        {
            _productService = productService;
        }

        #region SendMessagges
        public async Task SendDefaultMessageAsync(string message = "Something was updated or added!")
        {
            await Clients.Others.SendDefaultMessageAsync(message);
        }
        public async Task SendAsync(string name)
        {
            Console.WriteLine(name);
            await Clients.All.SendAsync("SendAsync");

        }
        public async Task SendMessageAsync(string message)
        {
            await Clients.All.SendMessageAsync(message);
        }
        public async Task SendWithMessageAsync(ProductVM data , string message = "Something was updated or added!")
        {
             await Clients.All.SendWithMessageAsync(data , message);
        }
        #endregion

        #region Notify
        public async Task StartNotify()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "ProductSubscribers");
        }

        public async Task EndNotify()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ProductSubscribers");
        }
        #endregion
    }
    // js
    // connection.invoke("startNotify") EventListener 'click' =>
    //connection.invoke("endNotify") EventListener 'click' =>
}
