using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.NetFramework.Controllers
{
    public class ValuesController : ApiController
    {
        private static Dictionary<int, string> _values;
        static ValuesController()
        {
            _values = new Dictionary<int, string>()
            {{1, "Value 1"}, {2, "Value 2"}, };
        }

        // GET api/values
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, _values, Configuration.Formatters.JsonFormatter);
        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {
            if (_values.ContainsKey(id))
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, JsonConvert.SerializeObject(_values[id]), Configuration.Formatters.JsonFormatter);
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
            var largestKey = _values.OrderByDescending(x => x.Key).FirstOrDefault().Key + 1;
            _values[largestKey] = value;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
            _values[id] = value;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            if (!_values.ContainsKey(id))
            {
                return;
            }

            _values[id] = null;
        }
    }
}