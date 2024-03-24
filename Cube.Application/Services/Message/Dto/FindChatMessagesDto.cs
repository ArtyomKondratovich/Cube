using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Application.Services.Message.Dto
{
    public class FindChatMessagesDto
    {
        public int Id { get; set; }
        public int UsersTimezoneOffset { get; set; }
    }
}
