using Microsoft.AspNetCore.Mvc;

namespace Cube.Application.Services
{
    public class Response<T>
    {
        public T Value { get; set; }
        public IActionResult ActionResult { get; set; }
        public List<string> Messages { get; set; }
    }
}
