using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_4C
{
    internal class RandomExpression : Expression
    {
        private Random random = new();
        internal override void Parse(List<Token> tokens) { }


        internal override void Run(List<Worldcity> cityList, List<Worldcity> resultList)
        {
            resultList.Add(cityList[random.Next(0,cityList.Count-1)]);
        }
    }
}
