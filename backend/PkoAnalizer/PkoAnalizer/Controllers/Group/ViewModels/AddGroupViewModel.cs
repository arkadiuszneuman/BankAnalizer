using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Group.ViewModels
{
    public class BankTransactionGroupViewModel
    {
        public Guid BankTransactionId { get; set; }
        public string GroupName { get; set; }
    }
}
