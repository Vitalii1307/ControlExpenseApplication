using ControlExpense.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlExpense
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
