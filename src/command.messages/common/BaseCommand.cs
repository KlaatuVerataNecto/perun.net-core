using infrastructure.cqs;
using System;

namespace command.messages.common
{
    public class BaseCommand : ICommand
    {
        /// <summary>
        /// The Aggregate ID of the Aggregate Root being changed
        /// </summary>
        public Guid CommandId { get; set; }
    }
}
