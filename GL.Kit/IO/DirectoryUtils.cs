namespace System.IO
{
    public static class DirectoryUtils
    {
        public static void Rename(string srcDirectory, string newname)
        {
            Directory.Move(srcDirectory, Path.Combine(Path.GetDirectoryName(srcDirectory), newname));
        }

        public static void Copy(string srcFolder, string destFolder)
        {
            Directory.CreateDirectory(destFolder);

            string[] files = Directory.GetFiles(srcFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }

            string[] folders = Directory.GetDirectories(srcFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                Copy(folder, dest);
            }
        }
    }
}
