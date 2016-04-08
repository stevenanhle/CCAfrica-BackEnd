using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Hosting;

namespace Login_Registration.Models
{
    public class Document
    {
        public List<FileNames> GetFiles()
        { 
            List<FileNames> lstFiles = new List<FileNames>();
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/Files_Documents")); 
            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                lstFiles.Add(new FileNames() {
                FileId = i + 1, 
                FileName = item.Name,
                FilePath = dirInfo.FullName+@"\"+item.Name
                });
                i = i + 1;
            }
            return lstFiles; 
        }
    }

    //Another class called FileNames
    public class FileNames
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
    
}