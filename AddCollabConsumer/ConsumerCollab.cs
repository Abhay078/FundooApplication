using CommonLayer.Model;
using MassTransit;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AddCollabConsumer
{
    public class ConsumerCollab : IConsumer<AddCollabModel>
    {
        private readonly FunContext context;
        public ConsumerCollab(FunContext context)
        {
            this.context = context;
        }

        public Task Consume(ConsumeContext<AddCollabModel> collab)
        {
            var data = collab.Message;
            CollabEntity collabEntity = new CollabEntity();
            collabEntity.NoteId = data.NoteId;
            collabEntity.Email = data.Email;
            collabEntity.UserId = data.UserId;
            context.Collab.Add(collabEntity);
            context.SaveChanges();
            string subject = "Ticket Confirmation Mail";
            string body = $"You are Successfully added as a Collaborator for a note by User Have UserId:- {data.UserId} ";
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("abhaysri259@gmail.com", "exmnxmmtxebhpukf"),
                EnableSsl = true,
            };
            smtp.Send("abhaysri259@gmail.com", $"{data.Email}", subject, body);

            return Task.CompletedTask;

        }
    }
}
