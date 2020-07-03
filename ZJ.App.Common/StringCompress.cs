using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace ZJ.App.Common
{
    public static class StringCompress
    {
        public static string CompressToBase64String(string OrginalString)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream stream = new GZipStream(ms, CompressionMode.Compress))
                {
                    byte[] bdata = Encoding.UTF8.GetBytes(OrginalString);
                    stream.Write(bdata, 0, bdata.Length);
                    stream.Close();
                    ms.Close();
                    return  Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string DeCompressToBase64String(string Base64String)
        {
            byte[] OrignalBytes = Convert.FromBase64String(Base64String);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(OrignalBytes, 0, OrignalBytes.Length);
                ms.Position = 0;
                using (GZipStream stream = new GZipStream(ms, CompressionMode.Decompress))
                {
                    using (MemoryStream temp = new MemoryStream())
                    {
                        byte[] buffer = new byte[1024];
                        while (true)
                        {
                            int read = stream.Read(buffer, 0, buffer.Length);
                            if (read <= 0)
                            {
                                break;
                            }
                            else
                            {
                                temp.Write(buffer, 0, read);
                            }
                        }
                        temp.Close();
                        stream.Close();
                        ms.Close();
                        return Encoding.UTF8.GetString(temp.ToArray());
                    }
                }
                
            }

        }

    }
}
