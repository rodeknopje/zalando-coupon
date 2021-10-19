using System.Collections.Generic;

namespace Zalando.Models.Guerrilla
{
    public class Inbox
    {
        public List<Message> List { get; set; }
    }
    
    public class Message
    {
        public string MailFrom { get; set; }
        public int MailId { get; set; }
    }
}