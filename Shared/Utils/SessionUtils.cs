using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public static class SessionUtils
    {
        public static Dictionary<string, object> NormalizeSessionData(Hashtable sessionData)
        {
            var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (DictionaryEntry entry in sessionData)
            {
                var key = entry.Key?.ToString();
                if (!string.IsNullOrWhiteSpace(key))
                {
                    result[key] = entry.Value;
                }
            }

            return result;
        }

        public static bool VerifyPassword(string authDataStr, string password)
        {
            var serializer = new PHPSerializer();
            var result = serializer.Deserialize(authDataStr);

            if (result is not Hashtable authData || !authData.ContainsKey("hash"))
                return false;

            var storedHash = authData["hash"].ToString()?.Replace("$2y$", "$2a$");
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        public static byte[] GenerateSessionId()
        {
            var sessionIdBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(sessionIdBytes);
            return sessionIdBytes;
        }

        public static Hashtable CreateSessionData(
     int userId,
     string username,
     int userGroupId,
     string userGroupName,
     string clientIp,
     long nowUnix)
        {
            var ipAddress = string.IsNullOrEmpty(clientIp) ? "0.0.0.0" : clientIp;
            var ipBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
            var ipPackedString = Convert.ToBase64String(ipBytes); // SAFER than ISO-8859-1

            return new Hashtable
            {
                ["_ip"] = ipPackedString,
                ["userId"] = userId,
                ["username"] = username,
                ["userGroupId"] = userGroupId,
                ["userGroupName"] = userGroupName,
                ["passwordDate"] = (double)nowUnix,
                ["dismissedNotices"] = new Hashtable(),
                ["lastNoticeUpdate"] = (double)nowUnix,
                ["promotionChecked"] = true,
                ["trophyChecked"] = true,
                ["previousActivity"] = (double)nowUnix
            };
        }

    }
}
