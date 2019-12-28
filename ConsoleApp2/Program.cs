using System;
using System;
using System.Web;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Reflection;

namespace ConsoleApp2
{
    class MyNewClass
    {
        public static void Main()
        {
            MyNewClass myNewClass = new MyNewClass();
            Console.WriteLine("Enter a string having '&', '<', '>' or '\"' in it: ");
            string myString = Console.ReadLine();

            // Encode the string.
            string myEncodedString = HttpUtility.HtmlEncode(myString);
            HttpContext.Current = myNewClass.CreateHttpContextCurrent();
            string serverUrlEncodeString = HttpContext.Current.Server.UrlEncode("https://www.google.com/");

            Console.WriteLine($"HTML Encoded string is: {myEncodedString}");
            Console.WriteLine($"Server Url Encode string is: {serverUrlEncodeString}");
            StringWriter myWriter = new StringWriter();

            // Decode the encoded string.
            HttpUtility.HtmlDecode(myEncodedString, myWriter);
            string myDecodedString = myWriter.ToString();
            Console.Write($"Decoded string of the above encoded string is: {myDecodedString}");
        }

        private HttpContext CreateHttpContextCurrent()
        {
            var httpRequest = new HttpRequest(string.Empty, "http://someurl/", string.Empty);
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);

            var sessionContainer = new HttpSessionStateContainer(
              "id",
              new SessionStateItemCollection(),
              new HttpStaticObjectsCollection(),
              10,
              true,
              HttpCookieMode.AutoDetect,
              SessionStateMode.InProc,
              false);

            httpContext.Items["AspSession"] =
              typeof(HttpSessionState).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                CallingConventions.Standard,
                new[] { typeof(HttpSessionStateContainer) },
                null).Invoke(new object[] { sessionContainer });

            return httpContext;
        }
    }
}
