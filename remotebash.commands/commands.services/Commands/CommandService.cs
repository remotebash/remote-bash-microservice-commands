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
                    return GetCommandExecuted(commandSaved);
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

        public string SaveCommandInLaboratory(List<Command> commands)
        {
            string executeIn = string.Empty;
            foreach (Command command in commands)
            {
                try
                {
                    Command commandSaved = commandRepository.SaveCommand(command);
                    if (commandSaved != null)
                    {
                        ComputerService computerService = new ComputerService();
                        if (computerService.IsComputerOnline(command.IdComputer))
                            executeIn += $"Comando enviado para o computador {command.IdComputer}";
                        else
                            executeIn += $"O Computador {command.IdComputer} não está online, mas o comando será executado assim que o mesmo estiver.";
                    }
                }
                catch (Exception ex) { throw ex; }
            }
            return executeIn;
        }

        private Command GetCommandExecuted(Command command)
        {
            bool find = false;
            Command commandExecuted = null;

            while (!find)
            {
                ComputerService computerService = new ComputerService();
                Command commandSearchExecuted = SearchCommandExecuted(command.IdCommand);
                if (computerService.IsComputerOnline(command.IdComputer))
                {
                    if (commandSearchExecuted != null)
                    {
                        commandExecuted = commandSearchExecuted;
                        find = true;
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    var commandToExecute = GetCommand(command.IdCommand);
                    commandToExecute.ComplementCommandFinish("O computador não está online.");
                    commandExecuted = commandToExecute;
                    break;
                }
            }
            return commandExecuted;
        }

        public List<Command> GetCommandsByComputer(long idComputer)
        {
            List<Command> commands = commandRepository.GetCommandByComputer(idComputer);
            foreach (Command command in commands)
            {
                if (command.IsExecuted && string.IsNullOrWhiteSpace(command.Result))
                    command.Result = "Comando executado.";
                else if (!command.IsExecuted && string.IsNullOrWhiteSpace(command.Result))
                    command.Result = "Comando ainda não executado.";
            }
            return commands;
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
