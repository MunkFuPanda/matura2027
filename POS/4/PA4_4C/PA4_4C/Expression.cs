using DataModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_4C
{
    internal abstract class Expression
    {
        internal abstract void Parse(List<Token> tokens);
        internal static List<String> Errors { get; set; } = new();
        internal virtual void Run(List<Worldcity> cityList, List<Worldcity> resultList) 
        {
            //cityList - die verfügbaren Städte
            //resultList - die Ergebnisliste
        }

        internal static readonly Dictionary<string, Func<Expression>> ExpressionFactories = new()
        {
            ["LARGEST"] = () => new LargestExpression(),
            ["SMALLEST"] = () => new SmallestExpression(),
            ["RANDOM"] = () => new RandomExpression(),
            ["SELECT"] = () => new SelectExpression(),
        };
    }
}
