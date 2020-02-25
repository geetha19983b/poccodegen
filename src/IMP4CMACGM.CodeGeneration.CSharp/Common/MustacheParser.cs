using Newtonsoft.Json.Linq;
using Stubble.Core.Builders;
using Stubble.Extensions.JsonNet;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IMP4CMACGM.CodeGeneration.CSharp.Common
{
    public static class MustacheParser
    {

        /// <summary>
        /// To generate Code File based on Mustache File and JSON file
        /// </summary>
        /// <param name="fileName"> Code File to be created</param>
        /// <param name="jsonObject">JSON Object which contains all values for parsing Mustache file</param>
        /// <returns></returns>
        public static async Task<string> GenerateCodeFile(string fileName, string codeFilePath, JObject jsonObject)
        {
            var stubble = new StubbleBuilder().Configure(settings => settings.AddJsonNet()).Build();

            string startupMustacheFile = Directory.GetCurrentDirectory() + @"\Templates\AspDotNetCoreTemplate\" + fileName + ".Mustache";

            string codeOutput = string.Empty;
            using (StreamReader streamReader = new StreamReader(startupMustacheFile, Encoding.UTF8))
            {
                codeOutput = stubble.Render(streamReader.ReadToEnd(), jsonObject);

                await File.WriteAllTextAsync(codeFilePath, codeOutput);
            }

            return codeOutput;
        }
    }
}
