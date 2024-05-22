using BL.Models.VM;
using Core.Entities;
using Rpd.EventQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Events.EventHandlers
{

  /*  public class ProductAddEventHandler : IEventHandler<ProductAddEvent>
    {
        private readonly IHubContext<NotificationHub, IObserver<ProductVM>> _hubContext;

        public ProductAddedEventHandler(IHubContext<NotificationHub, IObserver<ProductVM>> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task OnNextAsync(ProductVM product)
        {
            // Ваша логика обработки события

            // Отправка уведомления подписчикам через SignalR
            await _hubContext.Clients.Group("ProductSubscribers").SendWithMessageAsync(product);
        }
    }*/
}
