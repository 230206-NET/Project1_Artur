using Models;
namespace DataAccess;
public interface IRepository
{
    /// <summary>
    /// Retrieves all tickets
    /// </summary>
    /// <returns>a list of tickets</returns>
    //List<Ticket> ViewTickets(User u);

    void UserLogin(string? username, string? password);

    /// <summary>
    /// Persists a new ticket to storage
    /// </summary>
    Ticket CreateNewTicket(User u, string? description, Decimal amount);

    /// <summary>
    /// Persists a new user to storage
    /// </summary>
    User CreateNewUser(string? name, string? password, int isManager);

    List<User> initUserList();

    void initTicketList(List<User> uList);

    void ViewTickets(User u);
    List<User> ShowUsersList();

    void ApproveTicket(int t_id);

    void RejectTicket(int t_id);

    void ViewAllTickets(User u);
}