using Minio;
using Minio.DataModel.Args;
using VideoService.Application.DTO;
using VideoService.Core.Results;

namespace VideoService.Application.Services;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    private readonly IConfiguration _configuration;
    
    public MinioService(IMinioClient minioClient, IConfiguration configuration)
    {
        _minioClient = minioClient;
        _configuration = configuration;
    }

    public async Task<Result<string>> CreateBucket(string bucketName)
    {
        //bucketName = bucketName.ToLowerInvariant();

        var bucketExists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName)
        );
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName)
            );
        }

        var policyJson =
            $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Effect"":""Allow"",""Principal"":{{""AWS"":""*""}},""Action"":[""s3:GetObject""],""Resource"":[""arn:aws:s3:::{bucketName}/*""]}}]}}";

        var args = new SetPolicyArgs()
            .WithBucket(bucketName)
            .WithPolicy(policyJson);

        await _minioClient.SetPolicyAsync(args).ConfigureAwait(false);

        return Result<string>.Success("bucket created successfully and is public");
    }

    public async Task<Result<string>> DeleteBucket(string bucketName)
    {
        await _minioClient.RemoveBucketAsync(
            new RemoveBucketArgs().WithBucket(bucketName));
        return Result<string>.Success("bucket deleted successfully");
    }
    
    public async Task<Result<string>> UploadUserAvatar(UploadUserAvatarRequestDto requestDto)
    {
        var fileName = $"{Guid.NewGuid()}_{requestDto.File.FileName}";
        var filePath = Path.Combine(Path.GetTempPath(), fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await requestDto.File.CopyToAsync(stream);
        }

        await _minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket("user-photos")
                .WithObject(fileName)
                .WithFileName(filePath)
                .WithContentType(requestDto.File.ContentType)
        );
        
        var fileUrl = $"{_configuration["MinioSettings:PublicMinioURL"]}/user-photos/{fileName}";
        
        return Result<string>.Success(fileUrl);
    }

    public async Task<Result<string>> DeleteObj(string bucketName, string fileName)
    {
        var objectExists = await _minioClient.StatObjectAsync(
            new StatObjectArgs().WithBucket(bucketName).WithObject(fileName)
        );
        
        if(objectExists == null)
        {
            return Result<string>.Failure("Object does not exist");
        }
        
        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs().WithBucket(bucketName).WithObject(fileName));
        
        return Result<string>.Success("Object deleted successfully");
    }
}