using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
                ///<summary>
        ///<para>Add ExtensionMethod to response object</para>
        ///</summary>
        
     public static void AddApplicationError(this HttpResponse response, string message)   
            {
              response.Headers.Add("Application-Error" ,message);
              response.Headers.Add("Access-Control-Expose-Headers" ,"Application-Error");
              response.Headers.Add("Access-Control-Allow-Origin" ,"*");

            }    

            public static int CalculateAge(this DateTime dt)
            {
            var age = DateTime.Today.Year - dt.Year ;

            if(dt.AddYears(age) > DateTime.Today)

            age--;

            return age;
            }
     }
}