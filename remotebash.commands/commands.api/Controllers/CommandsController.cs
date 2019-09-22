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
        /// get command executed
        /// </summary>
        /// <param name="idCommand"></param>
        /// <returns></returns>
        [HttpGet("{idCommand}")]
        public JsonResult GetCommandExecuted(string idCommand)
        {
            try
            {
                Command command = commandService.GetCommandExecuted(idCommand);
                JsonResult json = Json(command);
                if (command == null)
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
        /// get all commands to the computer run
        /// </summary>
        /// <returns>list of commands to the computer run</returns>
        [HttpGet("computer/{idComputer}")]
        public JsonResult GetCommandToExecute(string idComputer)
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveCommand([FromBody] Command command)
        {
            try
            {
                commandService.SaveCommand(command);
                JsonResult json = new JsonResult("sucess")
                {
                    StatusCode = (int)HttpStatusCode.Created
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public JsonResult PutCommand([FromBody] Command command)
        {
            try
            {
                commandService.UpdateCommand(command);
                JsonResult json = new JsonResult("sucess")
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