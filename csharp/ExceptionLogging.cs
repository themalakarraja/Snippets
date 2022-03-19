using System;
using System.IO;

/// <summary>  
/// Summary description for ExceptionLogging  
/// </summary>
public static class ExceptionLogging
{
    public static void SaveErrorToText(Exception ex)
    {
        string exceptionFileDir = Path.Combine(Directory.GetCurrentDirectory(), "Log");
        string exceptionFilePath = Path.Combine(exceptionFileDir, DateTime.Today.ToString("dd-MM-yy") + ".txt");

        try
        {
            if (!Directory.Exists(exceptionFileDir))
            {
                Directory.CreateDirectory(exceptionFileDir);
            }

            using (StreamWriter sw = File.AppendText(exceptionFilePath))
            {
                sw.WriteLine("-----------Exception Details on " + DateTime.Now.ToString() + "-----------------");
                sw.WriteLine(ex.ToString());
                sw.WriteLine(Environment.NewLine);
            }
        }
        catch (Exception e)
        {
            e.ToString();
        }
    }
}