using System;
using System.Collections.Generic;
using System.Linq;
using HasseDiagramService.Model;
using HasseDiagramService.Services.Interfaces;

namespace HasseDiagramService.Services
{
    public class HasseDiagramService : IHasseDiagramService
    {
        private List<Edge> _relations;
        private List<Node> _nodes;
        private CriterionEstimation _isCriterionEstimation;
        public HasseTableDto[][] Table { get; set; }

        public IHasseDiagramService Initialize(HasseDiagramRequest request)
        {
            Table = request.Table;
            _isCriterionEstimation = request.CriterionEstimation;
            return this;
        }

        public IHasseDiagramService CreateRelations()
        {
            _relations = new List<Edge>();
            _nodes = Table.SelectMany(x => x)
                .GroupBy(x => x.Variant)
                .Select(variant =>
                {
                    return new Node
                    {
                        Id = variant.Key,
                        Label = variant.Key,
                        CriterionSum = variant.Sum(x => x.NormalizedValue * (
                                                            _isCriterionEstimation == CriterionEstimation.CriterionWeightSum
                                                                ? x.Weight
                                                                : 1))
                    };
                }).ToList();

            if (_isCriterionEstimation == CriterionEstimation.Pareto)
                CriterionPareto();
            else
                CriterionSum();

            return this;
        }

        private void CriterionPareto()
        {
            foreach (var current in Table)
            {
                foreach (var iterator in Table)
                {
                    var relation = true;
                    var hasGreater = false;
                    for (var j = 0; j < current.Length; j++)
                    {

                        if ((current[j].CurrentValue > iterator[j].CurrentValue))
                            hasGreater = true;
                        if (current[j].CurrentValue >= iterator[j].CurrentValue)
                            continue;
                        relation = false;
                        break;
                    }

                    if (relation && hasGreater)
                        _relations.Add(new Edge { From = iterator[0].Variant, To = current[0].Variant });
                }
            }
        }

        private void CriterionSum()
        {
            foreach (var current in _nodes)
                foreach (var iterator in _nodes)
                    if (current.CriterionSum > iterator.CriterionSum)
                        _relations.Add(new Edge { From = iterator.Id, To = current.Id });
        }

        public IHasseDiagramService TransitiveReduction()
        {
            foreach (var x in _nodes)
                foreach (var y in _nodes)
                    foreach (var z in _nodes)
                        if (!(x.Id == y.Id && y.Id == z.Id) && !(x.Id == y.Id && x.Id == z.Id))
                            if (_relations.Any(edge => edge.From == x.Id && edge.To == y.Id) &&
                                _relations.Any(edge => edge.From == y.Id && edge.To == z.Id))
                                _relations.RemoveAll(edge => edge.From == x.Id && edge.To == z.Id);
            return this;
        }

        public IHasseDiagramService DetermineGraphLayers()
        {
            var layeredNodes = new List<Node>();
            var layeredEdges = _relations.ToList();
            var layer = 0;

            while (layeredEdges.Any() || layeredNodes.Count != _nodes.Count)
            {
                foreach (var x in _nodes)
                {
                    if ((layeredEdges.All(y => x.Id != y.To) || !layeredEdges.Any()) && layeredNodes.All(node => node.Id != x.Id))
                    {
                        x.Level = layer;
                        layeredNodes.Add(x);
                    }
                }
                layeredNodes.ForEach(toRemove => layeredEdges.RemoveAll(edge => edge.From == toRemove.Id));

                layer++;
            }
            return this;
        }

        public IHasseDiagramService Normalize()
        {
            foreach (var el in Table.SelectMany(x => x).GroupBy(x => x.Criterion))
            {
                var max = el.Max(x => x.Value);
                var min = el.Min(x => x.Value);

                foreach (var cell in el)
                {
                    cell.IsNormalized = cell.IsNormalized;
                    if (max != min)
                    {
                        Table[cell.I][cell.J].NormalizedValue = cell.IsMax
                            ? Math.Round(1 - ((max - cell.Value) / (max - min)), 2)
                            : Math.Round(((max - cell.Value) / (max - min)), 2);
                    }
                    else
                    {
                        Table[cell.I][cell.J].NormalizedValue = 1;
                    }
                }
            }

            return this;
        }

        public Graph CreateGraph()
        {
            return new Graph { Edges = _relations, Nodes = _nodes };
        }
    }
}