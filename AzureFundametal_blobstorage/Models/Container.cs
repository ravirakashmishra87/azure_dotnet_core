using System.ComponentModel.DataAnnotations;

namespace AzureFundametal_blobstorage.Models
{
    public class Container
    {
        [Required]
        public string? Name { get; set; }
    }
}
