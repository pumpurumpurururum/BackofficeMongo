using System;
using System.Linq;
using Helper.Model;

namespace Helper
{
    public static class TokenHelper
    {
        public static string CreateToken(DateTime time, Guid key)
        {
            byte[] timeb = BitConverter.GetBytes(time.ToBinary());
            byte[] keyb = key.ToByteArray();
            var token = Convert.ToBase64String(timeb.Concat(keyb).ToArray());
            return System.Web.HttpUtility.HtmlEncode(token);
        }


        public static TokenObject FromToken(string token)
        {
            try
            {
                byte[] data = Convert.FromBase64String(token);
                byte[] gb = data.Skip(8).ToArray();
                var ret = new TokenObject
                {
                    Time = DateTime.FromBinary(BitConverter.ToInt64(data, 0)),
                    Key = new Guid(gb)
                };
                return ret;

            }
            catch
            {
                return null;
            }


        }
    }
}
