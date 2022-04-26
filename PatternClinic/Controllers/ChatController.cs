using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatternClinic.Utils.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PatternClinic.Utils.Repository.ResponseManager;

namespace PatternClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly ICommonFunction _common;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatController(ICommonFunction common, IWebHostEnvironment hostEnvironment,
              IHttpContextAccessor httpContextAccessor)
        {
            _common = common;
            _webHostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("MyChat")]
        public ResponseChatList MyChat(UserMaster req)
        {
            try
            {
                //var patientResult = CommonFunction.AddChatMessage().Result;
                //var patientStringResult = CommonFunction.ContentToString(patientResult.Content);
                //dynamic json = JsonConvert.DeserializeObject(patientStringResult);


                //var mychatlist = _db.ChatHistory.Where(x => (x.ReceiverId == token.UserId && x.SenderId == req.UserId || (x.ReceiverId == req.UserId && x.SenderId == token.UserId))).Select(x => new ChatList()
                //{
                //    Message = x.Message,
                //    ReceiverId = x.ReceiverId,
                //    SenderId = x.SenderId,
                //    MessageOn = x.CreatedOn,
                //    SentOn = ((DateTime)(x.CreatedOn)).ToString("MM/dd/yyyy hh:mm:ss"),
                //    ReceiverImage = x.Receiver.Selfie,
                //    SenderImage = x.Sender.Selfie,
                //    ChatType = x.MessageType,
                //    FromIsRead = x.FromIsRead,
                //    ToIsRead = x.ToIsRead,
                //    TotalBill = null,
                //    ChatId = x.ChatId
                //}).OrderByDescending(x => x.MessageOn).ToList();


                //foreach (var chat in mychatlist)
                //{
                //    var k = _db.ChatHistory.SingleOrDefault(x => x.ChatId == chat.ChatId && x.FromIsRead == false);
                //    if (k != null)
                //    {
                //        k.FromIsRead = true;
                //        k.ToIsRead = true;
                //        _db.SaveChanges();
                //    }
                //}


                return new ResponseChatList
                {
                   // chatlist = mychatlist
                };

            }
            catch (Exception ex)
            {
                return new ResponseChatList(ResponseCode.InternalServerError, ex.Message);
            }
        }


    }
}
