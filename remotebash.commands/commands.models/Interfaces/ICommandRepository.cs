using commands.api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace commands.models.Interfaces
{
    public interface ICommandRepository
    {
        List<Command> GetCommandsToExecute(string idComputer);

        Command GetCommand(string idCommand);

        Command GetCommandExecuted(string idCommand);

        void SaveCommand(Command command);

        bool UpdateCommand(Command command);
    }
}
