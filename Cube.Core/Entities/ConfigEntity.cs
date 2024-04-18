using System.ComponentModel.DataAnnotations;

namespace Cube.Core.Entities
{
    public class ConfigEntity
    {
        [Key]
        public int UserId { get; set; }

        public string Config { get; set; }
    }
}
