using PasteBook.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        public IAlbumRepository AlbumRepository { get; }
        public IBlockedAccountRepository BlockedAccountRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public IFriendRepository FriendRepository { get; }
        public IFriendRequestRepository FriendRequestRepository { get; }
        public IImageRepository ImageRepository { get; }
        public ILikeRepository LikeRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IPostRepository PostRepository { get; }
        public IUserAccountRepository UserAccountRepository { get; }

    }
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private PasteBookDbContext context;
        public IAlbumRepository AlbumRepository { get; private set; }
        public IBlockedAccountRepository BlockedAccountRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
        public IFriendRepository FriendRepository { get; private set; }
        public IFriendRequestRepository FriendRequestRepository { get; private set; }
        public IImageRepository ImageRepository { get; private set; }
        public ILikeRepository LikeRepository { get; private set; }
        public INotificationRepository NotificationRepository { get; private set; }
        public IPostRepository PostRepository { get; private set; }
        public IUserAccountRepository UserAccountRepository { get; private set; }

        public UnitOfWork(PasteBookDbContext context)
        {
            this.context = context;
            this.AlbumRepository = new AlbumRepository(context);
            this.BlockedAccountRepository = new BlockedAccountRepository(context);
            this.CommentRepository = new CommentRepository(context);
            this.FriendRepository = new FriendRepository(context);
            this.FriendRequestRepository = new FriendRequestRepository(context);
            this.ImageRepository = new ImageRepository(context);
            this.LikeRepository = new LikeRepository(context);
            this.NotificationRepository = new NotificationRepository(context);
            this.PostRepository = new PostRepository(context);
            this.UserAccountRepository = new UserAccountRepository(context);
        }

        public async Task CommitAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
