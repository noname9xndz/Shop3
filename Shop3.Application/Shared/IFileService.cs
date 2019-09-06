using System.IO;

namespace Shop3.Application.Shared
{
    public interface IFileService
    {
        bool CheckDirectoryExist(string path);

        void CreateDirectory(string path);

        FileStream CreateFile(string filePath);

        bool CheckFileExist(FileInfo file);

        void DeleteFile(FileInfo file);

    }
}
