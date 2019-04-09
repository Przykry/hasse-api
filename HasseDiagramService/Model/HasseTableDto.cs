namespace HasseDiagramService.Model
{
    public class HasseTableDto
    {
        public string Criterion { get; set; }
        public string Variant { get; set; }
        public decimal Value { get; set; }
        public decimal NormalizedValue { get; set; }
        public bool IsNormalized { get; set; }
        public bool IsMax{ get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public decimal CurrentValue => IsNormalized 
            ? NormalizedValue 
            : IsMax 
                ? Value
                : -Value;

        public decimal Weight { get; set; }
    }
}