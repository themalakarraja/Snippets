using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

public class ImageCompress
{
    public ImageCompress(string sourceFilePath, string destDirectoryPath, long compressionLevel = 40L)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
        Image imgToCompress = Image.FromFile(sourceFilePath);
        Bitmap bitMap = new Bitmap(imgToCompress);
        ImageCodecInfo imageCodecInfo = GetEncoder(ImageFormat.Jpeg); //Current Image Format is Jpeg

        // Create an Encoder object based on the GUID
        // for the Quality parameter category.
        Encoder encoder = Encoder.Quality;

        // Create an EncoderParameters object.
        // An EncoderParameters object has an array of EncoderParameter objects.
        // In this case, there is only one EncoderParameter object in the array.
        EncoderParameters encoderParameters = new EncoderParameters(1);
        EncoderParameter encoderParameter = new EncoderParameter(encoder, compressionLevel);
        encoderParameters.Param[0] = encoderParameter;
        CreateFolder(destDirectoryPath);
        var fileSavePath = Path.Combine(destDirectoryPath, fileNameWithoutExtension 
            + "." + imageCodecInfo.FormatDescription.ToLower());
        bitMap.Save(fileSavePath, imageCodecInfo, encoderParameters);
        imgToCompress.Dispose();
    }

    private void CreateFolder(string folder_path)
    {
        if (Directory.Exists(folder_path))
        {
            Directory.Delete(folder_path, true);
            Directory.CreateDirectory(folder_path);
        }
        else
        {
            Directory.CreateDirectory(folder_path);
        }
    }

    private ImageCodecInfo GetEncoder(ImageFormat format)
    {

        ImageCodecInfo[] imageCodecInfos = ImageCodecInfo.GetImageDecoders();

        foreach (ImageCodecInfo imageCodecInfo in imageCodecInfos)
        {
            if (imageCodecInfo.FormatID == format.Guid)
            {
                return imageCodecInfo;
            }
        }
        return null;
    }
}