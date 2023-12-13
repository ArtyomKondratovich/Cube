using Microsoft.AspNetCore.Mvc;

namespace Cube.Application.Repository
{
    public class Result
    {
        public object? ReturnObject { get; set; }
        public IActionResult ActionResult { get; set; }
        public List<string> Errors { get; set; }
    }
}
