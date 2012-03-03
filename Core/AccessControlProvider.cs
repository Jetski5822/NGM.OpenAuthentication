using System.Security.Cryptography;
using System.Text;

namespace NGM.OpenAuthentication.Core {
    public abstract class AccessControlProvider : IAccessControlProvider {
        public abstract string Name { get; }

        /// <summary>
        /// http://blogs.msdn.com/b/csharpfaq/archive/2006/10/09/how-do-i-calculate-a-md5-hash-from-a-string_3f00_.aspx
        /// </summary>
        public virtual string Hash {
            get {
                MD5 md5Hasher = MD5.Create();
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(Name));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++) {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public override string ToString() {
            return Name;
        }
    }
}