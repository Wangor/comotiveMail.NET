//-----------------------------------------------------------------------
// Copyright 2022 Bärtschi Software (Matthias Bärtschi)
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files
// (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace Library
{
    public class MailBuilder
    {
        MailData _mailData = new MailData();

        public MailBuilder From(string mailAddress, string? name)
        {
            _mailData.From = mailAddress;
            _mailData.FromName = name;

            return this;
        }

        public MailBuilder AddRecipient(string mailAddress,
            RecipientType recipientType = RecipientType.To)
        {
            AddRecipientLocal(mailAddress, recipientType);

            return this;
        }
        
        public MailBuilder AddRecipients(IEnumerable<string> mailAddresses,
            RecipientType recipientType = RecipientType.To)
        {
            foreach (var mailAddress in mailAddresses)
            {
                AddRecipientLocal(mailAddress, recipientType);
            }

            return this;
        }

        private void AddRecipientLocal(string mailAddress,
            RecipientType recipientType)
        {
            switch (recipientType)
            {
                case RecipientType.To:
                {
                    _mailData.Destination.To ??= new List<string>();
                    _mailData.Destination.To.Add(mailAddress);
                    break;
                }
                case RecipientType.Cc:
                {
                    _mailData.Destination.Cc ??= new List<string>();
                    _mailData.Destination.Cc.Add(mailAddress);
                    break;
                }
                case RecipientType.Bcc:
                {
                    _mailData.Destination.Bcc ??= new List<string>();
                    _mailData.Destination.Bcc.Add(mailAddress);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(recipientType), recipientType,
                        null);
            }
        }

        public MailBuilder Subject(string subject)
        {
            _mailData.Message.Subject = subject;

            return this;
        }

        public MailBuilder TextMessage(string text)
        {
            _mailData.Message.Body.Text = text;

            return this;
        }
        
        public MailBuilder HtmlMessage(string htmlBody)
        {
            _mailData.Message.Body.Html = htmlBody;

            return this;
        }

        public MailBuilder ScheduleAt(DateTimeOffset dateTimeOffset)
        {
            _mailData.Delivery = dateTimeOffset;

            return this;
        }

        public MailBuilder AttachFile(string fileName, string pathToFile)
        {
            
            Byte[] bytes = File.ReadAllBytes(pathToFile);

            _mailData.Attachments ??= new List<Attachment>();
            
            _mailData.Attachments.Add(new Attachment()
            {
                Name = fileName,
                Data = Convert.ToBase64String(bytes)
            });
            
            return this;
        }

        public MailData Build()
        {
            return _mailData;
        }
    }
}