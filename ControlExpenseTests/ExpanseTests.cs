using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlExpense.DataBase;
using Microsoft.EntityFrameworkCore;
using ControlExpense;
using ControlExpense.Entities;
using System.Linq;

namespace ControlExpenseTests
{
    [TestClass]
    public class ExpanseTests
    {
        protected readonly ExpenseContext _context;

        public ExpanseTests() 
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExpenseContext>();

            var options = optionsBuilder
                    .UseSqlServer(@"Server=.;Database=expense_db;Trusted_Connection=True;")
                    .Options;
            _context = new ExpenseContext(options);
        }

        [TestMethod]
        public void CaterogyToString_WithValidValue_ReturnCategoryString()
        {
            int numberCategory = 1;
            string expectedString = "Food";

            DisplayMenu displayMenu = new DisplayMenu(_context);
            string actual = displayMenu.CaterogyToString(numberCategory);

            Assert.AreEqual(expectedString, actual);
        }

        [TestMethod]
        public void CaterogyToString_WithNotValidValue_ThrowArgumentOutOfRange()
        {
            int numberCategory = 0;
            DisplayMenu displayMenu = new DisplayMenu(_context);

            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => displayMenu.CaterogyToString(numberCategory));
        }

        [TestMethod]
        public void InsertExpence_WithNotValidValue_DbUpdateException()
        {
            WorkWithDb workWithDb = new WorkWithDb(_context);
            Expense expense = new Expense() {Amount = 100, Category="Food", Description="For dinner", Date = DateTime.Now }; //user isn't specified in the field

            Assert.ThrowsException<DbUpdateException>(() => workWithDb.InsertExpence(expense));
        }

        [TestMethod]
        public void InsertUser_WithNotValidValue_DbUpdateException()
        {
            WorkWithDb workWithDb = new WorkWithDb(_context);
            User user = new User { Login = "admin" }; //Password isn't specified in the field

            Assert.ThrowsException<DbUpdateException>(() => workWithDb.InsertUser(user));

        }

        [TestMethod]
        public void IsExistUser_UserIsExist_ReturnTrue()
        {

            WorkWithDb workWithDb = new WorkWithDb(_context);
            User checkingUser = new User() {Login = "login", Password = "password" };
            User existingUser = new User() {Login = "login", Password = "password" };

            _context.Users.Add(existingUser);
            _context.SaveChanges();

            bool isExist = workWithDb.IsExistUser(checkingUser);
            _context.Users.Remove(existingUser);
            _context.SaveChanges();

            Assert.IsTrue(isExist);
        }
        [TestMethod]
        public void IsExistUser_UserIsnotExist_ReturnFalse()
        {

            WorkWithDb workWithDb = new WorkWithDb(_context);
            User checkingUser = new User() { Login = "login", Password = "password1" };//password differs
            User existingUser = new User() { Login = "login", Password = "password" };

            _context.Users.Add(existingUser);
            _context.SaveChanges();

            bool isExist = workWithDb.IsExistUser(checkingUser);
            _context.Users.Remove(existingUser);
            _context.SaveChanges();

            Assert.IsFalse(isExist);
        }
        [TestMethod]
        public void GetExistUser_UserReturnSucces_ReturnUser()
        {
            
            WorkWithDb workWithDb = new WorkWithDb(_context);
            User checkingUser = new User() {Login = "login", Password = "password" };
            User existingUser = new User() {Login = "login", Password = "password" };

            _context.Users.Add(existingUser);
            _context.SaveChanges();

            User returnedUser = workWithDb.GetExistUser(checkingUser);

            _context.Users.Remove(returnedUser);
            _context.SaveChanges();
            
            Assert.AreEqual($"{checkingUser.Login}+{checkingUser.Password}", $"{returnedUser.Login}+{returnedUser.Password}");
        }

        [TestMethod]
        public void GetExistUser_UserIsNotExist_ReturnException()
        {
            WorkWithDb workWithDb = new WorkWithDb(_context);
            User checkingUser = new User() { Login = "login", Password = "password1" };//password differs
            
            try
            {
                workWithDb.GetExistUser(checkingUser);
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, "User isn't exist");
                return;
            }
            Assert.Fail("The expected exception was not thrown.");
        }
    }
}