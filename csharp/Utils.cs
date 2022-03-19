using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// This class contains all utility methods used in application
/// </summary>
public static class ExaminationUtils
{
    /// <summary>
    /// Checks wheather the specified object is null or an empty.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>False if the object is null or an empty, otherwise true.</returns>
    public static Boolean isDefined(object obj)
    {
        return obj != null && !obj.Equals("");
    }

    /// <summary>
    /// Checks wheather the specified string is null or an empty.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>False if the string is null or an empty, otherwise true.</returns>
    public static bool isDefined(string str)
    {
        return str != null && !str.Equals("");
    }

    /// <summary>
    /// Generate 15 characters unique alpha numeric string.
    /// </summary>
    /// <returns></returns>
    public static string generateDateTimeTicks()
    {
        return DateTime.Now.Ticks.ToString("x");
    }

    /// <summary>getRandomHexadecimal
    /// Generate random hexadecimal characters in lower case.
    /// </summary>
    /// <param name="length">Length of string</param>
    /// <returns>Hexadecimal string as per the given length</returns>
    public static string getRandomString(int length)
    {
        string KEY = "0123456789abcdef9876543210fedcba0123456789abcdef";
        string randomNumber = "";
        Random rnd = new Random();
        int iRandom;
        for (int i = 1; i <= length; i++)
        {
            iRandom = rnd.Next(1, KEY.Length);
            randomNumber += KEY.Substring(iRandom, 1);
        }
        return randomNumber;
    }

    public static bool isDirExists(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }

    public static void createDir(string directoryPath)
    {
        if (!isDirExists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public static void deleteDir(string directoryPath, bool recursive = false)
    {
        if (isDirExists(directoryPath))
        {
            Directory.Delete(directoryPath, recursive);
        }
    }

    public static bool isFileExists(string filePathWithExtension)
    {
        return File.Exists(filePathWithExtension);
    }

    public static void deleteFile(string filePathWithExtension)
    {
        File.Delete(filePathWithExtension);
    }

    public static void moveFile(string sourceFilePathWithExtension, string destFilePathWithExtension)
    {
        if (!isDirExists(Path.GetDirectoryName(destFilePathWithExtension)))
        {
            createDir(Path.GetDirectoryName(destFilePathWithExtension));
        }
        File.Move(sourceFilePathWithExtension, destFilePathWithExtension);
    }

    public static byte[] readAllBytesOfFile(string filePathWithExtension)
    {
        if (isFileExists(filePathWithExtension))
        {
            return File.ReadAllBytes(filePathWithExtension);
        }
        return null;
    }

    /// <summary>
    /// Creates a new file and writes the specified string to the file. 
    /// If the target file already exists, it is overwritten.
    /// </summary>
    /// <param name="filePathWithExtension">The file path to write the specified string to.</param>
    /// <param name="contents">The string to write to the file.</param>
    public static void createFile(string filePathWithExtension, string contents)
    {
        if (!isDirExists(Path.GetDirectoryName(filePathWithExtension)))
            createDir(Path.GetDirectoryName(filePathWithExtension));
        if (isFileExists(filePathWithExtension))
            deleteFile(filePathWithExtension);
        File.WriteAllText(filePathWithExtension, contents);
    }

    /// <summary>
    /// Appends the specified string to the file. If the file does not exist, 
    /// this method creates a new file and writes the specified string to the file, 
    /// otherwise append string to the exesting file.
    /// </summary>
    /// <param name="filePathWithExtension">The file path to append the specified string to.</param>
    /// <param name="contents">The string to append to the file.</param>
    public static void appendFile(string filePathWithExtension, string contents)
    {
        if (!isFileExists(filePathWithExtension))
        {
            createFile(filePathWithExtension, contents);
        }
        else
        {
            File.AppendAllText(filePathWithExtension, "\r\n" + contents);
        }
    }

    public static void saveByteArray(string filePathWithExtension, byte[] bytes)
    {
        if (!isDirExists(Path.GetDirectoryName(filePathWithExtension)))
        {
            createDir(Path.GetDirectoryName(filePathWithExtension));
        }

        if (isFileExists(filePathWithExtension))
        {
            deleteFile(filePathWithExtension);
        }
        File.WriteAllBytes(filePathWithExtension, bytes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contents"></param>
    public static void createLog(string contents)
    {
        string logFilePathWithExtension = HttpContext.Current.Server.MapPath(
            UrlManager.LOG_DIR + "/" + UrlManager.LOG_FILE_NAME);
        contents = "\r\n" + "\r\n" + "\r\n" + DateTime.Now.ToString() + "\r\n" + contents;

        if (!Directory.Exists(HttpContext.Current.Server.MapPath(UrlManager.LOG_DIR)))
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(UrlManager.LOG_DIR));

        if (File.Exists(logFilePathWithExtension))
            File.AppendAllText(logFilePathWithExtension, contents);
        else
            File.WriteAllText(logFilePathWithExtension, contents);
    }
}