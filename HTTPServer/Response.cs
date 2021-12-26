using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        static  string CRLF = "\r\n";
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {

            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            
            this.code = code;
            headerLines.Add(GetStatusLine(code)+CRLF);
            headerLines.Add("Content-Type: "+ contentType+ CRLF  );
            headerLines.Add("Content-Length: "+ content.Length.ToString() + CRLF);
            headerLines.Add("Date: "+ DateTime.Now.ToString()+ CRLF) ;
           
            

            if(redirectoinPath != "")
            {
                headerLines.Add ( "redirection: " + redirectoinPath + CRLF);
            }

            headerLines.Add(CRLF);
            // TODO: Create the request string
            foreach (string header in headerLines)
            {
                responseString += header;
            }


            responseString += content;

        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Format("HTTP/1.1 {0} {1}",((int)code).ToString(), "OK");
            
                        
         
            return statusLine;
        }
    }
}

