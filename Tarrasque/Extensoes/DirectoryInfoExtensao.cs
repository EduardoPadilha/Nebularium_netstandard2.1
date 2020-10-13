using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nebularium.Tarrasque.Extensoes
{
    public static class DirectoryInfoExtensao
    {
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo diretorio, params string[] extensoes)
        {
            if (extensoes == null)
                throw new ArgumentNullException(nameof(extensoes));
            var files = diretorio.GetFiles();
            return files.Where(f => extensoes.Contains(f.Extension));
        }
    }
}
