namespace apief;

    public class Account
    {
        public int Id {get; set;}
        public string Email {get; set;}="";
        public byte[]? PasswordHash {get; set;}
        public byte[]? PasswordSalt {get; set;}
        public string TokenValue {get; set;}="";

        
    }
