using ControlExpense.DataBase;
using ControlExpense.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlExpense
{
    public class DisplayMenu
    {
        public ExpenseContext dbContext { get; set; }
        public WorkWithDb dbWork { get; set; }

        public DisplayMenu(ExpenseContext db)
        {
            this.dbContext = db;
            dbWork = new WorkWithDb(db);
        }

        public User LogInSingUpDisplay()
        {
            int choiceVeriable = 0;
            User user = new User();

            Console.WriteLine("\tSelect a number");
            do
            {
                Console.WriteLine("1.Log In\t2.Sign Up");
                choiceVeriable = EnterNumber();
                Console.WriteLine();
            } while (choiceVeriable < 1 || choiceVeriable > 2);
            Console.Clear();

            if (choiceVeriable == 1)
            {
                user = LogIn();
            }
            else if (choiceVeriable == 2)
            {
                user = SignUp();
            }

            return user;
        }

        public User LogIn()
        {
            User user;
            bool isExist;
            do
            {
                LoginPassword logPass = DisplayLoginPass();

                user = new User { Login = logPass.Login, Password = logPass.Password };

                isExist = dbWork.IsExistUser(user);
                if (!isExist)
                    Console.WriteLine("This user isn't registered");
            } while (!isExist);   

            return dbWork.GetExistUser(user);
        }

        public User SignUp()
        {
            User user;
            bool isExist;
            do
            {
                LoginPassword logPass = DisplayLoginPass();

                user = new User { Login = logPass.Login, Password = logPass.Password};
                isExist = dbWork.IsExistUser(user);

                if (isExist)
                    Console.WriteLine("This password is already taken");
                else
                    dbWork.InsertUser(user);

            } while (isExist);

            return user;
        }

        public LoginPassword DisplayLoginPass()
        {
            string login = "";
            string password = "";
            do
            {
                Console.WriteLine("Fill all fields. Password must contain 8 or more symbols");
                Console.Write("Login: ");
                login = Console.ReadLine();
                Console.Write("Password: ");
                password = Console.ReadLine();
            } while (login == "" || password == "" || password.Length < 8);

            return new LoginPassword { Login = login, Password = password };
        }

        public void StartMenu(User user)
        {
            int choiceVariable;

            do
            {
                Console.WriteLine("\t\tMain menu:");
                Console.WriteLine("1.Enter the expense \t 2.Review existing expense\t0.Exit");
                choiceVariable = EnterNumber();
                Console.Clear();

                if (choiceVariable < 0 || choiceVariable > 2)
                {
                    Console.WriteLine("Chose the correct option");
                }

                if (choiceVariable == 1)
                {
                    Create_Expense(user);
                }
                else if (choiceVariable == 2)
                {
                    ReviewExpense(user);
                    Console.ReadLine();
                }

                Console.Clear();
            }
            while (choiceVariable != 0);

        }

        public void Create_Expense(User user)
        {
            int amount;
            int numCategory;
            string category;
            string description;
            DateTime dateTime = DateTime.Now;

            string choiceVariable;

            Console.WriteLine("\t\tExpense:");
            Console.Write("Amount: ");

            amount = EnterNumber();
            Console.WriteLine();
            do
            {
                Console.Write("Category: ");
                Console.Write("1.Food 2.Bills 3.Purcheses 4.Transport 5.Gifts 6.Education 7.Rent 8.Travels");

                Console.Write(": ");

                numCategory = EnterNumber();
                Console.WriteLine();
                if (numCategory < 1 || numCategory > 8)
                {
                    Console.WriteLine("Chose the correct option");
                }
            }
            while (numCategory < 1 || numCategory > 8);

            category = CaterogyToString(numCategory);

            Console.Write("Description: ");
            description = Console.ReadLine();

            do
            {
                Console.Write("Date (press 'enter' if today or command 'edit' if another date): ");
                choiceVariable = Console.ReadLine();

                if (choiceVariable == "")
                {
                    dateTime = DateTime.Now;
                }
                else if (choiceVariable == "edit")
                {
                    dateTime = EnterDate();
                }
            }
            while (choiceVariable != "" && choiceVariable != "edit");
            
            dbWork.InsertExpence(new Expense { Amount = amount, Category = category, Description = description, Date = dateTime, User = user });
        }

        public string CaterogyToString(int numberCategory)
        {
            switch (numberCategory)
            {
                case 1: return "Food";
                case 2: return "Bills";
                case 3: return "Purcheses";
                case 4: return "Transport";
                case 5: return "Gifts";
                case 6: return "Education";
                case 7: return "Rent";
                case 8: return "Travels";
                default: throw new ArgumentOutOfRangeException("Choice category excaption");
            }
        }

        public void ReviewExpense(User user)
        {
            int choiseVariable;

            Console.WriteLine("\tReview");
            do
            {
                Console.WriteLine("1.Statistics by category\t2.Statistics by time\t3.Clear all data");
                choiseVariable = EnterNumber();
                Console.Clear();
                if (choiseVariable == 1)
                {
                    StatisticByCategory(user);
                }
                else if (choiseVariable == 2)
                {
                    StatisticByTime(user);
                }
                else if (choiseVariable == 3)
                {
                    dbWork.RemoveAllExpenses(user);
                    Console.WriteLine("Data removed");
                }
                if (choiseVariable < 1 || choiseVariable > 3)
                {
                    Console.WriteLine("Chose the correct option");
                }
            } while (choiseVariable < 1 || choiseVariable > 3);

        }

        public void StatisticByCategory(User user)
        {
            int choiceVariable = 0;
            IQueryable<Expense> categoryList = null;

            do
            {
                Console.WriteLine("1.Food 2.Bills 3.Purcheses 4.Transport 5.Gifts 6.Education 7.Rent 8.Travels 9.All Category");
                choiceVariable = EnterNumber();
                Console.WriteLine();
                if (choiceVariable < 1 || choiceVariable > 9)
                    Console.WriteLine("Chose the correct option");

            } while (choiceVariable < 1 || choiceVariable > 9);


            if (choiceVariable < 9)
            {
                categoryList = dbWork.GetSpecialCategory(user, CaterogyToString(choiceVariable));
            }
            else if (choiceVariable == 9)
            {
                categoryList = dbWork.GetAllCategory(user);
            }
            dbWork.ShowCategory(categoryList);

        }

        public void StatisticByTime(User user)
        {
            IQueryable<Expense> categories = null;

            Console.WriteLine("Statistic for day");
            categories = dbWork.GetCategoryByDay(user, DateTime.Today);
            dbWork.ShowCategory(categories);

            Console.WriteLine();
            Console.WriteLine("Statistic for month");
            categories = dbWork.GetCategoryByMonth(user, DateTime.Now.Month);
            dbWork.ShowCategory(categories);

            Console.WriteLine();
            Console.WriteLine("Statistic for year");
            categories = dbWork.GetCategoryByYear(user, DateTime.Now.Year);
            dbWork.ShowCategory(categories);

        }

        public DateTime EnterDate()
        {
            int day;
            int month;
            int year;
            do
            {
                Console.WriteLine("Enter correct date");
                Console.Write("day: ");
                day = EnterNumber();
                Console.WriteLine();

                Console.Write("month: ");
                month = EnterNumber();
                Console.WriteLine();

                Console.Write("year: ");
                year = EnterNumber();
                Console.WriteLine();
            }
            while (day == 0 || day > 31 || month == 0 || month > 12 || year == 0 || year > 2022);


            return new DateTime(year, month, day);
        }

        public int EnterNumber()
        {
            double val = 0;
            string num = "";
            ConsoleKeyInfo chr;
            do
            {
                chr = Console.ReadKey(true);
                if (chr.Key != ConsoleKey.Backspace)
                {
                    bool control = double.TryParse(chr.KeyChar.ToString(), out val);
                    if (control)
                    {
                        num += chr.KeyChar;
                        Console.Write(chr.KeyChar);
                    }
                }
                else
                {
                    if (chr.Key == ConsoleKey.Backspace && num.Length > 0)
                    {
                        num = num.Substring(0, (num.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            while (chr.Key != ConsoleKey.Enter || num == "");


            return Convert.ToInt32(num);
        }
    }
}
