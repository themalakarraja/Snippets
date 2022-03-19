using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Sharing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CSI.Bal.DropBox
{
    public class DropboxAPITool
    {
        private string _dToken = "";
        private DropboxClient dbx = null;
        public DropboxAPITool()
        {
            _dToken = "";
            dbx = new DropboxClient(_dToken);
        }

        public async Task getFullDetailsOfAccount()
        {
            var full = await dbx.Users.GetCurrentAccountAsync();
            Console.WriteLine("{0} - {1}", full.Name.DisplayName, full.Email);
        }

        public async Task ListRootFolder(string folder = "")
        {
            var list = await dbx.Files.ListFolderAsync(folder);

            // show folders then files
            //foreach (var item in list.Entries.Where(i => i.IsFolder))
            //{
            //    Console.WriteLine("D  {0}/", item.Name);
            //}

            foreach (var item in list.Entries.Where(i => i.IsFile))
            {
                Console.WriteLine("F{0,8} {1}", item.AsFile.Size / 1024, item.Name);
            }
        }

        public async Task Download(string folder, string fileName)
        {
            Console.WriteLine("Download file...");
            try
            {
                using (var response = await dbx.Files.DownloadAsync(folder + "/" + fileName))
                {
                    Console.WriteLine("Downloaded {0} Rev {1}", response.Response.Name, response.Response.Rev);
                    Console.WriteLine("------------------------------");
                    Console.WriteLine(await response.GetContentAsStringAsync());
                    Console.WriteLine("------------------------------");
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Uploads given content to a file in Dropbox.
        /// </summary>
        /// <param name="folder">The folder to upload the file.</param>
        /// <param name="file">File path</param>
        /// <param name="fileName">The name of the file with extension.</param>
        /// <returns></returns>
        public async Task FileUpload(string folder, string file, string fileName)
        {
            if (!File.Exists(file))
            {
                throw new Exception("File not found " + file);
            }

            try
            {
                // Chunk size is 10MB.
                const int CHUNK_SIZE = 10 * 1024 * 1024;
                long uploadFileSize = new System.IO.FileInfo(file).Length;

                // check if upload file size more than CHUNK_SIZE (10MB)
                if (uploadFileSize > CHUNK_SIZE)
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(file)))
                    {
                        int numChunks = (int)Math.Ceiling((double)stream.Length / CHUNK_SIZE);
                        //Console.WriteLine("Total Chunks: " + numChunks.ToString());

                        byte[] buffer = new byte[CHUNK_SIZE];
                        string sessionId = null;

                        for (var idx = 0; idx < numChunks; idx++)
                        {
                            //Console.WriteLine("uploading chunk {0}", idx);
                            var byteRead = stream.Read(buffer, 0, CHUNK_SIZE);

                            using (MemoryStream memStream = new MemoryStream(buffer, 0, byteRead))
                            {
                                if (idx == 0)
                                {
                                    var result = await dbx.Files.UploadSessionStartAsync(body: memStream);
                                    sessionId = result.SessionId;
                                }
                                else
                                {
                                    UploadSessionCursor cursor = new UploadSessionCursor(sessionId, (ulong)(CHUNK_SIZE * idx));

                                    if (idx == numChunks - 1)
                                    {
                                        await dbx.Files.UploadSessionFinishAsync(cursor, new CommitInfo(folder + "/" + fileName), body: memStream);
                                    }
                                    else
                                    {
                                        await dbx.Files.UploadSessionAppendV2Async(cursor, body: memStream);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (var memStream = new MemoryStream(File.ReadAllBytes(file)))
                    {
                        var updated = await dbx.Files.UploadAsync(folder + "/" + fileName, WriteMode.Overwrite.Instance, body: memStream);
                        //Console.WriteLine("Saved {0}/{1} rev {2}", folder, file, updated.Rev);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task FileUpload(string folder, IFormFile file, string fileName)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    using (var memStream = new MemoryStream(memoryStream.ToArray()))
                    {
                        // Chunk size is 10MB.
                        const int CHUNK_SIZE = 10 * 1024 * 1024;

                        // check if upload file size more than CHUNK_SIZE (10MB)
                        if (memStream.Length > CHUNK_SIZE)
                        {
                            int numChunks = (int)Math.Ceiling((double)memStream.Length / CHUNK_SIZE);
                            byte[] buffer = new byte[CHUNK_SIZE];
                            string sessionId = null;
                            for (var idx = 0; idx < numChunks; idx++)
                            {
                                var byteRead = memStream.Read(buffer, 0, CHUNK_SIZE);
                                using (MemoryStream stream = new MemoryStream(buffer, 0, byteRead))
                                {
                                    if (idx == 0)
                                    {
                                        var result = await dbx.Files.UploadSessionStartAsync(body: stream);
                                        sessionId = result.SessionId;
                                    }
                                    else
                                    {
                                        UploadSessionCursor cursor = new UploadSessionCursor(sessionId, (ulong)(CHUNK_SIZE * idx));

                                        if (idx == numChunks - 1)
                                        {
                                            await dbx.Files.UploadSessionFinishAsync(cursor, new CommitInfo(folder + "/" + fileName), body: stream);
                                        }
                                        else
                                        {
                                            await dbx.Files.UploadSessionAppendV2Async(cursor, body: stream);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            await dbx.Files.UploadAsync(folder + "/" + fileName, WriteMode.Overwrite.Instance, body: memStream);
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
       }

        public async Task<string> GetSharedUrl(string folderfile)
        {
            string sharedlink = string.Empty;
            SharedLinkMetadata sharedLinkMetadata;
            try
            {
                sharedLinkMetadata = await dbx.Sharing.CreateSharedLinkWithSettingsAsync(folderfile);
                var sharedLinksMetadata = await dbx.Sharing.ListSharedLinksAsync(folderfile, null, true);
                sharedLinkMetadata = sharedLinksMetadata.Links.First();
                sharedlink = sharedLinkMetadata.Url;
            }
            catch (ApiException<CreateSharedLinkWithSettingsError> err)
            {
                if (err.ErrorResponse.IsSharedLinkAlreadyExists)
                {
                    var sharedLinksMetadata = await dbx.Sharing.ListSharedLinksAsync(folderfile, null, true);
                    sharedLinkMetadata = sharedLinksMetadata.Links.First();
                    sharedlink = sharedLinkMetadata.Url;
                }
                else
                {
                    //throw err;
                }
            }
            return sharedlink;
        }
    }
}
