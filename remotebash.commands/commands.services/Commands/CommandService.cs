using commands.api.Models;
using commands.dal.Commands;
using commands.models;
using commands.models.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace commands.services.Commands
{
    public class CommandService : ICommandRepository
    {
        private readonly CommandRepository commandRepository;
        public CommandService(IOptions<Settings> settings)
        {
            commandRepository = new CommandRepository(settings);
        }

        public void SaveCommand(Command command)
        {
            if (command != null)
                commandRepository.SaveCommand(command);
        }

        public Command GetCommandExecuted(string idCommand)
        {
            bool find = false;
            Command commandExecuted = null;

            if (GetCommand(idCommand) == null)
                return null;

            while (!find)
            {
                //TOOD VERIFICAR SE O PC ESTÁ ONLINE
                Command commandSearch = SearchCommandExecuted(idCommand);
                if (commandSearch != null)
                {
                    commandExecuted = commandSearch;
                    find = true;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
            return commandExecuted;
        }

        private Command SearchCommandExecuted(string idCommand)
        {
            return commandRepository.GetCommandExecuted(idCommand);
        }

        public List<Command> GetCommandsToExecute(long idComputer)
        {
            return commandRepository.GetCommandsToExecute(idComputer);
        }

        public bool UpdateCommand(Command command)
        {
            return commandRepository.UpdateCommand(command);
        }

        public Command GetCommand(string idCommand)
        {
            return commandRepository.GetCommand(idCommand);
        }
    }
}
