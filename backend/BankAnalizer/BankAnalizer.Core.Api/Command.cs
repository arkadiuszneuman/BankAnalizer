﻿using BankAnalizer.Core.Cqrs.Command;
using System;

namespace BankAnalizer.Core.Api
{
    public class Command : ICommand
    {
        public Guid CommandId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
    }
}
