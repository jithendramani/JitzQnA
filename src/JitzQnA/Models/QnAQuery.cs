using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JitzQnA.Models
{
    public class QnAQuery
    {
        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }
        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }
}
