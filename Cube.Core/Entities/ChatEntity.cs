using Cube.Core.Entities;
using Cube.Core.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cube.Core.Models
{
    public class ChatEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ChatType Type { get; set; }
        public ICollection<UserEntity> Users { get; set; }
    }
}