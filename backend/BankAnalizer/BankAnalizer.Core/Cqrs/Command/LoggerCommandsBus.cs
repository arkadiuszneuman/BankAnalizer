using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BankAnalizer.Core.Cqrs.Command
{
    public class LoggerCommandsBus : ICommandsBus
    {
        private readonly ICommandsBus commandsBus;
        private readonly ILogger<LoggerCommandsBus> logger;

        public LoggerCommandsBus(ICommandsBus commandsBus,
            ILogger<LoggerCommandsBus> logger)
        {
            this.commandsBus = commandsBus;
            this.logger = logger;
        }

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            logger.LogDebug("Sending command {command}", command.GetType().Name);
            var sw = Stopwatch.StartNew();
            await commandsBus.SendAsync(command);
            sw.Stop();
            logger.LogDebug("Command {command} executed in {elapsed}", command.GetType().Name, sw.Elapsed);
        }

        public async Task SendAsync(Type commandType, ICommand command)
        {
            logger.LogDebug("Sending command {command}", commandType.Name);
            var sw = Stopwatch.StartNew();
            await commandsBus.SendAsync(commandType, command);
            sw.Stop();
            logger.LogDebug("Command {command} executed in {elapsed}", commandType.Name, sw.Elapsed);
        }
    }
}
