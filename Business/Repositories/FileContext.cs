using Business.Interfaces;
using Newtonsoft.Json;

namespace Business.Repositories
{
    public class FileContext<T> : IFileContext<T> where T : class
    {
        public static string UserData { get; set; } = "./UserData.txt";
        public void Create(T entity)
        {
            if (entity is User userEntity)
            {
                string json = "," + JsonConvert.SerializeObject(userEntity, Formatting.Indented);
                if (userEntity.UserName != null)
                {
                    using (StreamWriter writer = new StreamWriter(UserData, true))
                    {
                        writer.WriteLine(json);
                    }
                }
            }
        }

        public void Delete(T entity)
        {
            if (entity is User user)
            {
                using (StreamReader reader = new StreamReader("./UserData.txt"))
                {
                    string json = "[" + reader.ReadToEnd() + "]";
                    List<User> Data = JsonConvert.DeserializeObject<List<User>>(json);
                    Data.RemoveAt(Data.FindIndex(x => x.ID == user.ID));
                    json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                    reader.Close();
                    json = json.Substring(1, json.Length - 2);
                    File.WriteAllText(UserData, json);
                }
            }
        }

        public ICollection<T> Read(T entity)
        {
            if (entity is User userEntity)
            {
                using (StreamReader reader = new StreamReader(UserData))
                {
                    string json = "[" + reader.ReadToEnd() + "]";
                    List<User> Data = JsonConvert.DeserializeObject<List<User>>(json);
                    return (ICollection<T>)Data;
                }
            }
            return null;
        }

        public void Update(T entity)
        {
            if (entity is User user)
            {
                using (StreamReader reader = new StreamReader(UserData))
                {
                    string json = "[" + reader.ReadToEnd() + "]";
                    List<User> Data = JsonConvert.DeserializeObject<List<User>>(json);
                    Data[Data.FindIndex(x => x.ID == user.ID)] = user;
                    json = JsonConvert.SerializeObject(Data, Formatting.Indented);
                    reader.Close();
                    json = json.Substring(1, json.Length - 2);
                    File.WriteAllText(UserData, json);
                }
            }
        }
    }
}
