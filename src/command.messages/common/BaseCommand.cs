using infrastructure.cqs;
using System;

namespace command.messages.common
{
    public class BaseCommand : ICommand
    {
        protected Guid _guid;

        public Guid CommandId { get { return _guid; }  }        
    }
}
