using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using Server.Exceptions;
using Server.Models.Request;
using Server.Models.Response;
using Server.Options;
using System.Net;

namespace Server.Services
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly AmazonS3Client _s3client;
        private readonly ILogger<AwsS3Service> _logger;
        private readonly AwsS3Options _options;
        public AwsS3Service(IOptions<AwsS3Options> options,
            ILogger<AwsS3Service> logger)
        {
            _options = options.Value;
            _logger = logger;

            var credentials = new BasicAWSCredentials(_options.AWSAccessKey, _options.AWSSecretKey);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUCentral1
            };
            _s3client = new AmazonS3Client(credentials, config);
        }

        public async Task<S3Response> DeleteFileAsync(string keyName)
        {
            try
            {
                DeleteObjectResponse response = await _s3client.DeleteObjectAsync(new DeleteObjectRequest { BucketName =  _options.BucketName, Key =  keyName });
                
                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw new UserFriendlyException(ErrorCode.AWS_ERROR, $"Failed to retrieve image. HTTP Status Code: {response.HttpStatusCode}");

                return new S3Response() { StatusCode = 200 };
            }
            catch (AmazonS3Exception s3Exception)
            {
                throw new AwsS3Exception(s3Exception.ErrorCode, s3Exception.Message, s3Exception.ResponseBody);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ErrorCode.GENERAL, ex.Message, ex.InnerException);
            }
        }

        public async Task<S3Response> GetFileAsync(string keyName)
        {
            try
            { 
                var getObject = new GetObjectRequest
                {
                    BucketName = _options.BucketName,
                    Key = keyName
                };

                using GetObjectResponse response = await _s3client.GetObjectAsync(getObject);

                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw new UserFriendlyException(ErrorCode.AWS_ERROR, $"Failed to retrieve image. HTTP Status Code: {response.HttpStatusCode}");

                await using var memoryStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(memoryStream);
                var base64Image = Convert.ToBase64String(memoryStream.ToArray());

                return new S3Response() { Base64File = base64Image, StatusCode = 200 };
            }
            catch (AmazonS3Exception s3Exception)
            {
                throw new AwsS3Exception(s3Exception.ErrorCode, s3Exception.Message, s3Exception.ResponseBody);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ErrorCode.GENERAL, ex.Message, ex.InnerException);
            }
        }

        public async Task<S3Response> UploadFileAsync(UploadS3Object request)
        {
            var response = new S3Response();
            try
            {

                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = request.InputStream,
                    Key = request.Name,
                    BucketName = _options.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                // initialise the transfer/upload tools
                var transferUtility = new TransferUtility(_s3client);

                // initiate the file upload
                await transferUtility.UploadAsync(uploadRequest);
                response.StatusCode = 201;
                response.Message = $"{request.Name} has been uploaded sucessfully";
            }
            catch (AmazonS3Exception s3Exception)
            {
                throw new AwsS3Exception(s3Exception.ErrorCode, s3Exception.Message, s3Exception.ResponseBody);  
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ErrorCode.GENERAL, ex.Message,ex.InnerException);
            }

            return response;
        }
    }
}
