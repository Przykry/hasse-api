using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HasseDiagramService.Model
{
    public class HasseDiagramRequest
    {
        public HasseTableDto[][] Table { get; set; }
        public bool IsNormalized { get; set; }

        [EnumDataType(typeof(CriterionEstimation))]
        [JsonConverter(typeof(StringEnumConverter))]
        public CriterionEstimation CriterionEstimation {
            get;
            set;
        }
    }

    public enum CriterionEstimation {
        [EnumMember(Value = "Pareto")]
        Pareto,
        [EnumMember(Value = "CriterionSum")]
        CriterionSum,
        [EnumMember(Value = "CriterionWeightSum")]
        CriterionWeightSum 
    }
}