using commands.api.Models;
using commands.dal.Commands;
using commands.models;
using commands.models.Interfaces;
using commands.services.Computer;
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

        public Command SaveCommand(Command command)
        {
            if (command == null)
                return null;
            try
            {
                Command commandSaved = commandRepository.SaveCommand(command);
                if (commandSaved != null)
                    return GetCommandExecuted(commandSaved.IdCommand);
                else
                {
                    command.ComplementCommandFinish("Ocorreu um erro ao tentar executar esse comando. Se o erro persistir entre em contato com a Remotebash.");
                    return command;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private Command GetCommandExecuted(string idCommand)
        {
            bool find = false;
            Command commandExecuted = null;

            while (!find)
            {
                ComputerService computerService = new ComputerService();
                Command commandSearch = SearchCommandExecuted(idCommand);
                if (computerService.IsComputerOnline(commandSearch.IdComputer))
                {
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
                else
                {
                    var commandToExecute = GetCommand(idCommand);
                    commandToExecute.ComplementCommandFinish("O computador não está online.");
                    commandExecuted = commandToExecute;                    
                    break;
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
