using System.Collections.Generic;

namespace SoundFunding.Models
{
    public class Cause
    {
        public string Name { get; set; }
        public int GoalSum { get; set; }
        public List<ApplicationUser> Contributor { get; set; }
    }
}