using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Net6.Controllers
{
    [Route("/api/values")]
    public class ValuesController : ControllerBase
    {
        private static Dictionary<int, string> _values;
        static ValuesController()
        {
            _values = new Dictionary<int, string>()
            {{1, "Value 1"}, {2, "Value 2"}, };
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok(_values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (_values.ContainsKey(id))
            {
                return this.Ok(_values[id]);
            }
            else
            {
                return this.NotFound();
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var largestKey = _values.OrderByDescending(x => x.Key).FirstOrDefault().Key + 1;
            _values[largestKey] = value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            _values[id] = value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
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