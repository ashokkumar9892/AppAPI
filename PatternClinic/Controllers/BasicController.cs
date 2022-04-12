using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatternClinic.Utils.Repository;
using PatternClinic.Utils.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PatternClinic.Utils.Repository.ResponseManager;

namespace PatternClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicController : Controller
    {  
        private readonly ICommonFunction _common;
        private readonly IWebHostEnvironment _webHostEnvironment;      
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BasicController(ICommonFunction common, IWebHostEnvironment hostEnvironment,           
              IHttpContextAccessor httpContextAccessor )
        {          
            _common = common;
            _webHostEnvironment = hostEnvironment;         
            _httpContextAccessor = httpContextAccessor;
        }


        #region Screen 2
        [HttpPost("Login")]
        public PatientResult Login(UserMaster req)
        {
            try
            {
                var accessTokenResult = CommonFunction.GetdynamodbToken(req.UserName, req.Password).Result;
                var kyctoken = CommonFunction.ContentToString(accessTokenResult.Content);
                var json = JsonConvert.DeserializeObject<DynamoToken>(kyctoken);

                req.AuthToken = json.idToken;
                var patientResult = CommonFunction.GetPatientInfo(req).Result;
                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);
                var objResponse = JsonConvert.DeserializeObject<List<PatientInfo>>(patientStringResult);

                return new PatientResult
                {
                    AuthToken = json.idToken,
                    patientInfo = objResponse,
                    Response = (int)ResponseCode.OK,
                    ErrorMessage = "Login Successfully!",
                };
            }
            catch (Exception ex)
            {
                return new PatientResult
                {                   
                    Response = (int)ResponseCode.InternalServerError,
                    ErrorMessage = ex.Message,
                };
            }
        }
        #endregion

        #region Screen 3
        [HttpPost("ForgotPassword")]
        public async Task<Result> ForgotPassword(UserMaster req)
        {
            try
            {
                if (req.UserName == null) return new Result(ResponseCode.ErrorFound, "Enter Email");


                var patientResult = CommonFunction.GetPatientInfo(req).Result;
                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);
                var objResponse = JsonConvert.DeserializeObject<List<PatientInfo>>(patientStringResult);

                if (objResponse.Count == 0) return new Result(ResponseCode.RecordNotFound);


                string PatientName = objResponse.FirstOrDefault().UserName.s.ToString();
                string token = _common.Convert_StringvalueToHexvalue(DateTime.UtcNow.ToString(), System.Text.Encoding.Unicode);
                string Id = _common.Convert_StringvalueToHexvalue(objResponse.FirstOrDefault().PK.s.ToString(), System.Text.Encoding.Unicode);
                var subject = "Pattern Clinic APP Provide you reset password Link";
                var request = _httpContextAccessor.HttpContext.Request;
                var _baseURL = $"{request.Scheme}://{request.Host}";
                string PathByPage = _baseURL + "/ResetPassword.html?token=" + token + "&Id=" + Id;
                var callback = "<a href='" + PathByPage + "'>Reset Password</a>";
                string callbackUrl = "<b>Please find the Password Reset Link. </b><br/>" + callback;
                var body = @"<html>
                      <body><p>Dear "+ PatientName + @"</p>
                           <p>" + callbackUrl + @"</p><br/>
                           <p>Thanks</p>
                           <p><strong>Pattern Clinic APP</strong></p></body>
                      </html>";
                var response = await EmailManager.SendEmailSendGridAsync(PatientName, subject, body, req.UserName);
                if (response == "OK")
                {
                return new Result
                    {
                        Response = (int)ResponseCode.OK,
                        ErrorMessage = "Mail Sent Successfull",
                    };
                }
                else
                {
                    return new Result
                    {
                        Response = (int)ResponseCode.ErrorFound,
                        ErrorMessage = "Mail failed",
                    };
                }
            }
            catch (Exception ex)
            {               
                return new Result(ResponseCode.InternalServerError, ex.Message);
            }
        }
        #endregion 

        #region Screen 4
        [HttpPost("ResetPassword")]
        public string ResetPassword(string Password, string id, string token)
        {
            try
            {               
                string tokenSol = _common.Convert_HexvalueToStringvalue(token, System.Text.Encoding.Unicode);
                string userId = _common.Convert_HexvalueToStringvalue(id, System.Text.Encoding.Unicode);

                UserMaster req = new UserMaster();
                req.UserName = id;
                req.Password = token;

                var accessTokenResult = CommonFunction.GetdynamodbToken(userId, tokenSol).Result;
                var kyctoken = CommonFunction.ContentToString(accessTokenResult.Content);
                var json = JsonConvert.DeserializeObject<DynamoToken>(kyctoken);

                var patientResult = CommonFunction.GetPatientInfo(req).Result;
                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);

                var objResponse = JsonConvert.DeserializeObject<List<PatientInfo>>(patientStringResult);


                DateTime dateTime = Convert.ToDateTime(tokenSol);
                if ((DateTime.UtcNow - dateTime).TotalMinutes < 5)
                {
                    //user.Password = _common.GeneratePasswordSaltedHashString(Password);
                    //_db.SaveChanges();
                    return "Your Password changed successfully!!";
                }
                else
                {
                    return "Link Expired or Not valid, Please Try with New verification Link!!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Screen 6, 7
        [HttpPost("UpdateProfile")]
        public Task<PatientResult> UpdateProfile(UserMaster req)
        {
            try
            {
                var patientResult = CommonFunction.UpdatePatientInfo(req).Result;
                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);
                var objResponse = JsonConvert.DeserializeObject<List<PatientInfo>>(patientStringResult);

                return Task.FromResult(new PatientResult
                {
                    AuthToken = req.AuthToken,
                    patientInfo = objResponse,
                    Response = (int)ResponseCode.OK,
                    ErrorMessage = "Updated Successfully!",
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new PatientResult
                {
                    Response = (int)ResponseCode.InternalServerError,
                    ErrorMessage = ex.Message,
                });
            }       
        }
        #endregion
         
        #region Test
        [HttpPost("Test")]
        public string Test()
        {
            var accessTokenResult = CommonFunction.GetdynamodbToken("ashokkumar9892@hotmail.com", "Test@123").Result;

            var kyctoken = CommonFunction.ContentToString(accessTokenResult.Content);
            var json = JsonConvert.DeserializeObject<DynamoToken>(kyctoken);

            return "";
        }
        #endregion Test
    }
}
