using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication17.Models
{
	public class Expert
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Job { get; set; }
		public string ImgUrl { get; set; }
		[NotMapped]
		public IFormFile ImgFile { get; set; }
	}
}
