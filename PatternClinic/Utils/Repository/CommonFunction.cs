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

                var requestBody = new HttpRequestMessage(HttpMethod.Post, "https://rpmcrudapis20210808220332demo.azurewebsites.net/api/signin")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
                };
                var response = await SendPost("https://rpmcrudapis20210808220332demo.azurewebsites.net/api/signin", requestBody);

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
                    ExpressionAttributeValues = new ExpressionAttributeValues()
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
                var response = await client.PostAsync("https://rpmcrudapis20210808220332demo.azurewebsites.net/api/DynamoDbAPIs/getitem", content);
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
                    ProjectionExpression = "PK,SK,UserId,UserName,Email,ContactNo,DOB,DoctorName,CarecoordinatorName,Coach,Height,reading,diastolic,systolic,weight,BMI,FirstName,LastName,Gender,Lang,Street,City,Zip,WorkPhone,MobilePhone,ActiveStatus, Notes",
                    KeyConditionExpression = "PK = :v_PK",
                    FilterExpression = "Email = :v_email",
                    ExpressionAttributeValues = new ExpressionAttributeValues()
                    {
                        VPK = new VPK()
                        {
                            S = "patient"
                        },                        
                         Email = new Email()
                         {
                             S = "jaypal@mailinator.com"
                         }
                    }

                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync("https://rpmcrudapis20210808220332demo.azurewebsites.net/api/DynamoDbAPIs/getitem", content);

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
                        PK = new PK { s = "patient" },
                        SK = new SK { s = req.SK }
                    },                 
                    UpdateExpression = "SET Height = :v_Height",
                    ExpressionAttributeValues = new ExpressionAttributeValues()
                    {
                        Height = new Height()
                        {
                            s = req.Height
                        }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", req.AuthToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync("https://appapi.apatternplus.com/api/DynamoDbAPIs/updateitem", content);

                return response;
            }
        }
    }
}

         