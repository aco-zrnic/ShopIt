namespace Server.Models.Response
{
    public class S3Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Base64File { get; set; }
    }
}
