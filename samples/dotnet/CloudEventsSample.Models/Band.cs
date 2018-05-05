using System;
using System.Collections.Generic;

namespace CloudEventsSample.Models
{
    public class Band
    {
        public string Name { get; set; }

        public List<Album> Albums { get; set; }
    }
}
