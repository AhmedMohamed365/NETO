﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;
        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {

            //TODO: parse the receivedRequest using the \r\n delimeter   
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            // Parse Request line
            // Validate blank line exist
            // Load header lines into HeaderLines dictionary
            // parse request line 
            requestLines = requestString.Split('\n', '\r');
            if (requestLines.Length >= 3 && ValidateBlankLine())
            {
                // request line is more than 3 
                if (!ParseRequestLine())
                    return false;
                if (!LoadHeaderLines())
                    return false;
                return true;
            }
            else
                return false;

        }
        private bool ParseRequestLine()
        {
            string[] requestLine = requestLines[0].Split();

            if (requestLine.Length >= 2 && requestLine.Length < 4)
            {
                // first line contains 
                // method type
                if (requestLine[0].Equals(RequestMethod.HEAD))
                {
                    method = RequestMethod.HEAD;
                }
                if (requestLine[0].Equals(RequestMethod.POST))
                {
                    method = RequestMethod.POST;
                }
                if (requestLine[0].Equals(RequestMethod.GET))
                {
                    method = RequestMethod.GET;
                }
                // URI
                relativeURI = requestLine[1];
                // http virsion
                if (requestLine.Length < 3)
                    httpVersion = HTTPVersion.HTTP09;
                else if (requestLine[2].Equals("HTTP/0.9"))
                    httpVersion = HTTPVersion.HTTP09;
                else if (requestLine[2].Equals("HTTP/1.0"))
                    httpVersion = HTTPVersion.HTTP10;
                else if (requestLine[2].Equals("HTTP/1.1"))
                    httpVersion = HTTPVersion.HTTP11;
                return true;
            }
            else
                return false;
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }
        private bool LoadHeaderLines()
        {
            int i = 0;
            foreach (string iterator in requestLines)
            {
                string[] headerLine = iterator.Split(':',' ');
                if (headerLine.Length == 2) 
                {
                    // ignore 
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    if (iterator.Equals("\n"))
                        break;
                    else
                    {
                        headerLines.Add(headerLine[0], headerLine[1]);
                    }

                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidateBlankLine()
        {
            bool blankLineExist = false;
            foreach (string iterator in requestLines)
            {
                if (iterator.Equals('\n'))
                    blankLineExist = true;
            }
            if (blankLineExist)
                return true;
            else
                return false;
        }

    }
}
