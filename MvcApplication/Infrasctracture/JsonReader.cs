using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication.Infrasctracture
{
    public static class JsonReader
    {

        public static async Task<string> ReadOneArrayItem(StreamReader sr)
        {
            //using (var sr = new StreamReader(jsonStream,Encoding.UTF8,false,1,true))
            {
                char[] buf = new char[1];

                char firstChar = await ReadNotSpaceChar(sr,buf);
                if (firstChar == ',') firstChar = await ReadNotSpaceChar(sr, buf);
                char closeChar = GetCloseBrace(firstChar);

                int openCharsCount = 1;
                StringBuilder result = new StringBuilder(firstChar.ToString());

                while (openCharsCount != 0)
                {
                    char next = await ReadChar(sr,buf);
                    result.Append(next);
                    if (next == closeChar) openCharsCount --;
                    else if (next == firstChar) openCharsCount++;
                }
                return result.ToString();
            }
        }

        public static async Task<bool> IsEnd(StreamReader sr)
        {
           // using (var sr = new StreamReader(jsonStream, Encoding.UTF8, false, 1, true))
             {
                 char[] buf = new char[1];

                 char next = await PeekNotSpaceChar(sr,buf);
                 if (next == ',')
                 {
                     await ReadNotSpaceChar(sr,buf);
                     next = await PeekNotSpaceChar(sr,buf);
                 }
                 return next == ']';
             }
        }

        public static async Task RemoveArrayOpenChar(StreamReader sr)
        {
            //using (var sr = new StreamReader(jsonStream, Encoding.UTF8, false, 1, true))
            {
                char[] buf = new char[1];
                await ReadNotSpaceChar(sr,buf);
            }
        }

        private static char GetCloseBrace(char firstChar)
        {
            switch (firstChar)
            {
                case '{':
                    return '}';
                case '(' :
                    return ')';
                case '[':
                    return ']';
                case '<':
                    return '>';
                default:
                    return firstChar;
            }
        }

        private static async Task<char> ReadNotSpaceChar(StreamReader sr, char[] buf)
        {
            char result;
            do
            {
                result = await ReadChar(sr, buf);
            } while (char.IsWhiteSpace(result) || char.GetUnicodeCategory(result) == UnicodeCategory.Format || char.IsControl(result));
            
            return result;
        }

        private static async Task<char> ReadChar(StreamReader sr, char[] buf)
        {
            await sr.ReadAsync(buf, 0, 1);
            return buf[0];
        }

        private static async Task<char> PeekNotSpaceChar(StreamReader sr, char[] buf)
        {
            int nextChar = -1;
            do
            {
                if (nextChar != -1) await ReadChar(sr,buf);
                nextChar = sr.Peek();
            } while (char.IsWhiteSpace((char)nextChar) || char.GetUnicodeCategory((char)nextChar) == UnicodeCategory.Format || char.IsControl((char)nextChar));

            return (char)nextChar;
        }
    }
}