using Models;
using DataAccess;
using Services;

namespace UI;

public class MainMenu
{
    private readonly ERSService _service;

    private readonly IRepository _repo;
    public MainMenu(ERSService service) {
        _service = service;
    }

    public static List<User> UserList = new List<User>();
    public static List<Ticket> TicketList = new List<Ticket>();
    public MainMenu(IRepository repo) {
        _repo = repo;
        
        UserList = _repo.initUserList();
        _repo.initTicketList(UserList);


    }
    

    public void Start()
    {
        
        Console.WriteLine("-----------------------------------------------------------------");
        Console.WriteLine("Welcome to the Expense Reimbursment System");
        Console.WriteLine("-----------------------------------------------------------------");
        //bool mainMenu = true;
        
        

        while (true) {
            Console.WriteLine("\nPLEASE ENTER THE NUMBER OF YOUR CHOICE\n");
            Console.WriteLine("0 -> Exit Application");
            Console.WriteLine("1 -> Login to My Account");
            Console.WriteLine("2 -> Register to New Account");
            Console.WriteLine("3 -> Show Users List");

            string input= Console.ReadLine()!;

            switch (input) {
                case "0":
                Console.WriteLine("Goodbye");
                Environment.Exit(0);
                break;
                case "1":
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Welcome to the Employee portal login");
                Console.WriteLine("-----------------------------------------------------------------\n");
                UserLogin();
                break;
                case "2":
                CreateNewUser();
                break;
                case "3":
                ShowUsersList();
                break;
                default:
                Console.WriteLine("Invalid entry");
                break;
            }
        }       
    }

    private void CreateNewUser() {

        Console.WriteLine("Creating New User");
        Console.Write("Please enter your name: ");
        string? UserName = Console.ReadLine()!;
        Console.Write("Please enter your password: ");
        //string? Password = Console.ReadLine()!;
        string? Password = PasswordEntryMasking();
        Console.WriteLine("\nAre you a manager? ([1]-yes, [0]-no)");
        int isManager = int.Parse(Console.ReadLine()!);

        if(isManager==0 || isManager==1) {
            // User user = new User(UserName,Password);
            User user = _repo.CreateNewUser(UserName, Password,isManager);
            UserList.Add(user);

            //return;
        }
        else
            Console.WriteLine("Invalid entry!!!!!!!!!");
    }

    public void UserLogin() {
        Console.Write("Please enter your Username: ");
        string? UserName = Console.ReadLine()!;
        Console.Write("Please enter your Password: ");
        string? Password = PasswordEntryMasking();

        _repo.UserLogin(UserName, Password);

        // bool userFound = false;
        // bool passwordMatch = false;
        // User currentUser = new();

        // while(!userFound) {
        //     Console.Write("Please enter your Username: ");
        //     string? UserName = Console.ReadLine()!;
        //     foreach (User obj in UserList) {
        //         if (obj.Username==UserName) {
        //             currentUser=obj;           
        //             userFound=true;
        //             break;
        //         }
        //         else 
        //             continue; 
        //     }
        //     if(!userFound) {
        //         Console.WriteLine("Invalid Username",Console.ForegroundColor=ConsoleColor.Red);
        //         Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
        //     }
        // }

        //  while(!passwordMatch) {
        //     Console.Write("Please enter your Password: ");
        //     string? Password = Console.ReadLine()!;
        //     if (currentUser.Password==Password) {
        //         passwordMatch=true;
        //         Console.WriteLine("You logged in successfully!",Console.ForegroundColor=ConsoleColor.Green);
        //         Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
        //         break;
        //     }
        //     else {
        //         Console.WriteLine("Invalid Password",Console.ForegroundColor=ConsoleColor.Red);
        //         Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
        //     }
        // }
       
        User currentUser = new();

        foreach(User u in UserList) {
            if(u.Username==UserName && u.Password==Password) {
                if(u.IsManager==0) {
                    currentUser=u;
                    UserMenu userMenu = new UserMenu(_repo);
                    userMenu.Start(currentUser);
                }
                else {
                    currentUser=u;
                    ManagerMenu managerMenu = new ManagerMenu(_repo);
                    managerMenu.Start(currentUser);
                }
            }
        }
        
    }

    public void ShowUsersList() {
        List<User> usersList = _repo.ShowUsersList();

        foreach (User u in usersList) {
            Console.WriteLine($"{u.ID} - {u.Username} - {u.IsManager}");
        }
    }

    public string PasswordEntryMasking() {
        string? Password = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && Password.Length > 0)
            {
                Console.Write("\b \b");
                Password = Password[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                Password += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);
        return Password;
    }
}