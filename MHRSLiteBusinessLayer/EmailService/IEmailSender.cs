using MHRSLiteEntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.EmailService
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
    }
}
