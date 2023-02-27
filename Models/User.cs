namespace Models;

public class User
{
    protected string? _username;
    protected string? _password;

    protected int _id;

    protected int _isManager;

    public List<Ticket> listOfTickets = new List<Ticket>();

    public User() {

    }
    public User(string? username, string? password, int id, int isManager)
    {
        _username=username;
        _password=password;
        _id=id;
        _isManager=isManager;
    }
    
    public string? Username {
        set {
            _username=value;
        }
        get {
            return _username;
        }
    }
    public string? Password {
        set {
            _password=value;
        }
        get {
            return _password;
        }
    }

     public int ID {
        set {
            _id=value;
        }
        get {
            return _id;
        }
    }

    public int IsManager {
        set {
            _isManager=value;
        }
        get {
            return _isManager;
        }
    }
}




