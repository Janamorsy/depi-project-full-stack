using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NileCareAPI.DTOs;
using NileCareAPI.Models;
using OpenCvSharp;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using SkiaSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YoloDotNet;
using YoloDotNet.Models;
using Microsoft.AspNetCore.Hosting;

namespace NileCareAPI.Services;

public class DoctorAuthService : IDoctorAuthService
{
    private readonly UserManager<DoctorUser> _userManager;
    private readonly SignInManager<DoctorUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DoctorAuthService(
        UserManager<DoctorUser> userManager,
        SignInManager<DoctorUser> signInManager,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<DoctorDto?> RegisterAsync(DoctorRegisterDto registerDto)
    {
        var doctor = new DoctorUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.PhoneNumber,
            Specialty = registerDto.Specialty,
            SpecialtyTags = registerDto.SpecialtyTags,
            Hospital = registerDto.Hospital,
            City = registerDto.City,
            Languages = registerDto.Languages,
            ConsultationFee = registerDto.ConsultationFee,
            YearsOfExperience = registerDto.YearsOfExperience,
            Bio = registerDto.Bio,
            ImageUrl = string.IsNullOrWhiteSpace(registerDto.ImageUrl)
                ? "https://avatar.iran.liara.run/public/10"
                : registerDto.ImageUrl,
            IsAvailable = true,
            NationalIdFrontImageUrl = registerDto.NationalIdFrontImageUrl,
            NationalIdBackImageUrl = registerDto.NationalIdBackImageUrl
        };
        string webRootPath = _webHostEnvironment.WebRootPath;
        string frontPhysicalPath = Path.Combine(webRootPath, doctor.NationalIdFrontImageUrl.TrimStart('/', '\\'));
        string backPhysicalPath = Path.Combine(webRootPath, doctor.NationalIdBackImageUrl.TrimStart('/', '\\'));
        if (!System.IO.File.Exists(frontPhysicalPath))
        {
            throw new Exception($"File not found at: {frontPhysicalPath}");
        }
        if (!System.IO.File.Exists(backPhysicalPath))
        {
            throw new Exception($"File not found at: {backPhysicalPath}");
        }
        var front_id_type = NationalIdClassification(frontPhysicalPath);
        var back_id_type = NationalIdClassification(backPhysicalPath);


        if (front_id_type != "front_egy_id")
        {
            throw new Exception("Rejected: Please upload a clear image of the front of your Egyptian ID.");
        }

        if (back_id_type != "back_egy_id")
        {
            throw new Exception("Rejected: Please upload a clear image of the back of your Egyptian ID.");
        }
        var job_field = JobDetector(backPhysicalPath, back_id_type);
        if (!job_field.Contains("طبيب") && !job_field.Contains("طبيبة"))
        {
            throw new Exception("Rejected: ID Profession is not a medical profession");
        }

        var result = await _userManager.CreateAsync(doctor, registerDto.Password);

        if (!result.Succeeded)
            return null;

        var token = GenerateJwtToken(doctor);

        return new DoctorDto
        {
            Id = doctor.Id,
            Email = doctor.Email ?? string.Empty,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Specialty = doctor.Specialty,
            Hospital = doctor.Hospital,
            City = doctor.City,
            Languages = doctor.Languages,
            ConsultationFee = doctor.ConsultationFee,
            YearsOfExperience = doctor.YearsOfExperience,
            ImageUrl = doctor.ImageUrl,
            NationalIdFrontImageUrl = doctor.NationalIdFrontImageUrl,
            NationalIdBackImageUrl = doctor.NationalIdBackImageUrl,
            Token = token
        };
    }

    public async Task<DoctorDto?> LoginAsync(LoginDto loginDto)
    {
        var doctor = await _userManager.FindByEmailAsync(loginDto.Email);
        if (doctor == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(doctor, loginDto.Password, false);
        if (!result.Succeeded)
            return null;

        var token = GenerateJwtToken(doctor);

        return new DoctorDto
        {
            Id = doctor.Id,
            Email = doctor.Email ?? string.Empty,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Specialty = doctor.Specialty,
            Hospital = doctor.Hospital,
            City = doctor.City,
            Languages = doctor.Languages,
            ConsultationFee = doctor.ConsultationFee,
            YearsOfExperience = doctor.YearsOfExperience,
            ImageUrl = doctor.ImageUrl,
            NationalIdFrontImageUrl = doctor.NationalIdFrontImageUrl,
            NationalIdBackImageUrl = doctor.NationalIdBackImageUrl,
            Token = token
        };
    }

    private string GenerateJwtToken(DoctorUser doctor)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, doctor.Id),
            new Claim(ClaimTypes.Email, doctor.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, $"{doctor.FirstName} {doctor.LastName}"),
            new Claim(ClaimTypes.Role, "Doctor")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private string NationalIdClassification(string imagePath)
    {
        if (!File.Exists(imagePath))
        {
            var error = $"Error: Cannot find image at '{Path.GetFullPath(imagePath)}'";
            return error;
        }
        using var image = SKImage.FromEncodedData(imagePath);
        string modelPath = Path.Combine(AppContext.BaseDirectory, "AI_Resources", "id_detection.onnx");
        using var classifier = new Yolo(new YoloOptions
        {
            OnnxModel = modelPath
        });
        var idResults = classifier.RunObjectDetection(image, confidence: 0.25, iou: 0.5);
        if (idResults.Count == 0)
        {
            var error = "No ID detected. Please upload a clear image.";
            return error;
        }
        var detectedId = idResults[0];
        string idType = detectedId.Label.Name;
        return idType;
    }
    private string JobDetector(string imagePath, string idType)
    {
        if (!File.Exists(imagePath))
        {
            var error = $"Error: Cannot find image at '{Path.GetFullPath(imagePath)}'";
            return error;
        }
        if (idType != "back_egy_id")
        {
            var error = "Unable to Identify Document. Please upload a clear image of the back of your ID.";
            return error;
        }
        using var image = SKImage.FromEncodedData(imagePath);
        string modelPath = Path.Combine(AppContext.BaseDirectory, "AI_Resources", "job_detection.onnx");
        using var jobDetector = new Yolo(new YoloOptions
        {
            OnnxModel = modelPath
        });
        var fieldResults = jobDetector.RunObjectDetection(image, confidence: 0.25, iou: 0.5);
        var jobField = fieldResults.FirstOrDefault(x =>
                    x.Label.Name == "job_title");
        if (jobField != null)
        {
            using var cvImage = Cv2.ImRead(imagePath);
            int padding = 5;

            int x = Math.Max(0, (int)jobField.BoundingBox.Left - padding);
            int y = Math.Max(0, (int)jobField.BoundingBox.Top - padding);
            int w = Math.Min((int)jobField.BoundingBox.Width + (padding * 2), cvImage.Width - x);
            int h = Math.Min((int)jobField.BoundingBox.Height + (padding * 2), cvImage.Height - y);

            Rect cropRect = new Rect(x, y, w, h);
            using var croppedJob = new Mat(cvImage, cropRect);
            string arabicText = "";
            using (var reader = new ArabicReader())
            {
                reader.Initialize();
                using var cleanedJob = reader.PreprocessForOCR(croppedJob);
                string resultText = reader.ReadArabicFromMat(cleanedJob);
                char[] charArray = resultText.ToCharArray();
                Array.Reverse(charArray);
                arabicText = new string(charArray);
            }
            return arabicText;
        }
        return "Unable to Identify Document. Please upload a clear image.";
    }
    internal class ArabicReader : IDisposable
    {
        private PaddleOcrAll _ocrEngine;
        public void Initialize()
        {
            string baseDir = AppContext.BaseDirectory;
            string modelsDir = Path.Combine(baseDir, "AI_Resources", "models");

            string detPath = Path.Combine(modelsDir, "ch_PP-OCRv3_det_infer");
            string clsPath = Path.Combine(modelsDir, "ch_ppocr_mobile_v2.0_cls_infer");
            string recPath = Path.Combine(modelsDir, "arabic_PP-OCRv3_rec_infer");
            string dictPath = Path.Combine(recPath, "ppocr_keys_v1.txt");

            var detector = DetectionModel.FromDirectory(detPath, ModelVersion.V3);
            var classifier = ClassificationModel.FromDirectory(clsPath, ModelVersion.V2);
            var recognizer = RecognizationModel.FromDirectory(recPath, dictPath, ModelVersion.V3);

            var fullModel = new FullOcrModel(detector, classifier, recognizer);

            _ocrEngine = new PaddleOcrAll(fullModel, PaddleDevice.Mkldnn())
            {
                AllowRotateDetection = true,
                Enable180Classification = true
            };
        }

        public Mat PreprocessForOCR(Mat src)
        {
            Mat resized = new Mat();
            Cv2.Resize(src, resized, new Size(0, 0), 4.0, 4.0, InterpolationFlags.Cubic);

            Mat gray = new Mat();
            Cv2.CvtColor(resized, gray, ColorConversionCodes.BGR2GRAY);

            Mat claheResult = new Mat();
            using (var clahe = Cv2.CreateCLAHE(clipLimit: 2.0, tileGridSize: new Size(8, 8)))
            {
                clahe.Apply(gray, claheResult);
            }

            Mat blurred = new Mat();
            Cv2.GaussianBlur(claheResult, blurred, new Size(0, 0), 3);

            Mat sharpened = new Mat();
            Cv2.AddWeighted(claheResult, 1.5, blurred, -0.5, 0, sharpened);

            return sharpened;
        }
        public string ReadArabicFromMat(Mat src)
        {
            if (_ocrEngine == null)
                throw new InvalidOperationException("You must call Initialize() before reading!");
            PaddleOcrResult result = _ocrEngine.Run(src);

            var validRegions = result.Regions.Where(r => r.Score > 0.6).Select(r => r.Text);
            return string.Join(" ", validRegions);
        }
        public void Dispose()
        {
            _ocrEngine?.Dispose();
            _ocrEngine = null;
        }
    }
}

