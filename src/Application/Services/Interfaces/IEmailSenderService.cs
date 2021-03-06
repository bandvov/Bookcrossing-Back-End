﻿using System.Threading.Tasks;
using Application.Dto.Email;

namespace Application.Services.Interfaces
{
    public interface IEmailSenderService
    {
        /// <summary>
        /// Sending email to confirm whether book was delivered
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="bookName"></param>
        /// <param name="bookId"></param>
        /// <param name="requestId"></param>
        /// <param name="userAddress"></param>
        /// <returns></returns>
        Task SendReceiveConfirmationAsync(string userName, string bookName, int bookId, int requestId, string userAddress);

        /// <summary>
        /// Sending email to notify previous owner that book was delivered to user
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendThatBookWasReceivedToPreviousOwnerAsync(RequestMessage message);

        /// <summary>
        /// Sending email to notify new owner that book was delivered to user
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendThatBookWasReceivedToNewOwnerAsync(RequestMessage message);
        /// <summary>
        /// Sending email to notify that book was activated
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendForBookActivatedAsync(RequestMessage message);

        /// <summary>
        /// Sending email to notify that book was deactivated
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendForBookDeactivatedAsync(RequestMessage message);

        /// <summary>
        /// Sending email to notify that request was canceled
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendForCanceledRequestAsync(RequestMessage message);

        /// <summary>
        /// Sending email to user that his book was requested
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        Task SendForRequestAsync(RequestMessage message);

        /// <summary>
        /// Sending email to user that want to reset his password
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="confirmNumber">Unique confirmation number that is available only for 30 minutes</param>
        /// <param name="email">User email</param>
        /// <returns></returns>
        Task SendForPasswordResetAsync(string userName, string confirmNumber, string email);

        Task SendForWishBecameAvailable(string userName, int bookId, string bookName, string email);

        Task SendTheUserWasDeleted(RequestMessage message, string emailMessage);

        Task SendTheUserWasRecovered(RequestMessage message, string emailMessage);

        Task SendForOwnershipAsync(RequestMessage message, string emailMessage);

        


    }
}
