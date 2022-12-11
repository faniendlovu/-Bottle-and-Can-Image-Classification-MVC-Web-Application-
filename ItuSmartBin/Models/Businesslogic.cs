using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using static System.Net.WebRequestMethods;

namespace ItuSmartBin.Models
{
    public class Businesslogic
    {
        public Stream GetImageready(HttpPostedFileBase file)
        {

   
            var FileLen = file.ContentLength;
            byte[] input = new byte[FileLen];
            System.IO.Stream MyStream;
            // Initialize the stream.
           return MyStream = file.InputStream;

            // Read the file into the byte array.
     //     return     MyStream.Read(input, 0, FileLen);


           // MemoryStream testimage = new MemoryStream();
           //testimage = new MemoryStream(File.ReadAllBytes(imagefile.ContentType));
           //return testimage;
          
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

    }
}