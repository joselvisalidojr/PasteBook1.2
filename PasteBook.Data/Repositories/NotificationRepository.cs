using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {

    }
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(PasteBookDbContext context) : base(context)
        {
        }
    }
}
