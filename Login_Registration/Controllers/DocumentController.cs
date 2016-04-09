using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Login_Registration.Models;

namespace Login_Registration.Controllers
{
    public class DocumentController : Controller
    {
        //
        // GET: /Document/

        Document objData =new Document(); 

        public ActionResult Index()
        {
            var files = objData.GetFiles();  
            return View(files);
        }
 
        [Authorize]
        public FileResult Download(string id)
        {
            int fID = Convert.ToInt32(id);
            var files = objData.GetFiles();
            //LINQ statement, files is like a database of files, even it is an array of files
            string filename = (from f in files
                               where f.FileId == fID
                               select f.FilePath).First();
            string contentType = "application/pdf";
            //Parameters to file are
            //1. The File Path on the File Server
            //2. The content type MIME type
            //3. The parameter for the file save by the browser
            return File(filename, contentType,"CCAfrica_Report.pdf");
        }
        
        //We can open pdf file within Google Chrome by using this function
        //For Firefox, it does not work. I guess it can work with Firefox but we may need to install some extensions 
        public FileResult ViewFile(string id)
        {
            int fID = Convert.ToInt32(id);
            var files = objData.GetFiles();
            //LINQ statement, files is like a database of files, even it is an array of files
            string filename = (from f in files
                               where f.FileId == fID
                               select f.FilePath).First();
            string contentType = "application/pdf";
            return File(filename, contentType);
        }

    }
}
