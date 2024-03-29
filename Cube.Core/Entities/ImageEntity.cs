﻿using Cube.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cube.Core.Entities
{
    public class ImageEntity
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public ImageType Type { get; set; }
        public int OwnerId { get; set; }
    }
}
