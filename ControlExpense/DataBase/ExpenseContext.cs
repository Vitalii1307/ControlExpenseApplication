using ControlExpense.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlExpense.DataBase
{
    public class ExpenseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


    }
}
