using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Application.Repository.Message.Dto
{
    public class UpdateMessageDto
    {
        public int Id { get; set; }
        public string NewMessage { get; set; }
        public string OldMessage { get; set; }
    }
}
