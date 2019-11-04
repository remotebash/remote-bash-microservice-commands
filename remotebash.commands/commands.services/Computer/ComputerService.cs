using commands.models.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace commands.services.Computer
{
    public class ComputerService
    {
        public bool IsComputerOnline(long idComputer)
        {
            var request = WebRequest.CreateHttp(string.Format("https://remotebash.herokuapp.com/status/{0}", idComputer));
            request.Method = "GET";
            using (var response = request.GetResponse())
            {
                using (var streamDados = response.GetResponseStream())
                {
                    StreamReader rd = new StreamReader(streamDados);
                    var computerOnline = JsonConvert.DeserializeObject<ComputerOnline>(rd.ReadToEnd().ToString());
                    return "online".Equals(computerOnline.Status.ToLower());
                }   
            }            
        }
    }
}
