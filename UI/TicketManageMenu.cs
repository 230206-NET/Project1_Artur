using Models;
using DataAccess;
namespace UI;

public class TicketManageMenu
{
    private readonly IRepository _repo;
    public TicketManageMenu(IRepository repo) {
        _repo = repo;
        //TicketList = _repo.initTicketList();
    }

    List<Ticket> TicketList = new List<Ticket>();
    public void Start(User u, int t)
    {
        
        Console.WriteLine("-----------------------------------------------------------------");
        Console.WriteLine("Welcome to TicketManagement Menu");
        Console.WriteLine("-----------------------------------------------------------------\n");

        while (true) {
            Console.WriteLine("PLEASE ENTER THE NUMBER OF YOUR CHOICE\n");
            Console.WriteLine("0 -> Log out");
            Console.WriteLine("1 -> Approve ticket");
            Console.WriteLine("2 -> Reject ticket");
           
            
            string input= Console.ReadLine()!;

            switch (input) {
                case "0":
                Console.WriteLine("Logged out! Back to the Manager Menu\n");
                return;
                case "1":
                ApproveTicket(t);
                break;
                case "2":
                RejectTicket(t);
                break;
                default:
                Console.WriteLine("Invalid entry");
                break;
            }
        }       
    }

    public void ApproveTicket(int t_id) {
        _repo.ApproveTicket(t_id);
    }

    public void RejectTicket(int t_id) {
        _repo.RejectTicket(t_id);        
    }
}
   