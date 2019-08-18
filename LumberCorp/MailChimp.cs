using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using System.Net;
using System.IO;

namespace LumberCorp
{
    public class MailChimp
    {
        public enum MergeVarFieldType { text, number, radio, dropdown, date, address, phone, url, imageurl, zip, birthday };

        private const string endPoint = "https://us7.api.mailchimp.com/2.0/";

        // http://us2.api.mailchimp.com/2.0/
        private const string apiKey = "98c45be6d12e9a99f5515a9a2d7e9107-us7";
        private const string listId = "797356e495";

        public static void UpdateMembers()
        {
            string parameters = "apikey=" + apiKey + "&id=" + listId + "&status=subscribed";

            XmlDocument xmlDocument = Invoke("lists", "members", parameters);

        }

        public static bool ListUpdateMember(string email, string tag, DateTime? value)
        {
            HttpServerUtility server = HttpContext.Current.Server;

            string parameters = "apikey=" + apiKey + "&id=" + listId;
            parameters += "&email[email]=" + email;
            if (value.HasValue)
            {
                parameters += "&merge_vars[" + tag + "]=" + server.UrlEncode(value.Value.ToString("yyyy-MM-dd"));
            }
            else
                parameters += "&merge_vars[" + tag + "]=";

            XmlDocument xmlDocument = Invoke("lists", "update-member", parameters);
            XmlNodeList ErrorList = xmlDocument.SelectNodes("MCAPI/error");
            if (ErrorList.Count == 0)
                return true;
            else
            {
                XmlNode error = ErrorList[0];
                return false;
            }
        }

        // merge-var-update(string apikey, string id, string tag, struct options);
        public static bool ListMergeVarUpdate(string tag, string name)
        {
            string parameters = "apikey=" + apiKey + "&id=" + listId;
            parameters += "&tag=" + tag;
            parameters += "&options[name]=" + name;

            XmlDocument xmlDocument = Invoke("lists", "merge-var-update", parameters);
            XmlNodeList ErrorList = xmlDocument.SelectNodes("MCAPI/error");
            if (ErrorList.Count == 0)
                return true;
            else
            {
                XmlNode error = ErrorList[0];
                return false;
            }
        }

        // MailChimp.ListMergeVarSet("CONTACT" + contactId, userContact.Response);
        public static void ListMergeVarSet(string tag, DateTime? value)
        {
            string parameters = "apikey=" + apiKey + "&id=" + listId;
            parameters += "&tag=" + tag;
            if (value.HasValue)
                parameters += "&value=" + value.Value.ToString("dd/MM/yyyy");
            else
                parameters += "&value=";

            XmlDocument xmlDocument = Invoke("lists", "merge-var-set", parameters);
        }

        // listMergeVarAdd(string apikey, string id, string tag, string name, array options)
        public static void ListMergeVarAdd(string tag, string name, MergeVarFieldType type, bool required, bool pub, bool show)
        {
            string parameters = "apikey=" + apiKey + "&id=" + listId;
            parameters += "&tag=" + tag;
            parameters += "&name=" + name;
            parameters += "&options[field_type]=" + type;
            parameters += "&options[req]=" + (required ? "true" : "false");
            parameters += "&option[public]=" + (pub ? "true" : "false");
            parameters += "&options[show]=" + (show ? "true" : "false");

            if (type == MergeVarFieldType.date)
                parameters += "&options[dateformat]=DD/MM/YYYY";
            if (type == MergeVarFieldType.birthday)
                parameters += "&options[dateformat]=DD/MM";
            if (type == MergeVarFieldType.phone)
                parameters += "&options[phoneformat]=NZ";

            XmlDocument xmlDocument = Invoke("lists", "merge-var-add", parameters);
        }

        public static void ListSubscribe(string email_address, string first, string last)
        {
            HttpServerUtility server = HttpContext.Current.Server;


            string parameters = "apikey=" + apiKey + "&id=" + listId + "&double_optin=false&update_existing=false&replace_interests=false&send_welcome=true";
            parameters += "&email[email]=" + server.UrlEncode(email_address);
            parameters += "&merge_vars[FNAME]=" + server.UrlEncode(first);
            parameters += "&merge_vars[LNAME]=" + server.UrlEncode(last);

            XmlDocument xmlDocument = Invoke("lists", "subscribe", parameters);
        }



        //TODO: Would be better to create an API call method and build up the url as required to cater for all calls and HTTP verbs
        //Need to be careful that we arent sending too many failed authentication retries due to account key mismatches as there may be a lockout policy, this could easily go unnoticed during ajax calls
        private static string BuildUrl(string area, string method, string parameters)
        {
            string template = endPoint + "{0}/{1}.xml";
            if (!String.IsNullOrEmpty(parameters))
                template += "?{2}";

            //TODO: Retrieve these settings from the Organization table for the current Organization

            return string.Format(template, area, method, parameters);
        }

        private static string BuildUrl(string area, string method)
        {
            return BuildUrl(area, method, null);
        }


        private static XmlDocument Invoke(string area, string method)
        {
            return Invoke(area, method, null);
        }

        private static XmlDocument Invoke(string area, string method, string parameters)
        {
            return Invoke(string.Empty, area, method, parameters);
        }

        private static XmlDocument Invoke(string xml, string area, string method, string parameters)
        {
            string url = BuildUrl(area, method, parameters);

            XmlDocument responseXml = GetXml(xml, url);
            return responseXml;
        }

        public static XmlDocument GetXml(string xml, string url)
        {
            XmlDocument result = null;
            // convert the xml into a byte array
            byte[] data = null;

            if (!String.IsNullOrEmpty(xml))
                data = System.Text.Encoding.UTF8.GetBytes(xml);

            // set up the request
            StringBuilder responseText = new StringBuilder();
            try
            {
                // create and initialize the web request 
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = "Petpack";
                request.KeepAlive = false;
                request.Method = "get";
                request.ContentType = "application/x-www-form-urlencoded";
                // set the timeout to 120 seconds
                request.Timeout = 120 * 1000;
                // set the content length in the request headers 
                if (data != null)
                {
                    request.ContentLength = data.Length;

                    // write the data to be imported into the request stream
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(data, 0, data.Length);
                    }
                }

                // get the response
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (request.HaveResponse == true && response != null)
                    {
                        // response should be 200 (OK) however certain errors such as validation errors
                        // will come through in the response so it must be read and parsed accordingly

                        // get the response stream and read it
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            responseText = new StringBuilder(reader.ReadToEnd());
                        }
                    }
                }
            }
            catch (WebException wex)
            {
                // this exception will be raised if the server didn't return 200 (OK)
                // some requests cause the import API to return status codes such as 401 and 500
                // it is important to retrieve more information about any particular errors
                if (wex.Response != null)
                {
                    // get the error response
                    using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
                    {

                        // get the error response stream and read it
                        using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            responseText = new StringBuilder(reader.ReadToEnd());
                            // responseText = new StringBuilder();
                        }
                    }
                    responseText = new StringBuilder("<MCAPI><error>" + wex.Message + "</error></MCAPI>");
                }
                else
                {
                    throw; // HTTP exception
                }
            }

            try
            {
                // load the response text into an XML document
                // all responses from the import API will return an xml document
                result = new XmlDocument();
                string xmlString = responseText.ToString();
                if (xmlString[0] == '{' && xmlString[xmlString.Length - 1] == '}')
                {
                    xmlString = xmlString.Substring(1, xmlString.Length - 2);
                }
                while (xmlString[0] == '\n')
                    xmlString = xmlString.Substring(1, xmlString.Length - 1);
                result.LoadXml(xmlString);
            }
            catch (XmlException xmlException)
            {
                // response text was not a valid XML document which should not happen
                throw xmlException;
            }

            return result;
        }
    }
}