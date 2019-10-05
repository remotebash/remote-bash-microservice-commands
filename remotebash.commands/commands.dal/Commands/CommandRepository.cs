using commands.api.Models;
using commands.models;
using commands.models.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace commands.dal.Commands
{
    public class CommandRepository : ICommandRepository
    {

        private readonly CommandContext _context = null;

        public CommandRepository(IOptions<Settings> settings)
        {
            _context = new CommandContext(settings);
        }

        public Command GetCommand(string idCommand)
        {
            try
            {
                var commmad = _context.Command.Find(cmd => cmd.IdCommand == idCommand).FirstOrDefault();
                return commmad ?? new Command();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Command GetCommandExecuted(string idCommand)
        {
            try
            {
                var commmad = _context.Command.Find(cmd => cmd.IdCommand == idCommand && cmd.IsExecuted).FirstOrDefault();
                return commmad ?? new Command();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Command> GetCommandsToExecute(string idComputer)
        {
            try
            {
                var commands = _context.Command.Find(cmd => cmd.IdComputer == idComputer);
                if (commands.CountDocuments() > 0)
                    return commands.ToList();
                else
                    return new List<Command>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveCommand(Command command)
        {
            try
            {
                _context.Command.InsertOne(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateCommand(Command command)
        {
            var filter = Builders<Command>.Filter.Eq(cmd => cmd.IdCommand, command.IdCommand);
            var update = Builders<Command>.Update
                            .Set(cmd => cmd.Result, command.Result)
                            .Set(cmd => cmd.End, command.End)
                            .Set(cmd => cmd.IsExecuted, true);

            try
            {
                UpdateResult actionResult = _context.Command.UpdateOne(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
