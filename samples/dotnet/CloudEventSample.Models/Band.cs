using System.Collections.Generic;

namespace CloudEventSample.Models
{
    public class Band
    {
        public string Name { get; set; }

        public List<Album> Albums { get; set; }
    }
}
