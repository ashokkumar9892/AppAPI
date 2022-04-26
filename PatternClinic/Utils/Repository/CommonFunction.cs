using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using RestSharp;
using static PatternClinic.Utils.Repository.ResponseManager;


namespace PatternClinic.Utils.Repository
{
    public interface ICommonFunction
    {
        string baseurl();
        string GeneratePasswordSaltedHashString(string input);
        string GenerateNewPassword(int lowercase, int uppercase, int numerics, int SpecialCharacter);
        //string Convert_StringvalueToHexvalue(string stringvalue, System.Text.Encoding encoding);
        string GetToken(int length, Random random);
       
        string Convert_StringvalueToHexvalue(string stringvalue, System.Text.Encoding encoding);
        string Convert_HexvalueToStringvalue(string hexvalue, System.Text.Encoding encoding);
    }
    public class CommonFunction : ICommonFunction
    {
       
        private readonly IWebHostEnvironment _webHostEnvironment;       
        public string baseurl()
        {
            return "http://petjet.harishparas.com/";
        }
        public static string dynamodburl()
        {
            return "https://rpmcrudapis20210808220332demo.azurewebsites.net/api/";
        }
        public static string apidynamodburl()
        {
            return "https://appapi.apatternplus.com/api/DynamoDbAPIs/";
        }
        public string GeneratePasswordSaltedHashString(string input)
        {
            var salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
            var plainText = Encoding.UTF8.GetBytes(input);
            HashAlgorithm algorithm = new SHA256Managed();

            var plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (var i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (var i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            var b = algorithm.ComputeHash(plainTextWithSaltBytes);

            var str = Convert.ToBase64String(b);
            return str;
        }
        public string GenerateNewPassword(int lowercase, int uppercase, int numerics, int SpecialCharacter)
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";
            string Special = "$#@%&";

            Random random = new Random();

            string generated = "!";

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );
            for (int i = 1; i <= SpecialCharacter; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    Special[random.Next(Special.Length - 1)].ToString()
                );
            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);

        }
        public string GetToken(int length, Random random)
        {
            var result = new StringBuilder();
            try
            {
                var characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                result = new StringBuilder(length);
                for (var i = 0; i < length; i++)
                {
                    result.Append(characters[random.Next(characters.Length)]);
                }
            }
            catch (Exception ex)
            {
             //   Logger.Error("Error in GetToken : " + ex.InnerException, ex);
            }
            return result.ToString();
        }
        public static string ContentToString(HttpContent httpContent)
        {
            return httpContent == null ? "" : httpContent.ReadAsStringAsync().Result;
        }
        public string Convert_StringvalueToHexvalue(string stringvalue, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(stringvalue);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }
        public string Convert_HexvalueToStringvalue(string hexvalue, System.Text.Encoding encoding)
        {
            int CharsLength = hexvalue.Length;
            byte[] bytesarray = new byte[CharsLength / 2];
            for (int i = 0; i < CharsLength; i += 2)
            {
                bytesarray[i / 2] = Convert.ToByte(hexvalue.Substring(i, 2), 16);
            }
            return encoding.GetString(bytesarray);
        }                
        public static async Task<HttpResponseMessage> GetdynamodbToken(string username, string password)
        {          
             
                var body = new
                {
                    Username = username,
                    Password = password
                };

                var requestBody = new HttpRequestMessage(HttpMethod.Post,  dynamodburl() +"signin")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
                };
                var response = await SendPost(dynamodburl() +"signin", requestBody);

                return response;
         
        }
        public static async Task<HttpResponseMessage> SendPost(string url, HttpRequestMessage requestBody)
        { 
            var client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
           
            var response = await client.PostAsync(url, requestBody.Content);

            if (!response.IsSuccessStatusCode)
            {
              
            }          
            return response;
        }
        public static async Task<HttpResponseMessage> GetAllPatients(string AuthToken)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {

                    TableName = "UserDetailsDemo",
                    ProjectionExpression = "PK,SK,UserId",
                    KeyConditionExpression = "PK = :v_PK AND begins_with(SK, :v_SK)",
                    ExpressionAttributeValues = new ExpressionAttributeValues_GetInfo()
                    {
                        VPK = new VPK()
                        {
                            S = "patient"
                        },
                        //VSK = new VSK()
                        //{
                        //    S = "PATIENT_"
                        //}
                    }

                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(dynamodburl()+ "DynamoDbAPIs/getitem", content);
                return response;
            }
        }
        public static async Task<HttpResponseMessage> GetPatientInfo(UserMaster req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    TableName = "UserDetailsDemo",
                    ProjectionExpression = "PK,SK,UserId,UserName,Email,ContactNo,DOB,DoctorName,CarecoordinatorName,Coach,Height,reading,diastolic,systolic,weight,BMI,FirstName,LastName,Gender,Lang,Street,City,Zip,WorkPhone,MobilePhone,ActiveStatus,Notes,ProfileImage,Country,Weight,ReferAs,WeightUnit",
                    KeyConditionExpression = "PK = :v_PK",
                    FilterExpression = "UserName = :v_username",
                    ExpressionAttributeValues = new ExpressionAttributeValues_GetInfo()
                    {
                        VPK = new VPK()
                        {
                            S = "patient"
                        },
                        UserName = new LoginUserName()
                        {                            
                              S = req.UserName
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(dynamodburl()+ "DynamoDbAPIs/getitem", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> GetPatientInfoBySK(UserMaster req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {

                    TableName = "UserDetailsDemo",
                    ProjectionExpression = "PK,SK,UserId,UserName,Email,ContactNo,DOB,DoctorName,CarecoordinatorName,Coach,Height,reading,diastolic,systolic,weight,BMI,FirstName,LastName,Gender,Lang,Street,City,Zip,WorkPhone,MobilePhone,ActiveStatus,Notes,ProfileImage",
                    KeyConditionExpression = "PK = :v_PK AND SK = :v_SK",
         
                    ExpressionAttributeValues = new ExpressionAttributeValues_GetInfoBySK()
                    {
                        VPK = new GetVPK()
                        {
                            S = "patient"
                        },
                        SK = new VSK()
                        {
                            S = req.SK
                        }
                    }                    
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(dynamodburl() + "DynamoDbAPIs/getitem", content);

                return response;
            }
        }

        public static async Task<HttpResponseMessage> GetIsLastChat(ChatMessage req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    TableName = "UserDetailsDemo",
                    ProjectionExpression = "SK,MessageOn,Message",
                    KeyConditionExpression = "PK = :v_PK",
                    FilterExpression = "UnqueId = :v_UnqueId AND IsLast =  :v_IsLast",
                    ExpressionAttributeValues = new ExpressionAttributeValues_GetIsLastChat()
                    {
                        VPK = new VPK()
                        {
                            S = "Chat_History"
                        },
                        UnqueId = new UnqueId()
                        {
                            s = req.UniqueId
                        },
                        IsLast = new IsLast
                        { 
                            S = "true"
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(dynamodburl() + "DynamoDbAPIs/getitem", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> UpdatePatientInfo(UserMaster req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    TableName = "UserDetailsDemo",
                    Key = new Key()
                    {
                        PK = new UpdatePK { s = "patient" },
                        SK = new UpdateSK { s = req.SK }
                    },
                    UpdateExpression = "SET Height = :v_Height, Weight = :v_Weight, FirstName = :v_FirstName, LastName = :v_LastName, Country = :v_Country, Email = :v_Email, ProfileImage = :v_ProfileImage, Gender = :v_Gender, DOB = :v_DOB, ReferAs = :v_ReferAs, WeightUnit = :v_Unit",
                    ExpressionAttributeValues = new ExpressionAttributeValues()
                    {                        
                        Height = new UpdatedHeight()
                        {
                            s = req.Height
                        },
                        Weight = new UpdatedWeight()
                        {
                            s = req.Weight
                        },
                        FirstName = new UpdatedFirstName()
                        {
                            s = req.FirstName
                        },
                        LastName = new UpdatedLastName()
                        {
                           s = req.LastName
                        },
                        Country = new UpdatedCountry()
                        {
                            s = req.Country
                        },
                        Email = new UpdatedEmail()
                        {
                            s = req.Email
                        },
                        ProfileImage = new ProfileImage()
                        { 
                            s = req.ProfilePic
                        },
                         DOB = new UpdatedDOB()
                         {
                            S = req.DOB
                         },
                         Gender = new UpdatedGender()
                         {
                            S = req.Gender
                         },
                         ReferAs = new ReferAs()
                         { 
                            S = req.ReferAs
                         },
                         WeightUnit = new WeightUnit()
                         { 
                         S = req.WeightUnit
                         }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(apidynamodburl() +"updateitem", content);
                return response;
            }
        }
        public static async Task<HttpResponseMessage> UpdateEmailinPool(UserMaster req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new EmailRequest
                {
                    Email = req.Email,
                    Username = req.UserName
                };
                

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");             
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync("https://appapi.apatternplus.com/api/" + "updateEmail", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> UpdateConnectionId(UserMaster req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    TableName = "UserDetailsDemo",
                    Key = new Key()
                    {
                        PK = new UpdatePK { s = "patient" },
                        SK = new UpdateSK { s = req.SK }
                    },
                    UpdateExpression = "SET ConnectionId = :v_ConnectionId",
                    ExpressionAttributeValues = new ChatExpressionAttributeValues()
                    {
                         ConnectionId = new ConnectionId()
                        {
                           S  = req.ConnectionId
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(apidynamodburl() + "updateitem", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> UpdateIsLastChat(ChatMessage req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    TableName = "UserDetailsDemo",
                    Key = new Key()
                    {
                        PK = new UpdatePK { s = "Chat_History" },
                        SK = new UpdateSK { s = req.ChatSK }
                    },
                    UpdateExpression = "SET IsLast = :v_IsLast",
                    ExpressionAttributeValues = new IsLastExpressionAttributeValues()
                    {
                        IsLast = new IsLast { S = "false" }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(apidynamodburl() + "updateitem", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> UpdateTeam(UserMaster req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    TableName = "UserDetailsDemo",
                    Key = new Key()
                    {
                        PK = new UpdatePK { s = "patient" },
                        SK = new UpdateSK { s = req.SK }
                    },
                    UpdateExpression = "SET DoctorId = :v_DoctorId, DoctorName = :v_DoctorName, Coach = :v_Coach, CoachId = :v_CoachId, GSI1SK = :v_GSI1SK",
                    ExpressionAttributeValues = new ExpressionAttributeValues_UpdateTeam()
                    {
                        DoctorId = new DoctorId()
                        {
                           S  = req.DoctorId
                        },
                        DoctorName = new UpdatedDoctorName()
                        {
                            S = req.DoctorName
                        },
                        Coach = new UpdateCoach()
                        {
                            S = req.CoachName
                        },
                        CoachId = new CoachId()
                        {
                            S = req.CoachId
                        },
                        GSI1SK = new GSI1SK()
                        {
                            S = req.DoctorId
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(apidynamodburl() + "updateitem", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> ForgotPatientPassword(UserMaster req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                   Username = req.UserName
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(dynamodburl() + "forgotpassword", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> ResetPatientPassword(ResetPasswordRequest req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    Username = req.username,
                    NewPassword = req.newpassword,
                    ConfirmationCode = req.confirmationcode
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(dynamodburl() + "confirmforgotpassword", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> GetDoctorsList(UserMaster req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {

                    TableName = "UserDetailsDemo",
                    ProjectionExpression = "PK,SK,UserId,UserName,LastName,ActiveStatus, Notes,ProfileImage",
                    KeyConditionExpression = "PK = :v_PK",
                  
                    ExpressionAttributeValues = new ExpressionAttributeValues_GetdoctorInfo()
                    {
                        VPK = new VPK()
                        {
                            S = "doctor"
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(dynamodburl() + "DynamoDbAPIs/getitem", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> GetCoachList(UserMaster req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {

                    TableName = "UserDetailsDemo",
                    ProjectionExpression = "PK,SK,UserId,UserName,LastName,ActiveStatus, Notes,ProfileImage",
                    KeyConditionExpression = "PK = :v_PK",

                    ExpressionAttributeValues = new ExpressionAttributeValues_GetdoctorInfo()
                    {
                        VPK = new VPK()
                        {
                            S = "coach"
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(dynamodburl() + "DynamoDbAPIs/getitem", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> AddChatMessage(ChatMessage chatmsg)
        {
            Random rnd = new Random();
         

            using (HttpClient client = new HttpClient())
            {
                var body = new chatdata()
                {
                    PK = "Chat_History",
                    CreatedOn = chatmsg.CreatedOn,
                    SK = "ChatHistory_"+ rnd.Next(),
                    IsRead = "false",
                    Message = chatmsg.Message,
                    MessageType = chatmsg.MessageType,
                    ReceiverSK = chatmsg.ReceiverSK,
                    SenderSK = chatmsg.SenderSK,
                    UnqueId = chatmsg.UniqueId,
                    IsLast = "true"
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");             
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(dynamodburl() + "DynamoDbAPIs/putitem?jsonData="+ JsonConvert.SerializeObject(body) + "&tableName=UserDetailsDemo&actionType=register", content);

                return response;
            }
        }
        public static async Task<HttpResponseMessage> GetChatHistory(ChatList req)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {

                    TableName = "UserDetailsDemo",
                    ProjectionExpression = "Message,CreatedOn,ReceiverSK,SenderSK,UniqueId",
                    KeyConditionExpression = "PK = :v_PK",                   
                    FilterExpression = "(SenderSK = :v_SenderSK OR ReceiverSK = :v_ReceiverSK)  AND IsLast = :v_IsLast",
                    ExpressionAttributeValues = new ExpressionAttributeValuesChat()
                    {
                        VPK = new VPK()
                        {
                            S = "Chat_History"
                        },
                        SenderSK = new ChatSenderSK()
                        {
                            S = req.SenderId
                        },
                        ReceiverSK = new ChatReceiverSK()
                        { 
                             S = req.SenderId
                        },
                        IsLast =  new IsLast()
                        { 
                             S = "true"
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(dynamodburl() + "DynamoDbAPIs/getitem", content);

                return response;
            }
        }
    }
}

         