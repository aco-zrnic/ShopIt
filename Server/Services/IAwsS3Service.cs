using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Server.Models.Request;
using Server.Models.Response;

namespace Server.Services
{
    public interface IAwsS3Service
    {
        Task<S3Response> UploadFileAsync(UploadS3Object obj);
        Task<S3Response> GetFileAsync(string keyName);
        Task<S3Response> DeleteFileAsync(string keyName);
    }
}
