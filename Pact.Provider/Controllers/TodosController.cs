using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pact.Models;

namespace Pact.Provider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodosController : ControllerBase
    {
        private static readonly Todo[] Todos = new Todo[] { new Todo {
                      Id = "1",
                      UserId = "1",
                      Title = "delectus aut autem",
                      Completed = false
                  }};

        private readonly ILogger<TodosController> _logger;

        public TodosController(ILogger<TodosController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Todo> Get()
        {
            return Todos;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var todo = Todos.Where(t => t.Id == id).FirstOrDefault();
            return Ok(todo);
        }
    }
}
