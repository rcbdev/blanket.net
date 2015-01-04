using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blanket
{
    public class Wrapper : DynamicObject
    {
        private string _url;
        private List<string> _urlPath = new List<string>();
        private Dictionary<string, string> _headers = new Dictionary<string, string>();

        public string Url
        {
            get
            {
                return _url + string.Join("/", _urlPath);
            }
        }

        public Dictionary<string, string> Headers
        {
            get
            {
                return _headers;
            }
        }

        private Wrapper(string url)
        {
            if (!url.EndsWith("/"))
            {
                url += "/";
            }
            _url = url;
        }

        private Wrapper(Wrapper old, IEnumerable<string> newPaths)
            : this(old._url)
        {
            _urlPath.AddRange(old._urlPath);
            _urlPath.AddRange(newPaths);
            foreach(var header in old.Headers)
            {
                AddHeader(header.Key, header.Value);
            }
        }

        public static dynamic Wrap(string url, object headers)
        {
            var wrapper = new Wrapper(url);

            wrapper.ExtractHeaders(headers, wrapper.AddHeader);

            return wrapper;
        }

        public static dynamic Wrap(string url)
        {
            return Wrap(url, null);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var newPaths = new List<string>();

            newPaths.Add(binder.Name);
            if(args != null && args.Length == 1 && args[0] is string)
            {
                newPaths.Add(args[0] as string);
            }

            result = new Wrapper(this, newPaths);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = new Wrapper(this, new[] { binder.Name });
            return true;
        }

        private void ExtractHeaders(object headers, Action<string, string> headerAction)
        {
            if (headers != null)
            {
                var props = headers.GetType().GetProperties();
                foreach (var prop in props)
                {
                    headerAction(prop.Name, prop.GetValue(headers, null)?.ToString());
                }
            }
        }

        public void AddHeader(string name, string value)
        {
            Headers.Add(name, value);
        }

        public Task<string> Get()
        {
            return Get(null);
        }

        public Task<string> Get(object headers)
        {
            var client = new HttpClient();

            foreach (var header in Headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            ExtractHeaders(headers, client.DefaultRequestHeaders.Add);

            return client.GetStringAsync(Url);
        }
    }
}
