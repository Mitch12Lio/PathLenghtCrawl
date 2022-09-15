using System;
using System.Collections.Generic;
using System.Text;

namespace PathLenghtCrawl.Models
{
    class CustomIEnumerable
    {
        public static IEnumerable<System.IO.FileInfo> EnumerateFilesIgnoreErrors(IEnumerable<System.IO.FileInfo> files)
        {
            using (var e1 = files.GetEnumerator())
            {
                while (true)
                {
                    System.IO.FileInfo cur = null;

                    try
                    {
                        // MoveNext() can throw an Exception
                        if (!e1.MoveNext())
                            break;

                        cur = e1.Current;

                    }
                    catch (Exception ex)
                    {
                        //Debug.WriteLine(ex);
                    }

                    if (cur != null)
                    {
                        yield return cur;
                    }
                }
            }
        }
    }
}
