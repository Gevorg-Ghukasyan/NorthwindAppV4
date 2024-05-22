using Core.Abstraction;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.IObservice
{
    public interface IObserver<T>
    {
        Task SendDefaultMessageAsync(string mesasge = "Something was updated or added!");
        Task SendAsync(string name, string message = "Something was updated or added!");
        Task SendMessageAsync(string message);
        Task SendWithMessageAsync(T data , string  message = "Something was updated or added!");
        Task SendAsyncProduct(T data, string message);
    }
}
