namespace Server.Models.Request
{
    public class UploadS3Object
    {
        public string Name { get; set; } = null!;
        public MemoryStream InputStream { get; set; } = null!;
    }
}
