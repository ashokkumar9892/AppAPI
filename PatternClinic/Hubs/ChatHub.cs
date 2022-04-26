using PatternClinic.Utils.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PatternClinic.Utils.Repository.ResponseManager;
using Newtonsoft.Json;
using System.Net;

namespace PatternClinic.Hubs
{
    public class ChatHub : Hub
    {
      
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICommonFunction _common;
        public ChatHub(IWebHostEnvironment webHostEnvironment, ICommonFunction common)
        {
            _webHostEnvironment = webHostEnvironment;
            _common = common;
        }

        public string Test()
        {
            return "";
        }

        //public string GetConnectionId(UserMaster req)
        //{
        //    if (req.SK != null)
        //    {
        //        req.ConnectionId = Context.ConnectionId;
        //        CommonFunction.UpdateConnectionId(req);
        //    }

        //    return Context.ConnectionId;
        //}

        [HttpPost]
        public async Task SendMessages(ChatMessage chatreq)
        { 
            UserMaster req = new UserMaster();
            req.SK = chatreq.SenderSK;            
            req.ConnectionId = Context.ConnectionId;
            req.AuthToken = chatreq.AuthToken;
            var senderResult = CommonFunction.UpdateConnectionId(req).Result;
            if (!senderResult.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                chatreq.IsRead = "false";
                chatreq.CreatedOn = DateTime.UtcNow.ToString();

                string[] S = chatreq.SenderSK.Split("_");
                string[] R = chatreq.ReceiverSK.Split("_");
               
                Int64 Se = Convert.ToInt64(S[1]);
                Int64 Re = Convert.ToInt64(R[1]);
                chatreq.UniqueId = Se > Re ? Se + "_" + Re : Re + "_" + Se;

             
                
                var GetIsLastTrueResult = CommonFunction.GetIsLastChat(chatreq).Result;
                var GetIsLastTrueResultString = CommonFunction.ContentToString(GetIsLastTrueResult.Content);
                var Response = JsonConvert.DeserializeObject<List<ChatRoot>>(GetIsLastTrueResultString);
                foreach (var k in Response)
                {
                    chatreq.ChatSK = Response.FirstOrDefault().SK.s;
                    var IsFalseResult = CommonFunction.UpdateIsLastChat(chatreq).Result;
                    var IsFalseResultString = CommonFunction.ContentToString(IsFalseResult.Content);
                    dynamic jsonResult = JsonConvert.DeserializeObject(IsFalseResultString);
                }
              
               
                 
                    chatreq.IsLast = "true";

                    //Add Chat to database starts
                    var patientResult = CommonFunction.AddChatMessage(chatreq).Result;
                    var patientStringResult = CommonFunction.ContentToString(patientResult.Content);
                    dynamic json = JsonConvert.DeserializeObject(patientStringResult);
                    //Add Chat to database ends

                    if (json == "Registered")
                    {
                        LoadMessages mymsgs = new LoadMessages();
                        mymsgs.message = chatreq.Message;
                        mymsgs.MessageType = chatreq.MessageType;
                        mymsgs.senton = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss");
                        mymsgs.SenderSK = chatreq.SenderSK;

                        UserMaster receiverInfo = new UserMaster();
                        receiverInfo.SK = chatreq.ReceiverSK;
                        receiverInfo.AuthToken = chatreq.AuthToken;
                        var receiverResult = CommonFunction.GetPatientInfoBySK(receiverInfo).Result;
                        var receiverStringResult = CommonFunction.ContentToString(receiverResult.Content);
                        var objResponse = JsonConvert.DeserializeObject<List<UserInfo>>(receiverStringResult);

                        string ReceiverConnectionId = objResponse.FirstOrDefault().ConnectionId != null ? objResponse.FirstOrDefault().ConnectionId.S : null;
                        await Clients.Client(ReceiverConnectionId).SendAsync("ReceiveMessage", mymsgs);
                    }
                }            
        }     


        public class LoadMessages
        {
            public string SenderSK { get; set; }
            public string Image { get; set; }
            public string senton { get; set; }
            public string message { get; set; }
            public string MessageType { get; set; }
        }
        
        public async Task UserChatList(UserMaster Chatlistreq)
        {
            Chatlistreq.ConnectionId = Context.ConnectionId;
            var senderResult = CommonFunction.UpdateConnectionId(Chatlistreq).Result;

            List<ChatListResponse> mylist = new List<ChatListResponse>();


            ChatList req = new ChatList();
            req.SenderId = Chatlistreq.SK;
            req.AuthToken = Chatlistreq.AuthToken;
            var receiverResult = CommonFunction.GetChatHistory(req).Result;
            var receiverStringResult = CommonFunction.ContentToString(receiverResult.Content);
            var objResponse = JsonConvert.DeserializeObject<List<ChatRoot>>(receiverStringResult);



                // senderid-- - loggedin

            //var agent = userchat.Where(x => ((x.SenderId != userId && x.Sender.RoleId == 2) || (x.ReceiverId != userId && x.Receiver.RoleId == 2)))
            //.GroupBy(g => new { g.UniqueId })
            //             .Select(gcs => new ChatListResponse
            //             {
            //                 SenderId = userRecord.UserId,
            //                 RecieverId = userRecord.UserId == gcs.Last().ReceiverId ? gcs.Last().SenderId : gcs.Last().ReceiverId,
            //                 Image = userRecord.UserId == gcs.Last().ReceiverId ? (_common.baseurl() + gcs.Last().Sender.Selfie) : (_common.baseurl() + gcs.Last().Receiver.Selfie),

            //                 Name = userRecord.UserId == gcs.Last().ReceiverId ? (gcs.Last().Sender.FirstName + " " + gcs.Last().Sender.LastName) : (gcs.Last().Receiver.FirstName + " " + gcs.Last().Receiver.LastName),
            //                 LastMessage = gcs.Last().Message,
            //                 Count = gcs.Count(),
            //                 TimeBefore = _common.GetNotificationTimeBefore(gcs.Last().CreatedOn.ToString()).TimeBefore,
            //                 LastMessageType = gcs.Last().MessageType,
            //                 unseenCount = gcs.Count(x => x.ToIsRead == false),
            //                 ChatId = gcs.Last().ChatId,
            //                 UniqueNumber = gcs.Key.UniqueId
            //             }).OrderByDescending(x => x.ChatId).ToList();


         //   mylist = agent;

            await Clients.Client(Chatlistreq.ConnectionId).SendAsync("ShowUserChatList", mylist);
        }
    }

    public enum ChatType
    {
        Text,
        Image,
        Audio,
        Location,
        Video,
        File
    }
}
