using Models;
using UI;
using DataAccess;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("../logs/logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


IRepository repo = new DBRepository();
MainMenu menu = new MainMenu(repo);
menu.Start();
