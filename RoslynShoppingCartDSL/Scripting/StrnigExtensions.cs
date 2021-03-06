namespace RoslynShoppingCartDSL.Scripting {
    using System.IO;
    using System.Text;

    public static class StrnigExtensions {
        public static string ToPascalCase(this string snakeCasedString) {
            if (string.IsNullOrEmpty(snakeCasedString))
                return snakeCasedString;

            var result = new StringBuilder();
            var chuncks = snakeCasedString.Split('_');

            foreach (var chunck in chuncks) {
                if (chunck.Length == 0)
                    continue;

                var firtsChar = char.ToUpper(chunck[0]);
                result.Append(firtsChar + chunck.Substring(1));
            }

            return result.ToString();
        }

        public static string ToCSharp(this string src) {
            var cs = new StringBuilder();
            using (var reader=  new StringReader(src)) {
                while (reader.Peek() != -1) {
                    var line = reader.ReadLine();
                    if (line == null) continue;
                    var isIf = false;
                    foreach (var chunk in line.Split(' ')) {
                        if (chunk == "when") {
                            isIf = true;
                            cs.Append("if (");
                        }
                        else if (chunk == "and") {
                            cs.Append(" && ");
                        }
                        else {
                            cs.Append(chunk.ToPascalCase()
                                .Replace(":", "){\r\n")
                                //Those symbols are used to make the code esasy on the end user eyes. They are removed before eval.
                                .Replace("$", "")
                                .Replace("%", ""));
                            //                            
                        }
                    }
                    if (!isIf)
                        cs.Append(";\r\n");
                }
            }

            //var cs = new StringBuilder();
            //var chunks = src.ToLower().Split(new[] { ' ' });
            //foreach (var chunk in chunks) {
            //    if (chunk == "when")
            //        cs.Append("if (");
            //    else if (chunk == "and")
            //        cs.Append(" && ");
            //    else
            //        cs.Append(chunk.ToPascalCase().Replace(":", "){\r\n")
            //            .Replace("$", "")//Those symbols are used to make the code esasy on the end user eyes. They are removed before eval.
            //            .Replace("%", ""));
            //}
            return cs.Append("}").ToString();
        }
    }
}