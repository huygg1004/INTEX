using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Services
{
    public class S3 : S3interface
    {
        private readonly AmazonS3Client s3Client;
        private const string BUCKET_NAME = "arn:aws:s3:us-east-1:197469302543:accesspoint/image";
        private const string FOLDER_NAME = "Photos";
        private const double DURATION = 25;

        public S3()
        {
            s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
        }

        
        public async Task<string> AddItem(IFormFile file, string readerName)
        {
            //Reading in the file when the function gets called
            string fileName = file.FileName;
            string objectKey = $"{FOLDER_NAME}/{fileName}"; //Knows where to upload the file

           

            using (Stream fileToUpload = file.OpenReadStream())
            {
                var putObjectRequest = new PutObjectRequest();
                putObjectRequest.BucketName = BUCKET_NAME;
                putObjectRequest.Key = objectKey;
                putObjectRequest.InputStream = fileToUpload;
                putObjectRequest.ContentType = file.ContentType;

                var response = await s3Client.PutObjectAsync(putObjectRequest);
                

                return GeneratePreSignedURL(objectKey);

            }
        }

        private string GeneratePreSignedURL(string objectKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = BUCKET_NAME,
                Key = objectKey,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddHours(DURATION)
            };

            string url = s3Client.GetPreSignedURL(request);
            return url;
        }
    }
}
