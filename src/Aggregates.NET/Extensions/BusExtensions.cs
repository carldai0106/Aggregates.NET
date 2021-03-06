﻿using Aggregates.Exceptions;
using Aggregates.Internal;
using Aggregates.Messages;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Unicast;
using NServiceBus.Unicast.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregates.Extensions
{
    public static class BusExtensions
    {
        private static ILog Logger = LogManager.GetLogger("Bus");

        public static void CommandResponse(this IMessage msg)
        {
            if (msg is Reject)
            {
                var reject = msg as Reject;
                Logger.WriteFormat(LogLevel.Warn, "Command was rejected - Message: {0}\n", reject.Message);
                if (reject != null)
                    throw new CommandRejectedException(reject.Message);
                throw new CommandRejectedException();
            }
            if (msg is Error)
            {
                var error = msg as Error;
                Logger.Warn($"Command Fault!\n{error.Message}");
                throw new CommandRejectedException($"Command Fault!\n{error.Message}");
            }
        }
        


        public static async Task Command(this IMessageSession ctx, ICommand command)
        {
            var options = new NServiceBus.SendOptions();
            options.SetHeader(Defaults.REQUEST_RESPONSE, "1");

            var response = await ctx.Request<IMessage>(command, options).ConfigureAwait(false);
            response.CommandResponse();
        }
        public static async Task Command(this IMessageSession ctx, string destination, ICommand command)
        {
            var options = new NServiceBus.SendOptions();
            options.SetDestination(destination);
            options.SetHeader(Defaults.REQUEST_RESPONSE, "1");

            var response = await ctx.Request<IMessage>(command, options).ConfigureAwait(false);
            response.CommandResponse();
        }
        public static async Task Command<TCommand>(this IMessageSession ctx, Action<TCommand> command) where TCommand : ICommand
        {
            var options = new NServiceBus.SendOptions();
            options.SetHeader(Defaults.REQUEST_RESPONSE, "1");

            var response = await ctx.Request<IMessage>(command, options).ConfigureAwait(false);
            response.CommandResponse();
        }
        public static async Task Command<TCommand>(this IMessageSession ctx, string destination, Action<TCommand> command) where TCommand : ICommand
        {
            var options = new NServiceBus.SendOptions();
            options.SetDestination(destination);
            options.SetHeader(Defaults.REQUEST_RESPONSE, "1");

            var response = await ctx.Request<IMessage>(command, options).ConfigureAwait(false);
            response.CommandResponse();
        }

        /// <summary>
        /// Send the command, don't care if its rejected
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static async Task PassiveCommand<TCommand>(this IMessageSession ctx, Action<TCommand> command) where TCommand : ICommand
        {
            var options = new NServiceBus.SendOptions();
            options.SetHeader(Defaults.REQUEST_RESPONSE, "0");

            await ctx.Send(command, options).ConfigureAwait(false);
        }
        public static async Task PassiveCommand(this IMessageSession ctx, ICommand command)
        {
            var options = new NServiceBus.SendOptions();
            options.SetHeader(Defaults.REQUEST_RESPONSE, "0");

            await ctx.Send(command, options).ConfigureAwait(false);
        }
        public static async Task PassiveCommand<TCommand>(this IMessageSession ctx, string destination, Action<TCommand> command) where TCommand : ICommand
        {
            var options = new NServiceBus.SendOptions();
            options.SetDestination(destination);
            options.SetHeader(Defaults.REQUEST_RESPONSE, "0");

            await ctx.Send(command, options).ConfigureAwait(false);
        }
        public static async Task PassiveCommand(this IMessageSession ctx, string destination, ICommand command)
        {
            var options = new NServiceBus.SendOptions();
            options.SetDestination(destination);
            options.SetHeader(Defaults.REQUEST_RESPONSE, "0");

            await ctx.Send(command, options).ConfigureAwait(false);
        }
        public static async Task PassiveCommand<TCommand>(this IMessageHandlerContext ctx, Action<TCommand> command) where TCommand : ICommand
        {
            var options = new NServiceBus.SendOptions();
            options.SetHeader(Defaults.REQUEST_RESPONSE, "0");

            await ctx.Send(command, options).ConfigureAwait(false);
        }
        public static async Task PassiveCommand(this IMessageHandlerContext ctx, ICommand command)
        {
            var options = new NServiceBus.SendOptions();
            options.SetHeader(Defaults.REQUEST_RESPONSE, "0");

            await ctx.Send(command, options).ConfigureAwait(false);
        }
        public static async Task PassiveCommand<TCommand>(this IMessageHandlerContext ctx, string destination, Action<TCommand> command) where TCommand : ICommand
        {
            var options = new NServiceBus.SendOptions();
            options.SetDestination(destination);
            options.SetHeader(Defaults.REQUEST_RESPONSE, "0");

            await ctx.Send(command, options).ConfigureAwait(false);
        }
        public static async Task PassiveCommand(this IMessageHandlerContext ctx, string destination, ICommand command)
        {
            var options = new NServiceBus.SendOptions();
            options.SetDestination(destination);
            options.SetHeader(Defaults.REQUEST_RESPONSE, "0");

            await ctx.Send(command, options).ConfigureAwait(false);
        }
    }
}
