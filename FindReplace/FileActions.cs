using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindReplace
{
    public class FileActions
    {
        public string RootDirectoryPath { get; set; }
        public string FilePattern { get; set; }
        public bool IncludingSubdirectories { get; set; }
        public bool CaseSensitive { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public delegate void SkipDelegate(string file);
        public event SkipDelegate OnSkipEvent;

        private void Skip(string file)
            => OnSkipEvent?.Invoke(file);

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

        public async Task<List<string>> FindAsync(string searchText)
        {
            await Task.Yield();
            return Find(searchText);
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

        public async Task<List<string>> ReplaceAsync(string searchText, string replaceText)
        {
            await Task.Yield();
            return Replace(searchText, replaceText);
        }

        private List<string> getFiles(string path)
        {
            ThrowIfCancellationRequested();
            List<string> files = new List<string>();

            try
            {
                files = (string.IsNullOrWhiteSpace(this.FilePattern)) ? Directory.GetFiles(path).ToList() : Directory.GetFiles(path, this.FilePattern).ToList();

                if (this.IncludingSubdirectories)
                {
                    Directory.GetDirectories(path).ToList().ForEach(f => files.AddRange(this.getFiles(f)));
                }
            }
            catch (Exception)
            {
                Skip(path);
            }

            return files;
        }

        private bool hasSearchText(string file, string searchText)
        {
            ThrowIfCancellationRequested();
            try
            {
                if (File.Exists(file))
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
            }
            catch (Exception)
            {
                Skip(file);
            }
            return false;
        }

        private void ThrowIfCancellationRequested()
        {
#if DEBUG
            Debug.WriteLine($"Canceled: {CancellationToken.IsCancellationRequested}");
#endif
            if (!CancellationToken.IsCancellationRequested)
                return;
            CancellationToken.ThrowIfCancellationRequested();
        }

        private void replace(string file, string searchText, string replaceText)
        {
            try
            {
                if (hasSearchText(file, searchText))
                {
                    if (IsReadOnly(new FileInfo(file)))
                    {
                        Skip(file);
                        return;
                    }
                    replacedList.Add(file);
                    string text = File.ReadAllText(file);
                    text = text.Replace(searchText, replaceText);
                    write(file, text);
                }
            }
            catch (Exception) { }
        }

        private bool IsReadOnly(FileInfo fileInfo)
            => fileInfo.IsReadOnly;

        private void write(string file, string text)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(file, false, getEncoding(file)))
                {
                    writer.Write(text);
                }
            }
            catch (IOException)
            {
                Skip(file);
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
