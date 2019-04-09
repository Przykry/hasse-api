using System.Collections.Generic;

namespace HasseDiagramService.Model
{
    public class Graph
    {
        public IList<Edge> Edges { get; set; }
        public IList<Node> Nodes { get; set; }
    }
}