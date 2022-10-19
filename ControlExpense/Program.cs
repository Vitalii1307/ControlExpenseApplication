using ControlExpense;
using ControlExpense.DataBase;

class Progaram
{
    static void Main()
    {
        var connectionOption = new ConnectionToString();

        using (ExpenseContext db = new ExpenseContext(connectionOption.GetContextOptions()))
        {
            var display = new DisplayMenu(db);
            var currentUser = display.LogInSingUpDisplay();
            display.StartMenu(currentUser);
        }
    }
}