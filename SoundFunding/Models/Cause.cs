using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace SoundFunding.Models
{
    public class Cause
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int GoalSum { get; set; }
        public List<string> ContributorIds { get; set; }

        [NotMapped]
        public HttpPostedFileBase PostedPicture { get; set; }
        public string Picture { get; set; }

        [NotMapped]
        public SelectList GoalSums { get; set; }
    }
}