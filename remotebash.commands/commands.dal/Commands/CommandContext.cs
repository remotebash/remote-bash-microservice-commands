using commands.api.Models;
using commands.models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace commands.dal.Commands
{
    public class CommandContext
    {
        private readonly IMongoDatabase _database = null;

        public CommandContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Command> Command
        {
            get
            {
                return _database.GetCollection<Command>("Command");
            }
        }
    }
}
