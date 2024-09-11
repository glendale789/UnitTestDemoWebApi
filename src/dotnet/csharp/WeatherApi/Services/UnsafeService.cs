using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;


namespace WeatherApi.Services
{
    public class UnSafeService
    {
        public string ReadFile(string userInput)
        {
            using (FileStream fs = File.Open(userInput, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                StringBuilder result = new StringBuilder();

                int bytesRead;
                while ((bytesRead = fs.Read(b, 0, b.Length)) > 0)
                {
                    result.Append(temp.GetString(b, 0, bytesRead));
                }

                return result.ToString();
            }
        }

        public int GetProduct(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name cannot be null or empty.");
            }

            using (SqlConnection connection = new SqlConnection("fakeconnectionstring"))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("SELECT ProductId FROM Products WHERE ProductName = @ProductName", connection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@ProductName", productName);

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        throw new Exception("Product not found.");
                    }
                }
            }
        }
    }
}