using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace prj_chamadosBRA.Utils
{
    public class Criptografia : NameValueCollection
    {

        private DateTime _expireTime = DateTime.MaxValue;
        private const string timeStampKey = "__TimeStamp__";
        private const string cryptoKey = "ChangeThis!";
        private readonly byte[] IV = new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };

        public Criptografia() : base() { }

        public Criptografia(string encryptedString)
        {
            deserialize(decrypt(encryptedString));
            if (DateTime.Compare(ExpireTime, DateTime.Now) < 0)
            {
                throw new ExpiredQueryStringException();
            }
        }

        public string EncryptedString
        {
            get { return HttpUtility.UrlEncode(encrypt(serialize())); }
        }

        public DateTime ExpireTime
        {
            get { return _expireTime; }
            set { _expireTime = value; }
        }

        public override string ToString()
        {
            return EncryptedString;
        }

        private string encrypt(string serializedQueryString)
        {
            var buffer = Encoding.ASCII.GetBytes(serializedQueryString);
            using (var des = new TripleDESCryptoServiceProvider())
            {
                using (var MD5 = new MD5CryptoServiceProvider())
                {
                    des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey));
                    des.IV = IV;
                    return Convert.ToBase64String(
                        des.CreateEncryptor().TransformFinalBlock(
                            buffer,
                            0,
                            buffer.Length
                        )
                    );
                }
            }
        }

        private string decrypt(string encryptedQueryString)
        {
            try
            {
                var buffer = Convert.FromBase64String(encryptedQueryString);
                using (var des = new TripleDESCryptoServiceProvider())
                {
                    using (var MD5 = new MD5CryptoServiceProvider())
                    {
                        des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey));
                        des.IV = IV;
                        return Encoding.ASCII.GetString(
                            des.CreateDecryptor().TransformFinalBlock(
                                buffer,
                                0,
                                buffer.Length
                            )
                        );
                    }
                }
            }
            catch (CryptographicException)
            {
                throw new InvalidQueryStringException();
            }
            catch (FormatException)
            {
                throw new InvalidQueryStringException();
            }
        }

        private void deserialize(string decryptedQueryString)
        {
            var nameValuePairs = decryptedQueryString.Split('&');
            for (int i = 0; i < nameValuePairs.Length; i++)
            {
                var nameValue = nameValuePairs[i].Split('=');
                if (nameValue.Length == 2)
                {
                    base.Add(nameValue[0], nameValue[1]);
                }
            }
            if (base[timeStampKey] != null) _expireTime = DateTime.Parse(base[timeStampKey]);
        }

        private string serialize()
        {
            var sb = new StringBuilder();
            foreach (string key in base.AllKeys)
            {
                sb.Append(key);
                sb.Append('=');
                sb.Append(base[key]);
                sb.Append('&');
            }
            sb.Append(timeStampKey);
            sb.Append('=');
            sb.Append(_expireTime);
            return sb.ToString();
        }
    }

    public class InvalidQueryStringException : System.Exception
    {
        public InvalidQueryStringException() : base() { }
    }

    public class ExpiredQueryStringException : System.Exception
    {
        public ExpiredQueryStringException() : base() { }
    }
}
