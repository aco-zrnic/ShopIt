using System.ComponentModel.DataAnnotations;

namespace Server.Options
{
    public class AwsS3Options
    {
        public const string Gateway = "AwsConfiguration";
        [Required]
        public string BucketName { get; set; }
        [Required]
        public string AWSAccessKey { get; set; }
        [Required]
        public string AWSSecretKey { get; set; }
    }
}
