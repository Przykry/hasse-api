using System.Collections.Generic;
using HasseDiagramService.Model;

namespace HasseDiagramService.Services.Interfaces
{
    public interface IHasseDiagramService
    {
        HasseTableDto[][] Table { get; set; }
        IHasseDiagramService CreateRelations();
        IHasseDiagramService TransitiveReduction();
        IHasseDiagramService DetermineGraphLayers();
        IHasseDiagramService Normalize();
        IHasseDiagramService Initialize(HasseDiagramRequest request);
        Graph CreateGraph();
    }
}