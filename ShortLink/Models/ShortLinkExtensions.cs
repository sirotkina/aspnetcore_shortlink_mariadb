using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShortLink.Models
{
    public static class ShortLinkExtensions
    {
        public static string GetShortUrl(string longUrl)
        {
            StringBuilder builder = new StringBuilder();

            Regex regex = new Regex(@"https*:\/\/", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection protocol = regex.Matches(longUrl);
            builder.Append(protocol[0].Value)
                    .Append("sl.co/")
                    .Append(GetHash(longUrl.Substring(protocol[0].Length, longUrl.Length - protocol[0].Length)));

            return builder.ToString();
        }

        static string GetHash(string link)
        {

            byte[] tmpLink;
            byte[] tmpHash;

            tmpLink = ASCIIEncoding.ASCII.GetBytes(link);
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpLink);

            return ByteArrayToString(tmpHash).ToLower().Substring(0, 6);
        }

        static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public static bool CheckLink(string link)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadString(link);

            }
            catch (WebException ex)
            {
                //HttpWebResponse response = ex.Response != null ? ex.Response as HttpWebResponse : null;
                //if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                //{
                //}
                return false;
            }
            return true;
        }
    }
}
