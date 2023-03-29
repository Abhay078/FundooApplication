using CommonLayer.Model;
using MassTransit;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AddCollabConsumer
{
    public class ConsumerCollab : IConsumer<AddCollabModel>
    {

        public Task Consume(ConsumeContext<AddCollabModel> collab)
        {
            try
            {
                var data = collab.Message;
                string subject = "Ticket Confirmation Mail";
                string body = $"You are Successfully added as a Collaborator for a note by User in Note Having Id :- {data.NoteId}";
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("abhaysri259@gmail.com", "exmnxmmtxebhpukf"),
                    EnableSsl = true,
                };
                smtp.Send("abhaysri259@gmail.com", $"{data.Email}", subject, body);

                return Task.CompletedTask;

            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
