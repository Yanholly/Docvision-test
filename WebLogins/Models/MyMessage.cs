using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLogins.Models
{
    public class MyMessage
    {
        public int MessageId { get; set; }
        public string Title { get; set; }
        public List <string> Tags { get; set; }
        public string Sender { get; set; }
        public string Address { get; set; }
        public string Date { get; set; }
        public string Content { get; set; }
        

        public MyMessage(int id, string title, List<string> tags, string sender, string address, string date, string content)
        {
            MessageId = id;
            Title = title;
            Tags = tags;
            Sender = sender;
            Address = address;
            Date = date;
            Content = content;
           
        }
    }
}