%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe %~dp0ImgAutoUploadService.exe
Net Start ImgAutoUploadService
sc config ImgAutoUploadService start= auto
