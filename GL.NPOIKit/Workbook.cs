using NPOI.SS.UserModel;
using System.IO;

namespace GL.NpoiKit
{
    public class Workbook
    {
        readonly string _filename;
        readonly IWorkbook _workbook;

        public Workbook(string filename)
        {
            _filename = filename;
            _workbook = WorkbookExtension.GetOrCreateIWorkbook(filename);
        }

        public void Save()
        {
            using (FileStream fs = new FileStream(_filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                _workbook.Write(fs);
            }
        }

    }
}
