using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace DAL.Repository.XMLRepository
{
    public abstract class BaseXMLRepository<T>
    {
        private readonly string storagePath;

        protected BaseXMLRepository(string repositoryPath)
        {
            if (!Directory.Exists(repositoryPath))
                Directory.CreateDirectory(repositoryPath);

            storagePath = Path.Combine(repositoryPath, typeof(T).Name + ".xml");
        }

        protected void SaveCollection(IList<T> collection)
        {
            XmlSerializer serializer = new XmlSerializer(collection.GetType());
            using (var fs = new FileStream(storagePath, FileMode.Create))
            {
                serializer.Serialize(fs, collection);
            }

        }

        protected List<T> GetCollection()
        {
            List<T> collection = new List<T>(0);
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            using (FileStream fs = new FileStream(storagePath, FileMode.OpenOrCreate))
            {
                //TODO check file before deserialization
                if (fs.Length > 0) // hack
                    collection = (List<T>)serializer.Deserialize(fs);
            }

            return collection;
        }
    }
}
