#nullable disable

namespace MyJira.Models
{
    public class ObjectiveUI
    {
        public int? Parentobjective { get; set; }
        public string Title { get; set; }
        public int? Department { get; set; }
        public DateTime Termbegin { get; set; }
        public DateTime Termend { get; set; }
        public long Estimatedtime { get; set; }
    }
}
