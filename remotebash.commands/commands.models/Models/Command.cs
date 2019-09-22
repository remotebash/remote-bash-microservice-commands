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
        public string IdComputer { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("command")]
        public string CommandToExecute { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [BsonDateTimeOptions]
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [BsonDateTimeOptions]
        [JsonProperty("end")]
        public DateTime End { get; set; }

        [JsonProperty("whoExcuted")]
        public string WhoExcuted { get; set; }

        [JsonProperty("isExecuted")]
        public bool IsExecuted { get; set; }
    }
}