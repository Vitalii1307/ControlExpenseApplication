using ControlExpense.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlExpense.DataBase
{
    public class WorkWithDb
    {
        public ExpenseContext db { get; set; }

        public WorkWithDb(/*User user,*/ ExpenseContext db)
        {
            this.db = db;
        }

        public void InsertExpence(Expense expense)
        {
            db.Expenses.Add(expense);
            db.SaveChanges();
        }

        public void InsertUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public void RemoveAllExpenses(User user)
        {
            var expenses = db.Expenses.Where(p => p.User.UserId == user.UserId);
            db.Expenses.RemoveRange(expenses);
            db.SaveChanges();
        }

        public bool IsExistUser(User user)
        {
            var users = db.Users.ToList();

            foreach (var u_ in users)
            {
                if (user.Password == u_.Password && user.Login == u_.Login)
                    return true;
            }
            return false;
        }

        public User GetExistUser(User user)
        {
            var users = db.Users.ToList();

            foreach (var u_ in users)
            {
                if (user.Password == u_.Password && user.Login == u_.Login)
                    return u_;
            }
            throw new Exception("User isn't exist");
        }

        public IQueryable<Expense> GetAllCategory(User user)
        {
            IQueryable<Expense> categories = db.Expenses.Where(p => p.User.UserId == user.UserId).OrderBy(p => p.Date);
            return categories;
        }

        public IQueryable<Expense> GetSpecialCategory(User user, string categoryString)
        {
            IQueryable<Expense> categories = db.Expenses.Where(p => p.User.UserId == user.UserId && p.Category == categoryString);
            return categories;
        }

        public void ShowCategory(IQueryable<Expense> categories)
        {
            if (categories.Count() == 0)
            {
                Console.WriteLine("List is empty");
            }
            else
            {
                foreach (var cat in categories)
                {
                    Console.WriteLine(cat.Date.ToString("d") + " - " + cat.Amount + " UAH: " + cat.Category + ": " + cat.Description);
                }
            }
        }

        public IQueryable<Expense> GetCategoryByDay(User user, DateTime dateTime)
        {
            IQueryable<Expense> categories = db.Expenses.Where(p => p.User.UserId == user.UserId && p.Date.Day == dateTime.Day);
            return categories;
        }

        public IQueryable<Expense> GetCategoryByMonth(User user, int month)
        {
            IQueryable<Expense> categories = db.Expenses.Where(p => p.User.UserId == user.UserId && p.Date.Month == month);
            return categories;
        }
        public IQueryable<Expense> GetCategoryByYear(User user, int year)
        {
            IQueryable<Expense> categories = db.Expenses.Where(p => p.User.UserId == user.UserId && p.Date.Year == year);
            return categories;
        }
    }
}
