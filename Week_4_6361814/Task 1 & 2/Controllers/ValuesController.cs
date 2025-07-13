using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Task1_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private static readonly List<string> Values = new List<string> { "value1", "value2" };

        // GET: api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Values;
        }

        // GET: api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (id < 0 || id >= Values.Count)
                return NotFound();
            return Values[id];
        }

        // POST: api/values
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            Values.Add(value);
            return CreatedAtAction(nameof(Get), new { id = Values.Count - 1 }, value);
        }

        // PUT: api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            if (id < 0 || id >= Values.Count)
                return NotFound();
            Values[id] = value;
            return NoContent();
        }

        // DELETE: api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0 || id >= Values.Count)
                return NotFound();
            Values.RemoveAt(id);
            return NoContent();
        }
    }
}
