
using PatternClinic.Utils.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PatternClinic.Utils.Repository
{
    public class ResponseManager
    {
    

        public ResponseManager()
        {
        

        }


      
        public class Result
        {
            public int Response { get; set; }
            public string ErrorMessage { get; set; }
            public Result(ResponseCode responseCode = ResponseCode.OK, string errorMessage = null)
            {
                Response = (int)responseCode;
                if (errorMessage == null)
                {
                    errorMessage = (int)responseCode < ErrorMessages.Length ? ErrorMessages[(int)responseCode] : $"Response Code {responseCode}";
                }

                ErrorMessage = errorMessage;
            }

            public Result(Result result)
            {
                Response = result.Response;
                ErrorMessage = result.ErrorMessage;
            }

        }
        public enum CreditDebit
        {
            Credit = 1,
            Debit = 2
        }
        public enum ResponseCode
        {
            InternalServerError = 0,
            OK = 1,
            UnauthorizedAccess = 2,
            TimedOut = 3,
            RecordNotFound = 4,
            NotAllowed = 5,
            AlreadyAssigned = 6,
            WrongLinkCode = 7,
            AlreadyExist = 8,
            WrongPassword = 9,
            Invalid = 10,
            NetworkError = 11,
            WrongCredentials = 12,
            NotSupported = 13,
            NotImplemented = 14,
            LoginTimeout = 15,
            LinkExpired = 16,
            True = 17,
            False = 18,
            WrongOTP = 19,
            ImageNotFound = 20,
            WrongTrackingID = 21,
            AlreadyApplied = 22,
            CardNotFound = 23,
            ErrorFound = 24,
            DuplicateTranscation = 25,
            MissingParameter = 26,
            AcceptTermsAndConditions = 27,
            UserNameAlreadyExist = 28,
            EmailAlreadyExist = 29,
            MobileNumberAlreadyExist = 30,
            ChangedSocialLogin = 31,
            AccountSuspend = 32,
            ApprovelPending=33,
            ApprovelRejected=34
        }
        public static readonly string[] ErrorMessages =
        {
            "Internal Server Error",
            "No Error Found",
            "Unauthorized Access",
            "Timed Out",
            "Record Not Found",
            "Not Allowed",
            "Already Assigned",
            "Wrong LinkCode",
            "Already exist",
            "Old password does not match",
            "Invalid",
            "Network Issue",
            "Wrong Credentials",
            "Not Supported",
            "Not Implemented",
            "Login Timeout",
            "Invalid reference or Your reset password link has been expired",
            "true",
            "false",
            "Wrong OTP",
            "Image Not Found",
            "Wrong Tracking ID",
            "Already Applied",
            "Card not found for payments",
            "Error Found",
            "Duplicate Transcation",
            "Missing Parameter",
            "AcceptTermsAndConditions",
            "UserName Already exist",
            "Email Already exist",
            "Mobile Number Already exist",
            "Sorry you can't Login ,please Register",
            "Your Approvel is Pending from Admin Side",
            "Your Approvel is Rejected from Admin Side",
        };
        public enum AuthenticationType
        {
            Manual,
            Facebook,
            Google,
            Apple

        }
        public enum BookingStatusType
        {
            pending=5,
            accepted=6,
            ontheway=7,
            booked=8,
            cancelled=9,
            rejected=10,
            completed=11,
            process=12,
            Confirm=13
        }

        public enum DriverStatusType
        {
            newRequest=1,
            requestAccepted = 2,
            requestRejected = 3,
            startTour = 4,
            pickup = 5,
            ontheway = 6,
            delivered = 7,
            none=8,
            cancelled = 9,

        }
        public enum UserRoleType
        {
            customer=1,
            agent=2,
            driver=3
        }
        public enum DocumentTypes
        {
            Passport = 1,
            DrivingLicense = 2,
            IdentityCard = 3,
            Selfie =4,
            Residence =5
        }

        #region Dynamodb
        public class DynamoToken
        {
            public string idToken { get; set; }
            public int expiresin { get; set; }
            public string refreshToken { get; set; }
            public string tokenType { get; set; }
        }
        public class APIResult
        {
            public string result { get; set; }
        }

        public class DoctorsResponse
        {
            public string Designation { get; set; }
            public string UserName { get; set; }
            public string ProfileImage { get; set; }
            public string SK { get; set; }
        }

        public class LoginResponse
        {
            public string ProfilePic { get; set; }
            public string UserName { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Country { get; set; }
            public string Height { get; set; }
            public string Weight { get; set; }

            public string SK { get; set; }

            public string Gender { get; set; }
            public string DOB { get; set; }
            public string ReferAs { get; set;  }

    }
        public class UserMaster
        { 
            public string SK { get; set; }
            public string AuthToken { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Weight { get; set; }
            public string Height { get; set; }
            public string ProfilePic { get; set; }
            public string Email { get; set; }
            public string Country { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public string DoctorName { get; set; }
            public string DoctorId { get; set; }
            public string CoachName { get; set; }

            public string GSI1SK { get; set; }
            public string CoachId { get; set; }

            public string ReferAs { get; set; }
            public string Gender { get; set; }
            public string DOB { get; set; }
                   
        }



       

        public class PatientResult
        {
            public string AuthToken { get; set; }
            public LoginResponse patientInfo { get; set; }
            public int Response { get; set; }
            public string ErrorMessage { get; set; }
            public PatientResult(ResponseCode responseCode = ResponseCode.OK, string errorMessage = null)
            {
                Response = (int)responseCode;
                if (errorMessage == null)
                {
                    errorMessage = (int)responseCode < ErrorMessages.Length ? ErrorMessages[(int)responseCode] : $"Response Code {responseCode}";
                }

                ErrorMessage = errorMessage;
            }           
        }


        public class DoctorResult
        {
            public string AuthToken { get; set; }
            public List<DoctorsResponse> doctorInfo { get; set; }
            public int Response { get; set; }
            public string ErrorMessage { get; set; }
            public DoctorResult(ResponseCode responseCode = ResponseCode.OK, string errorMessage = null)
            {
                Response = (int)responseCode;
                if (errorMessage == null)
                {
                    errorMessage = (int)responseCode < ErrorMessages.Length ? ErrorMessages[(int)responseCode] : $"Response Code {responseCode}";
                }

                ErrorMessage = errorMessage;
            }
        }


        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class VPK
        {
            public string S { get; set; }
        }
   
        public class VSK
        {
            public string S { get; set; }
        }

        public class Email
        {
            public string S { get; set; }
        }

        public class ExpressionAttributeValues_GetInfo
        {
            [JsonProperty(":v_PK")]
            public VPK VPK { get; set; }           

            [JsonProperty(":v_email")]
            public Email Email { get; set; }
            
        }

        public class ExpressionAttributeValues_UpdateTeam
        {
            [JsonProperty(":v_DoctorId")]  public DoctorId DoctorId { get; set; }
            [JsonProperty(":v_DoctorName")]  public DoctorName DoctorName { get; set; }
            [JsonProperty(":v_Coach")]  public Coach Coach { get; set; }
            [JsonProperty(":v_GSI1SK")]  public GSI1SK GSI1SK { get; set; }
            [JsonProperty(":v_CoachId")]  public CoachId CoachId { get; set; }
        }

        public class DoctorId  { public string S { get; set; }  }
        public class GSI1SK { public string S { get; set; } }
        public class CoachId { public string S { get; set; } }

        #region Updated Fields
        public class UpdatedEmail
        { 
        public string s { get; set; }
        }
        public class UpdatedFirstName
        {
            public string s { get; set; }
        }
        public class UpdatedLastName
        {
            public string s { get; set; }
        }
        public class UpdatedCountry
        {
            public string s { get; set; }
        }
        public class ExpressionAttributeValues
        {
            [JsonProperty(":v_Height")]
            public UpdatedHeight Height { get; set; }

            [JsonProperty(":v_Weight")]
            public UpdatedWeight Weight { get; set; }

            [JsonProperty(":v_Email")]
            public UpdatedEmail Email { get; set; }

            [JsonProperty(":v_FirstName")]
            public UpdatedFirstName FirstName { get; set; }

            [JsonProperty(":v_LastName")]
            public UpdatedLastName LastName { get; set; }

            [JsonProperty(":v_Country")]
            public UpdatedCountry Country { get; set; }

            [JsonProperty(":v_ProfileImage")]
            public ProfileImage ProfileImage { get; set; }

            [JsonProperty(":v_DOB")]
            public UpdatedDOB DOB { get; set; }
            [JsonProperty(":v_Gender")]
            public UpdatedGender Gender { get; set; }
            [JsonProperty(":v_ReferAs")]
            public ReferAs ReferAs { get; set; }
        }

        public class ReferAs
        { 
        public string S { get; set; }
        }

        public class UpdatedGender
        {
            public string S { get; set; }
        }

        public class UpdatedDOB
        {
            public string S { get; set; }
        }
        public class UpdatedWeight
        { 
         public string s { get; set; }
        }
        public class Key
        {
            public UpdatePK PK { get; set; }
            public UpdateSK SK { get; set; }
        }
        #endregion Updated Fields

        public class ResetPasswordRequest
        { 
        public string newpassword { get; set; }
            public string username { get; set; }
        
            public string AuthToken { get; set; }
            public string confirmationcode { get; set; }
        }
      

        public class FilterExpression
        {
            [JsonProperty(":v_email")]
            public Email Email { get; set; }
        }

        public class Root
        {
            public string TableName { get; set; }
            public string ProjectionExpression { get; set; }
            public string KeyConditionExpression { get; set; }
            public ExpressionAttributeValues_GetInfo ExpressionAttributeValues { get; set; }
        }


        public class UpdateRoot
        {
            public string TableName { get; set; }
            public string ProjectionExpression { get; set; }
            public string KeyConditionExpression { get; set; }
            public ExpressionAttributeValues ExpressionAttributeValues { get; set; }
        }


        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class M
        {
        }

        public class UserId
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public string n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public object s { get; set; }
            public List<object> ss { get; set; }
        }

        public class ActiveStatus
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class DOB
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        //public class SK
        //{
        //    public object b { get; set; }
        //    public bool @bool { get; set; }
        //    public bool isBOOLSet { get; set; }
        //    public List<object> bs { get; set; }
        //    public List<object> l { get; set; }
        //    public bool isLSet { get; set; }
        //    public M m { get; set; }
        //    public bool isMSet { get; set; }
        //    public object n { get; set; }
        //    public List<object> ns { get; set; }
        //    public bool @null { get; set; }
        //    public string s { get; set; }
        //    public List<object> ss { get; set; }
        //}

        public class ContactNo
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }


        public class UpdatePK
        {

            public string s { get; set; }
        }

        public class UpdateSK
        {
            public string s { get; set; }

        }
        public class PK
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class SK
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class UserName
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

      

        public class Coach
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class Street
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class Height
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }


        public class UpdatedHeight
        { 
        public string s { get; set; }
        }
        public class Weight
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class CarecoordinatorName
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class MobilePhone
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class Zip
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class City
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class DoctorName
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class Reading
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class Gender
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class Notes
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class FirstName
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class Lang
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class LastName
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class WorkPhone
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class BMI
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class Diastolic
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class Systolic
        {
            public object b { get; set; }
            public bool @bool { get; set; }
            public bool isBOOLSet { get; set; }
            public List<object> bs { get; set; }
            public List<object> l { get; set; }
            public bool isLSet { get; set; }
            public M m { get; set; }
            public bool isMSet { get; set; }
            public object n { get; set; }
            public List<object> ns { get; set; }
            public bool @null { get; set; }
            public string s { get; set; }
            public List<object> ss { get; set; }
        }

        public class ProfileImage
        { 
        public string s { get; set; }
        }


        public class ExpressionAttributeValues_GetdoctorInfo
        {            
            [JsonProperty(":v_PK")]
            public VPK VPK { get; set; }
        }
        public class UserInfo
        {
            public ProfileImage profileImage { get; set; }
            public UserId UserId { get; set; }
            public ActiveStatus ActiveStatus { get; set; }
            public DOB DOB { get; set; }
            public SK SK { get; set; }
            public ContactNo ContactNo { get; set; }
            public PK PK { get; set; }
            public UserName UserName { get; set; }
            public Email Email { get; set; }
            public Coach Coach { get; set; }
            public Street Street { get; set; }
            public Height Height { get; set; }
            public Weight weight { get; set; }
            public CarecoordinatorName CarecoordinatorName { get; set; }
            public MobilePhone MobilePhone { get; set; }
            public Zip Zip { get; set; }
            public City City { get; set; }
            public DoctorName DoctorName { get; set; }
            public Reading reading { get; set; }
            public Gender Gender { get; set; }
            public Notes Notes { get; set; }
            public FirstName FirstName { get; set; }
            public Lang Lang { get; set; }
            public LastName LastName { get; set; }
            public WorkPhone WorkPhone { get; set; }
            public BMI BMI { get; set; }
            public Diastolic diastolic { get; set; }
            public Systolic systolic { get; set; }

            public ReferAs ReferAs { get; set; }
            public Country country { get; set; }
        }

        public class Country
        {
            public string S { get; set; }

        }

        #endregion
    }
}





