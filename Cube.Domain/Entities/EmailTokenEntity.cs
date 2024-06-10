namespace Cube.Domain.Entities
{
    public class EmailTokenEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Token { get; set; }
    }
}
