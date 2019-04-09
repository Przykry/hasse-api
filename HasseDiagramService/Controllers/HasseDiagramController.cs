using System.Collections.Generic;
using System.Linq;
using HasseDiagramService.Model;
using HasseDiagramService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HasseDiagramService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HasseDiagramController : ControllerBase
    {
        public IHasseDiagramService HasseDiagramService { get; set; }

        public HasseDiagramController(IHasseDiagramService hasseDiagramService)
        {
            HasseDiagramService = hasseDiagramService;
        }

        [HttpPost]
        public ActionResult<HasseDiagramResponse> Post([FromBody] HasseDiagramRequest request)
        {
            var graph = HasseDiagramService
                .Initialize(request)
                .Normalize()
                .CreateRelations()
                .TransitiveReduction()
                .DetermineGraphLayers()
                .CreateGraph();

            return new HasseDiagramResponse
            {
                Graph = graph,
                Table = HasseDiagramService.Table
            };
        }
    }
}