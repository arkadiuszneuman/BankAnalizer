using System.Threading.Tasks;

namespace BankAnalizer.Core.Cqrs.Event
{
    public interface IEventsBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
