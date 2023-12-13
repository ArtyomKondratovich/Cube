using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Application.Repository.Message.Dto
{
    public class NewMessageDto
    {
        public int SenderId { get; set; }
        public int RecieverId { get; set; }
        public string Message { get; set; }
    }
}
