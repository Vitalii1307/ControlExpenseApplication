using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlExpense.Entities
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        [Required]
        public int Amount { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public User User { get; set; }
    }
}
