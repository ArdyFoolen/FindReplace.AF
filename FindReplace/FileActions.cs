using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindReplace
{
    public class FileActions
    {
        public string RootDirectoryPath { get; set; }
        public string FilePattern { get; set; }
        public bool IncludingSubdirectories { get; set; }
        public bool CaseSensitive { get; set; }

        public delegate void SkipDelegate(string file);
        public event SkipDelegate OnSkipEvent;

        private void Skip(string file)
        {
            if (OnSkipEvent != null)
                OnSkipEvent(file);
        }

        public FileActions()
        {
            this.RootDirectoryPath = Directory.GetCurrentDirectory();
            this.IncludingSubdirectories = false;
            this.CaseSensitive = false;
        }

        public List<string> Find(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return null;
            List<string> files = this.getFiles(this.RootDirectoryPath);
            return files.Where(w => hasSearchText(w, searchText)).ToList();
        }

        private List<string> replacedList;
        public List<string> Replace(string searchText, string replaceText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return null;
            List<string> files = this.getFiles(this.RootDirectoryPath);
            this.replacedList = new List<string>();
            files.ForEach(f => replace(f, searchText, replaceText));
            return this.replacedList;
        }

        private List<string> getFiles(string path)
        {
            List<string> files = (string.IsNullOrWhiteSpace(this.FilePattern)) ? Directory.GetFiles(path).ToList() : Directory.GetFiles(path, this.FilePattern).ToList();
            if (this.IncludingSubdirectories)
            {
                Directory.GetDirectories(path).ToList().ForEach(f => files.AddRange(this.getFiles(f)));
            }

            return files;
        }

        private bool hasSearchText(string file, string searchText)
        {
            if (File.Exists(file))
            {
                try
                {
                    using (StreamReader sr = File.OpenText(file))
                    {
                        string line = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (this.CaseSensitive)
                            {
                                if (line.Contains(searchText))
                                    return true;
                            }
                            else
                                if (line.ToLowerInvariant().Contains(searchText.ToLowerInvariant()))
                                return true;
                        }
                    }
                }
                catch (Exception)
                {
                    Skip(file);
                }
            }
            return false;
        }

        private void replace(string file, string searchText, string replaceText)
        {
            if (hasSearchText(file, searchText))
            {
                replacedList.Add(file);
                string text = File.ReadAllText(file);
                text = text.Replace(searchText, replaceText);
                File.WriteAllText(file, text, getEncoding(file));
            }
        }

        private Encoding getEncoding(string file)
        {
            using (StreamReader reader = new StreamReader(file, true))
            {
                reader.Peek();
                return reader.CurrentEncoding;
            }
        }
    }
}
