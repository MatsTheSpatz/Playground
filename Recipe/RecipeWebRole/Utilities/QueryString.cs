using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace RecipeWebRole.Utilities
{
    public class QueryString : NameValueCollection
    {
        public QueryString()
        {
        }

        public QueryString(string queryString)
        {
            FillFromString(queryString);
        }

        public static QueryString Current
        {
            get { return new QueryString().FromCurrent(); }
        }

        public string ExtractQuerystring(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (s.Contains("?")) return s.Substring(s.IndexOf("?") + 1);
            }
            return s;
        }

        public QueryString FillFromString(string s)
        {
            base.Clear();
            if (string.IsNullOrEmpty(s)) return this;
            foreach (string keyValuePair in ExtractQuerystring(s).Split('&'))
            {
                if (string.IsNullOrEmpty(keyValuePair)) continue;
                string[] split = keyValuePair.Split('=');
                base.Add(split[0], split.Length == 2 ? split[1] : "");
            }
            return this;
        }


        public QueryString FromCurrent()
        {
            if (HttpContext.Current != null)
            {
                return FillFromString(HttpContext.Current.Request.QueryString.ToString());
            }
            base.Clear();
            return this;
        }

        public new QueryString Add(string name, string value)
        {
            return Add(name, value, false);
        }

        public QueryString Add(string name, string value, bool isUnique)
        {
            string existingValue = base[name];
            if (string.IsNullOrEmpty(existingValue)) base.Add(name, HttpUtility.UrlEncodeUnicode(value));
            else if (isUnique) base[name] = HttpUtility.UrlEncodeUnicode(value);
            else base[name] += "," + HttpUtility.UrlEncodeUnicode(value);
            return this;
        }

        public new QueryString Remove(string name)
        {
            string existingValue = base[name];
            if (!string.IsNullOrEmpty(existingValue)) base.Remove(name);
            return this;
        }

        public QueryString Reset()
        {
            base.Clear();
            return this;
        }

        public new string this[string name]
        {
            get { return HttpUtility.UrlDecode(base[name]); }
        }

        public new string this[int index]
        {
            get { return HttpUtility.UrlDecode(base[index]); }
        }

        public bool Contains(string name)
        {
            string existingValue = base[name];
            return !string.IsNullOrEmpty(existingValue);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (var i = 0; i < base.Keys.Count; i++)
            {
                if (!string.IsNullOrEmpty(base.Keys[i]))
                {
                    foreach (string val in base[base.Keys[i]].Split(','))
                        builder.Append((builder.Length == 0) ? "?" : "&").Append(
                            HttpUtility.UrlEncodeUnicode(base.Keys[i])).Append("=").Append(val);
                }
            }
            return builder.ToString();
        }
    }
}
