﻿using Auto_Part_WebUI.AppCode.Extensions;
using Auto_Part_WebUI.AppCode.Infrastructure;
using Auto_Part_WebUI.Models.DataContexts;
using Auto_Part_WebUI.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Vapie.WebUI.AppCode.Modules.SubscribeModule
{
    public class SubscribeCreateCommand : IRequest<CommandJsonResponse>
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Cannot Empty")]
        [EmailAddress(ErrorMessage = "Email does not contain")]
        public string Email { get; set; }
        public class SubscribeCreateCommandHandler : IRequestHandler<SubscribeCreateCommand, CommandJsonResponse>
        {
            readonly ECoPartDbContext db;
            readonly IConfiguration configuration;
            readonly IActionContextAccessor ctx;
            public SubscribeCreateCommandHandler(ECoPartDbContext db,IConfiguration configuration,IActionContextAccessor ctx)
            {
                this.db = db;
                this.configuration = configuration;
                this.ctx = ctx;
            }
            public async Task<CommandJsonResponse> Handle(SubscribeCreateCommand request, CancellationToken cancellationToken)
            {
                var displayName = configuration["emailAccount:displayName"];
                var smtpServer = configuration["emailAccount:smtpServer"];
                var smtpPort = Convert.ToInt32(configuration["emailAccount:smtpPort"]);
                var userName = configuration["emailAccount:userName"];
                var password = configuration["emailAccount:password"];
                //var cc = configuration["emailAccount:cc"];

                var subscribe = await db.Subscribes
                    .FirstOrDefaultAsync(s=>s.Email.Equals(request.Email),cancellationToken);
                if (subscribe==null)
                {
                    subscribe = new Subscribe();
                    subscribe.Email = request.Email;
                    subscribe.Id = request.Id;
                    await db.Subscribes.AddAsync(subscribe, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);
                }
                else if (subscribe.EmailSended == true)
                {
                    return new CommandJsonResponse
                    {
                        Error = true,
                        Message = "Siz artiq abunesiniz"
                    };
                }
                

                try
                {

                    string token = $"{subscribe.Id}-{subscribe.Email}".Encrypt();
                    string link = $"{ctx.GetAppLink()}/subscribe-confirm?token={token}";

                    SmtpClient client = new SmtpClient(smtpServer,smtpPort);
                    client.Credentials = new NetworkCredential(userName, password);
                    client.EnableSsl = false;

                    var from = new MailAddress(userName, displayName);
                    MailMessage message = new MailMessage(from, new MailAddress(subscribe.Email));
                    message.Subject = "Ecoparts Confirmation Mail";
                    message.Body = $"Please confirm subscribtion with <a href=\"{link}\">link</a>";
                    message.IsBodyHtml = true;
                    //string[] ccs = cc.Split(';', StringSplitOptions.RemoveEmptyEntries);

                    //foreach (var item in ccs)
                    //{
                    //    message.Bcc.Add(item);
                    //}

                    client.Send(message);

                    subscribe.EmailSended = true;
                    await db.SaveChangesAsync(cancellationToken);
                    
                }
                catch (Exception ex)
                {

                    return new CommandJsonResponse
                    {
                        Error = true,
                        Message = ex.Message
                    };
                }

                return new CommandJsonResponse
                {
                    Error = false,
                    Message = $"Abuneliyi tamamlamaq ucun \"{subscribe.Email}\"-e gonderilmish emeliyyati yerine yetirin!"
                };
            }
        }
    }
}
