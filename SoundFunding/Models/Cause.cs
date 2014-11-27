using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoundFunding.Models
{
    public class Cause
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int GoalSum { get; set; }
        public List<string> ContributorIds { get; set; }
    }
}