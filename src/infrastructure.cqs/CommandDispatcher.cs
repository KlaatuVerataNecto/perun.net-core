using Autofac;
using System;

namespace infrastructure.cqs
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _container;

        public CommandDispatcher(IComponentContext container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("_container");
            }
            _container = container;
        }

        public void Send<TParameter>(TParameter command) where TParameter : ICommand
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            dynamic handler = _container.Resolve(handlerType);

            handler.Handle((dynamic)command);
        }
    }
}