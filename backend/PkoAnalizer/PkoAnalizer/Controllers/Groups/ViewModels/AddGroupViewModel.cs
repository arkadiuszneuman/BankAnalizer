using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Groups.ViewModels
{
    public class AddGroupViewModel
    {
        public Guid BankTransactionId { get; set; }
        public string Name { get; set; }
    }
}
