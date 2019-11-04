using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using commands.api.Models;
using commands.models;
using commands.models.Interfaces;
using commands.services.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace commands.api.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Route("/command/")]
    [ApiController]
    public class CommandsController : Controller
    {
        private readonly CommandService commandService = null;

        public CommandsController(IOptions<Settings> settings)

        {
            this.commandService = new CommandService(settings);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// get all commands to the computer run
        /// </summary>
        /// <returns>list of commands to the computer run</returns>
        [HttpGet("computer/{idComputer}")]
        public JsonResult GetCommandToExecute(long idComputer)
        {
            try
            {
                List<Command> commands = commandService.GetCommandsToExecute(idComputer);
                JsonResult json = Json(commands);

                if (commands == null)
                    json.StatusCode = (int)HttpStatusCode.NoContent;
                else
                    json.StatusCode = (int)HttpStatusCode.OK;

                return json;
            }
            catch (Exception ex)
            {
                JsonResult json = Json(ex);
                json.StatusCode = (int)HttpStatusCode.InternalServerError;
                return json;
            }
        }

        /// <summary>
        /// saves a command to the base and execute on pc. This command is sent from our main api rest
        /// </summary>
        /// <returns>json object of Command (Executed)</returns>
        [HttpPost]
        public JsonResult SaveCommand([FromBody] Command command)
        {
            try
            {
                Command  commandSaved = commandService.SaveCommand(command);
                JsonResult json = new JsonResult(commandSaved)
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
                return json;
            }
            catch (Exception ex)
            {
                JsonResult json = Json(ex);
                json.StatusCode = (int)HttpStatusCode.InternalServerError;
                return json;
            }

        }

        /// <summary>
        /// updates a command.The computer sends an update for this microservice
        /// </summary>
        /// <returns>bool</returns>
        [HttpPut]
        public JsonResult PutCommand([FromBody] Command command)
        {
            try
            {
                commandService.UpdateCommand(command);
                JsonResult json = new JsonResult("success")
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
                return json;
            }
            catch (Exception ex)
            {
                JsonResult json = Json(ex);
                json.StatusCode = (int)HttpStatusCode.InternalServerError;
                return json;
            }
        }

    }
}