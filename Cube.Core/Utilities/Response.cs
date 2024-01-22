namespace Cube.Core.Utilities
{
    public class Response<T>
    {
        public T? Object { get; set; }
        public ResponceType ResponseType { get; set; }
        public int StatusCode { get; set; }
    }
}
