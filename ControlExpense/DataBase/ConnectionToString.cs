using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ControlExpense.DataBase
{
    public class ConnectionToString
    {
        public DbContextOptions<ExpenseContext> GetContextOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExpenseContext>();

            var options = optionsBuilder
                    .UseSqlServer(@"Server=.;Database=expense_db;Trusted_Connection=True;")
                    .Options;

            return options;
        }
    }
}
