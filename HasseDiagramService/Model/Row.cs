using System.Collections.Generic;

namespace HasseDiagramService.Model
{
    public class Row
    {
        public string Variant { get; set; }
        public IEnumerable<Criterion> Criterions { get; set; }
    }
}