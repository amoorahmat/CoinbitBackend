﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinbitBackend.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string firstName { get; set; }

        [Required]
        [StringLength(100)]
        public string lastName { get; set; }

        public bool isActive { get; set; }

        [StringLength(100)]
        public string fatherName { get; set; }

        public bool? gender { get; set; }

        public DateTime? birthDate { get; set; }

        [StringLength(50)]
        public string nationalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string mobile { get; set; }

        [StringLength(50)]
        public string tel { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(100)]
        public string company { get; set; }

        [StringLength(1000)]
        public string address { get; set; }

        [StringLength(50)]
        public string postalCode { get; set; }

        public DateTime createDate { get; set; }
    }
}