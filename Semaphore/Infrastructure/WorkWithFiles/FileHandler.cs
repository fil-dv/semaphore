using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semaphore.Infrastructure.WorkWithFiles
{
    public static class FileHandler
    {
        public static string ReadFile(string path)
        {
            string res = "";
            try
            {
                using (var streamReader = File.OpenText(path))
                {
                    res = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("From Semaphore.Infrastructure.WorkWithFiles.ReadFile()" + ex.Message);
            }
            return res;
        }

        public static void WriteToFile(string path, string text)
        {           
            try
            {
                string createText = "Hello and Welcome" + Environment.NewLine;
                File.WriteAllText(path, text);                
            }
            catch (Exception ex)
            {
                MessageBox.Show("From Semaphore.Infrastructure.WorkWithFiles.WriteToFile()" + ex.Message);
            }           
        }

    }
}
