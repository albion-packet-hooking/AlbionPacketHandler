using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AlbionMarshaller.Extractor
{
    public class ResourceLoader
    {
        public static XDocument LoadResource(String resourceName)
        {
            String decryptedFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Decrypted");
            if(!Directory.Exists(decryptedFolder))
            {
                Directory.CreateDirectory(decryptedFolder);
            }

            String decryptedFileName = Path.Combine(decryptedFolder, resourceName + ".xml");
            String binaryFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", resourceName + ".bin");

            String content = null;
            bool binaryFileExists = File.Exists(binaryFileName);

            // Decrypt and replace the file
            if(!File.Exists(decryptedFileName) || (binaryFileExists && File.GetCreationTime(decryptedFileName).CompareTo(File.GetCreationTime(binaryFileName)) >= 0))
            {
                content = BinaryDecrypter.DecryptBinaryFile(binaryFileName);
                File.WriteAllText(decryptedFileName, content);
            }
            else
            {
                content = File.ReadAllText(decryptedFileName);
            }
            
            return XDocument.Parse(content);
        }
    }
}
