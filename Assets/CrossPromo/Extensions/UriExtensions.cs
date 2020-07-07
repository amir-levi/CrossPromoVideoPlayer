using System;
using System.Collections.Specialized;
using System.Web;

namespace CrossPromo.Extensions
{
    public static class UriExtensions
    {
        public static Uri SetQueryVal(this Uri uri, string name, object value)
        {
            var nvc = HttpUtility.ParseQueryString(uri.Query);
            nvc[name] = (value ?? "").ToString();
            return new UriBuilder(uri) {Query = nvc.ToString()}.Uri;
        }
    }
}