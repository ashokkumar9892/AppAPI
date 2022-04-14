using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PatternClinic.Utils.Repository;
using PatternClinic.Utils.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                if(kyctoken == "Incorrect username or password.") return new PatientResult(ResponseCode.ErrorFound, "No User Found");


                var json = JsonConvert.DeserializeObject<DynamoToken>(kyctoken);

                req.AuthToken = json.idToken;
                var patientResult = CommonFunction.GetPatientInfo(req).Result;
                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);
                var objResponse = JsonConvert.DeserializeObject<List<UserInfo>>(patientStringResult);
                if (objResponse.Count == 0) return new PatientResult(ResponseCode.ErrorFound, "No User Found");

                LoginResponse response = new LoginResponse();

                response.Country = objResponse.FirstOrDefault().country != null ? objResponse.FirstOrDefault().country.S : null;
                response.Email = objResponse.FirstOrDefault().Email.S;
                response.FirstName = objResponse.FirstOrDefault().FirstName != null ? objResponse.FirstOrDefault().FirstName.s : null;
                response.Height = objResponse.FirstOrDefault().Height != null ? objResponse.FirstOrDefault().Height.s: null;
                response.LastName = objResponse.FirstOrDefault().LastName != null ? objResponse.FirstOrDefault().LastName.s : null;
                response.Weight = objResponse.FirstOrDefault().weight != null ? objResponse.FirstOrDefault().weight.s : null;
                response.UserName = objResponse.FirstOrDefault().UserName != null ? objResponse.FirstOrDefault().UserName.s : null;
                response.ProfilePic = objResponse.FirstOrDefault().profileImage != null ? objResponse.FirstOrDefault().profileImage.s : null;
                response.SK = objResponse.FirstOrDefault().SK != null ? objResponse.FirstOrDefault().SK.s : null;
                return new PatientResult
                {
                    AuthToken = json.idToken,
                    patientInfo = response,
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
        public Result ForgotPassword(UserMaster req)
        {
            try
            {
               if(req.UserName == null) return new Result(ResponseCode.ErrorFound, "No UserName Found");

                var patientResult = CommonFunction.ForgotPatientPassword(req).Result;
                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);


                dynamic json = JsonConvert.DeserializeObject(patientStringResult);
                if(json != "")
                { 
                return new Result
                    {
                        Response = (int)ResponseCode.OK,
                        ErrorMessage = json,
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
        public Result ResetPassword(ResetPasswordRequest req)
        {
            try
            {
                if (req.username == null) return new Result(ResponseCode.ErrorFound, "No UserName Found");

                var patientResult = CommonFunction.ResetPatientPassword(req).Result;
                if(patientResult.StatusCode == HttpStatusCode.Unauthorized) return new Result(ResponseCode.UnauthorizedAccess);

                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);
                dynamic json = JsonConvert.DeserializeObject(patientStringResult);

                if (json != "")
                {
                    return new Result
                    {
                        Response = (int)ResponseCode.OK,
                        ErrorMessage = json,
                    };
                }
                else
                {
                    return new Result
                    {
                        Response = (int)ResponseCode.ErrorFound,
                        ErrorMessage = "Error in Reset Password",
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result(ResponseCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Screen 6, 7
        [HttpPost("UpdateProfile")]
        public PatientResult UpdateProfile(UserMaster req)
        {
            try
            {
               
                var patientResult = CommonFunction.UpdatePatientInfo(req).Result;

                if (patientResult.StatusCode.Equals(HttpStatusCode.Unauthorized)) return  (new PatientResult
                {
                    Response = (int)ResponseCode.UnauthorizedAccess,
                });

                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);                
                dynamic json = JsonConvert.DeserializeObject(patientStringResult);
                if (json == "Updated")
                {
                    return (new PatientResult
                    {
                        AuthToken = req.AuthToken,
                        Response = (int)ResponseCode.OK,
                        ErrorMessage = "Updated Successfully!",
                    });
                }
                else {
                    return (new PatientResult
                    {                      
                        Response = (int)ResponseCode.ErrorFound,
                        ErrorMessage = "Error Found!",
                    });
                }
            }
            catch (Exception ex)
            {
                return (new PatientResult
                {
                    Response = (int)ResponseCode.InternalServerError,
                    ErrorMessage = ex.Message,
                });
            }       
        }
        #endregion

        #region Screen 10
        [HttpPost("DoctorsList")]
        public DoctorResult DoctorsList(UserMaster req)
        {
            try
            {
                var patientResult = CommonFunction.GetDoctorsList(req).Result;
                if (patientResult.StatusCode == HttpStatusCode.Unauthorized) return new DoctorResult(ResponseCode.UnauthorizedAccess);

                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);               
                var objResponse = JsonConvert.DeserializeObject<List<UserInfo>>(patientStringResult);

                List<DoctorsResponse> responselist = new List<DoctorsResponse>();
                foreach (var x in objResponse)
                {
                    DoctorsResponse response = new DoctorsResponse();
                    response.ProfileImage = x.profileImage != null ? x.profileImage.s : null;
                    response.SK = x.SK != null ? x.SK.s : null;
                    response.UserName = x.UserName != null ? x.UserName.s : null;
                    responselist.Add(response);
                }
                
                    return new DoctorResult
                    {
                        Response = (int)ResponseCode.OK,
                        doctorInfo = responselist
                    };
               
            }
            catch (Exception ex)
            {
                return new DoctorResult(ResponseCode.InternalServerError, ex.Message);
            }
        }
        #endregion


        #region Screen 11
        [HttpPost("CoachList")]
        public DoctorResult CoachList(UserMaster req)
        {
            try
            {
                var patientResult = CommonFunction.GetCoachList(req).Result;
                if (patientResult.StatusCode == HttpStatusCode.Unauthorized) return new DoctorResult(ResponseCode.UnauthorizedAccess);

                var patientStringResult = CommonFunction.ContentToString(patientResult.Content);
                var objResponse = JsonConvert.DeserializeObject<List<UserInfo>>(patientStringResult);

                List<DoctorsResponse> responselist = new List<DoctorsResponse>();
                foreach (var x in objResponse)
                {
                    DoctorsResponse response = new DoctorsResponse();
                    response.ProfileImage = x.profileImage != null ? x.profileImage.s : null;
                    response.SK = x.SK != null ? x.SK.s : null;
                    response.UserName = x.UserName != null ? x.UserName.s : null;
                    responselist.Add(response);
                }

                return new DoctorResult
                {
                    Response = (int)ResponseCode.OK,
                    doctorInfo = responselist
                };

            }
            catch (Exception ex)
            {
                return new DoctorResult(ResponseCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region RefreshToken
        [HttpPost("RefreshToken")]
        public PatientResult RefreshToken(UserMaster req)
        {
            try
            {
                var accessTokenResult = CommonFunction.GetdynamodbToken(req.UserName, req.Password).Result;

                var kyctoken = CommonFunction.ContentToString(accessTokenResult.Content);
                if (kyctoken == "Incorrect username or password.") return new PatientResult(ResponseCode.ErrorFound, "No User Found");

                var json = JsonConvert.DeserializeObject<DynamoToken>(kyctoken);
                return new PatientResult
                {
                    AuthToken = json.idToken,               
                    Response = (int)ResponseCode.OK                 
                };
            }
            catch(Exception ex)
            {
                return new PatientResult
                {
                    Response = (int)ResponseCode.InternalServerError,
                    ErrorMessage = ex.Message,
                };
            }
        }
        #endregion Refresh Token

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
