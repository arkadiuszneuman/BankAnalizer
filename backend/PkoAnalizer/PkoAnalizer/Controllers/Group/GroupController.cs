﻿using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Group;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Web.Controllers.Group.ViewModels;
using System;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Group
{
    [Route("api/transaction/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ICommandsBus bus;

        public GroupController(ICommandsBus bus)
        {
            this.bus = bus;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> AddGroup([FromHeader]Guid userId, BankTransactionGroupViewModel addGroupViewModel)
        {
            var command = new AddGroupCommand(addGroupViewModel.BankTransactionId, addGroupViewModel.GroupName, userId, default);
            _ = bus.Send(command);
            return Accepted(command);
        }

        [HttpDelete]
        [Route("")]
        public async Task<ActionResult> RemoveGroup(BankTransactionGroupViewModel addGroupViewModel)
        {
            var command = new RemoveGroupCommand(addGroupViewModel.BankTransactionId, addGroupViewModel.GroupName);
            _ = bus.Send(command);
            return Accepted(command);
        }
    }
}
