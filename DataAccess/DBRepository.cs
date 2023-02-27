using Models;
using System.Data.SqlClient;
using Serilog;
namespace DataAccess;


public class DBRepository : IRepository
{
    // public List<Ticket> ViewTickets(User u) {
    //     return new List<Ticket>();
    // }

    public List<User> initUserList() {
        List<User> UserList = new  List<User>();

        using SqlConnection connection = new SqlConnection(Secrets.getConnectionString()); 

        // Click the "Connect" button
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM USERS", connection);
        using SqlDataReader reader = cmd.ExecuteReader();
        
        while(reader.Read()) {
            int wId = (int) reader["ID"];
            string wName = (string) reader["NAME"];
            string wPassword = (string) reader["PASSWORD"];
            int wIsManager = (int) reader["ISMANAGER"];
            UserList.Add(new User(wName,wPassword,wId,wIsManager));
        }

        return UserList;
    }

    public void initTicketList(List<User> uList) {
        List<Ticket> TicketList = new  List<Ticket>();

        using SqlConnection connection = new SqlConnection(Secrets.getConnectionString()); 

        // Click the "Connect" button
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM TICKET", connection);
        using SqlDataReader reader = cmd.ExecuteReader();
        
        while(reader.Read()) {
            int wId = (int) reader["ID"];
            int wuserId = (int) reader["UserID"];
            string wDescription = (string) reader["DESCRIPTION"];
            decimal wAmount= (decimal) reader["AMOUNT"];
            foreach (User u in uList) {
                if (u.ID==wuserId)
                    u.listOfTickets.Add(new Ticket(wId,wuserId,wDescription,wAmount));
            }
        }
    }

    public void ViewTickets(User u) {
        //List<Ticket> UserList = new  List<Ticket>();

        using SqlConnection connection = new SqlConnection(Secrets.getConnectionString()); 

        // Click the "Connect" button
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM TICKET Where UserID = @id", connection);
        cmd.Parameters.AddWithValue("@id", u.ID);
        using SqlDataReader reader = cmd.ExecuteReader();
        
        while(reader.Read()) {
            int wId = (int) reader["ID"];
            string wDescription = (string) reader["DESCRIPTION"];
            decimal wAmount= (decimal) reader["AMOUNT"];
            int wStatus= (int) reader["STATUS"];
            string statusText="";
            if (wStatus==0)
                statusText="Pending";
            else if (wStatus==1)
                statusText="Approved";
            else if (wStatus==2)
                statusText="Rejected";
            Console.WriteLine($"Ticket ID{wId}: Spent {wAmount:C2} for {wDescription.ToLower()}. Status is {statusText.ToLower()}");
        }
    }

    public void UserLogin(string? username, string? password) {
        using SqlConnection connection = new SqlConnection(Secrets.getConnectionString()); 

        // Click the "Connect" button
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM USERS", connection);
        using SqlDataReader reader = cmd.ExecuteReader();
        
        while(reader.Read()) {
            string wName = (string) reader["NAME"];
            string wPassword = (string) reader["PASSWORD"];
            //int? eId = reader.IsDBNull(3) ? null : (int) reader["eID"];
            if(wName==username && wPassword==password) {
                Console.WriteLine("\nYou logged in successfully!",Console.ForegroundColor=ConsoleColor.Green);
                Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
                return;
            }            
        }
        Console.WriteLine("\nThe combination of Username nad Password is not correct",Console.ForegroundColor=ConsoleColor.Red);
        Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
    }

    /// <summary>
    /// Persists a new ticket to storage
    /// </summary>
    public Ticket CreateNewTicket(User u, string? description, Decimal amount) {
        Ticket ticket = new Ticket(u.ID,description,amount);
        using SqlConnection conn = new SqlConnection(Secrets.getConnectionString());
        conn.Open();


        using SqlCommand cmd = new SqlCommand("INSERT INTO Ticket(Description, Amount, UserID, Status) OUTPUT INSERTED.Id Values (@wDescription, @wAmount, @wUserId, @wStatus)", conn);
        cmd.Parameters.AddWithValue("@wDescription", description);
        cmd.Parameters.AddWithValue("@wAmount", amount);
        cmd.Parameters.AddWithValue("@wUserId", u.ID);       
        cmd.Parameters.AddWithValue("@wStatus", ticket.Status);
        Console.WriteLine($"The ticket is successfully created",  Console.ForegroundColor=ConsoleColor.Green);
        Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
        int createdId = (int) cmd.ExecuteScalar();
        ticket.UserId=createdId;   
        
        //u.listOfTickets.Add(ticket);

        return ticket;
    }

    /// <summary>
    /// Persists a new user to storage
    /// </summary>
    public User CreateNewUser(string? name, string? password, int isManager) {
        try 
        {
            using SqlConnection conn = new SqlConnection(Secrets.getConnectionString());
            conn.Open();

            using SqlCommand cmd = new SqlCommand("INSERT INTO Users(Name, Password, IsManager) OUTPUT INSERTED.Id Values (@wName, @wPassword, @wIsManager)", conn);
            cmd.Parameters.AddWithValue("@wName", name);
            cmd.Parameters.AddWithValue("@wPassword", password);
            cmd.Parameters.AddWithValue("@wIsManager", isManager);

            int createdId = (int) cmd.ExecuteScalar();

            User user = new User(name,password,createdId,isManager);
            Console.WriteLine($"User with the name {name} is successfully created",  Console.ForegroundColor=ConsoleColor.Green);
            Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
            
            return user;

            // you might want to do something if rowsAffected == 0;
            //sessionToCreate.Id = createdId;

            // foreach(Exercise ex in sessionToCreate.WorkoutExercises)
            // {
            //     using SqlCommand ecmd = new SqlCommand("INSERT INTO Exercises(ExerciseName, ExerciseNote) OUTPUT INSERTED.Id Values (@eName, @eNote)", conn);

            //     ecmd.Parameters.AddWithValue("@eName", ex.Name);
            //     ecmd.Parameters.AddWithValue("@eNote", ex.Notes);

            //     int eId = (int) ecmd.ExecuteScalar();
            //     ex.Id = eId;
            // }
        }
        catch (SqlException ex) {
            Console.WriteLine($"Username or password format is not valid",  Console.ForegroundColor=ConsoleColor.Red);
            Console.WriteLine($"Username length must be at least 4 and Password length must be at least 8 and must contain 1 uppercase, 1 lowercase letter, 1 digit and 1 special character ",  Console.ForegroundColor=ConsoleColor.Yellow);
            Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
            Log.Error("Caught SQL Exception trying to create new user");
            // Log.Error("Caught SQL ExceptiConsole.WriteLine($"Username or password format is not valid",  Console.ForegroundColor=ConsoleColor.red);Console.WriteLine($"Username or password format is not valid",  Console.ForegroundColor=ConsoleColor.red);Console.WriteLine($"Username or password format is not valid",  Console.ForegroundColor=ConsoleColor.red);on trying to create new user {0}", ex);
            //throw ex;
            return new User();
        }
    }

    public void ShowUsersList() {
        using SqlConnection connection = new SqlConnection(Secrets.getConnectionString()); 

        // Click the "Connect" button
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM USERS", connection);
        
        using SqlDataReader reader = cmd.ExecuteReader();
        Console.WriteLine($"\nRegistered Users list",  Console.ForegroundColor=ConsoleColor.Green);
        Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
        while(reader.Read()) {
            int wId = (int) reader["ID"];
            string wName = (string) reader["NAME"];
            Console.WriteLine($"{wId} - {wName}");
        }
    }   

    public void ApproveTicket(int t_id) {
        using SqlConnection conn = new SqlConnection(Secrets.getConnectionString());
        conn.Open();
        
        using SqlCommand cmd = new SqlCommand($"UPDATE Ticket SET STATUS=1 where ID={t_id}", conn);
        cmd.ExecuteReader();
        // cmd.Parameters.AddWithValue("@wt_id", t_id);
        // Console.WriteLine("@wt_id");
        Console.WriteLine($"Ticket {t_id} is successfully updated",  Console.ForegroundColor=ConsoleColor.Green);
        Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
    }

    public void RejectTicket(int t_id) {
        using SqlConnection conn = new SqlConnection(Secrets.getConnectionString());
        conn.Open();
        
        using SqlCommand cmd = new SqlCommand($"UPDATE Ticket SET STATUS=2 where ID={t_id}", conn);
        cmd.ExecuteReader();
        // cmd.Parameters.AddWithValue("@wt_id", t_id);
        // Console.WriteLine("@wt_id");
        Console.WriteLine($"Ticket {t_id} is successfully updated",  Console.ForegroundColor=ConsoleColor.Green);
        Console.WriteLine("",Console.ForegroundColor=ConsoleColor.White);
    }

    public void ViewAllTickets(User u) {

        using SqlConnection connection = new SqlConnection(Secrets.getConnectionString()); 

        // Click the "Connect" button
        connection.Open();

        using SqlCommand cmd = new SqlCommand("SELECT * FROM TICKET Where UserID != @id", connection);
        cmd.Parameters.AddWithValue("@id", u.ID);
        using SqlDataReader reader = cmd.ExecuteReader();
        
        while(reader.Read()) {
            int wId = (int) reader["ID"];
            string wDescription = (string) reader["DESCRIPTION"];
            decimal wAmount= (decimal) reader["AMOUNT"];
            int wStatus= (int) reader["STATUS"];
            string statusText="";
            if (wStatus==0)
                statusText="Pending";
            else if (wStatus==1)
                statusText="Approved";
            else if (wStatus==2)
                statusText="Rejected";
            Console.WriteLine($"Ticket ID{wId}: Spent {wAmount:C2} for {wDescription.ToLower()}. Status is {statusText.ToLower()}");
        }

    }
}
