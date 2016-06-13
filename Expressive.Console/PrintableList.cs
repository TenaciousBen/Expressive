using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Console
{
    public class PrintableList
    {
        public List<EvaluationResult> List { get; set; }

        public PrintableList(List<EvaluationResult> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            List = list;
        }

        public override string ToString()
        {
            var printable = new StringBuilder("(");
            for (int i = 0; i < List.Count; i++)
            {
                var item = List[i];
                printable.Append(item.Result?.ToString() ?? "(null)");
                if (i < List.Count - 1) printable.Append(", ");
            }
            printable.Append(")");
            return printable.ToString();
        }
    }
}
