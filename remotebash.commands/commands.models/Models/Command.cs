using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commands.api.Models
{
    public class Command
    {
        [BsonId]
        [JsonProperty("idCommand")]
        public string IdCommand { get; set; }

        [JsonProperty("idComputer")]
        public long IdComputer { get; set; }

        [JsonProperty("operationalSystem")]
        public string OperationalSystem { get; set; }

        [JsonProperty("command")]
        public string CommandToExecute { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [BsonDateTimeOptions]
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [BsonDateTimeOptions]
        [JsonProperty("end")]
        public DateTime? End { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("isExecuted")]
        public bool IsExecuted { get; set; }

        public void ComplementCommandFinish(string message)
        {
            Result = message;
            End = DateTime.Now;
            IsExecuted = true;
        }
    }
}