using System.IO;
using System.Xml.Serialization;
using XmlViewer.Editor.Const;
using XmlViewer.Editor.Model;

namespace XmlViewer.Editor.Helper
{
    internal static class LoadHistoryItemHelper
    {
        private static string cacheFolder;

        private static string filePath;

        static LoadHistoryItemHelper()
        {
            ConstructFilePath();
        }

        public static HistoryItemsModel Load()
        {
            var serializationType = typeof(HistoryItemsModel);
            var serializer = new XmlSerializer(serializationType);

            if (File.Exists(filePath) == false)
            {
                return default;
            }

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var model = (HistoryItemsModel) serializer.Deserialize(fileStream);

                return model;
            }
        }

        public static void Save(HistoryItemsModel model)
        {
            var serializationType = typeof(HistoryItemsModel);
            var serializer = new XmlSerializer(serializationType);

            CreateFolder();
            CreateHistoryFile();

            using (var fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                serializer.Serialize(fileStream, model);
            }
        }

        private static void CreateFolder()
        {
            if (Directory.Exists(cacheFolder) == false)
            {
                Directory.CreateDirectory(cacheFolder);
            }
        }

        private static void CreateHistoryFile()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static void ConstructFilePath()
        {
            var root = Directory.GetCurrentDirectory();

            cacheFolder = Path.Combine(root, ConfigConst.CACHE_FOLDER);

            filePath = Path.Combine(cacheFolder, ConfigConst.HISTORY_ITEM_FILE_NAME);
        }
    }
}
