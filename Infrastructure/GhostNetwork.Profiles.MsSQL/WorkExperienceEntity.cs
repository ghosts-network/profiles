﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GhostNetwork.Profiles.MsSQL
{
    public class WorkExperienceEntity
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string CompanyName { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        public long? StartWork { get; set; }

        public long? FinishWork { get; set; }

        public Guid ProfileId { get; set; }

        public ProfileEntity Profile { get; set; }
    }
}
