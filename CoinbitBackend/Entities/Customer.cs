using System;
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

        public int StatusId { get; set; }

        [ForeignKey("StatusId")]   
        public CustomerStatus CustomerStatus { get; set; }

        public int user_id { get; set; }

        [ForeignKey("user_id")]
        public User user { get; set; }

        public string idcardpic { get; set; }
        
        public string bankcardpic { get; set; }
        
        public string selfiepic { get; set; }

        public int? bank_id { get; set; }

        [ForeignKey("bank_id")]
        public Bank bank { get; set; }

        [StringLength(100)]
        public string card_number { get; set; }

        [StringLength(100)]
        public string sheba_number { get; set; }

        public int verify_user_id { get; set; } 
    }
}    