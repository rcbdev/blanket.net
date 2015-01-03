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

        public string Url
        {
            get
            {
                return _url + string.Join("/", _urlPath);
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
        }

        public static dynamic Wrap(string url)
        {
            return new Wrapper(url);
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

        public Task<string> Get()
        {
            var client = new HttpClient();

            return client.GetStringAsync(Url);
        }
    }
}
