namespace xofz.GoDaddyUpdater.Framework
{
    using System.Text;

    public class JsonMaker
    {
        public virtual string Make(string ip)
        {
            return new StringBuilder()
                .Append('[').Append('{')
                .Append("\"data\":\"").Append(ip).Append("\",")
                .Append("\"ttl\":3600")
                .Append('}').Append(']')
                .ToString();
        }
    }
}
