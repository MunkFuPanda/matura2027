using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_4C
{
    internal class Program : Expression
    {
        private List<Expression> expressions = [];
        internal override void Parse(List<Token> tokens)
        {
            while (tokens.Count > 0)
            {
                var token = tokens.First();
                if (token.Type == Token.TokenType.Close)
                    return;

                if (token.Type != Token.TokenType.Keyword)
                {
                    string foundType = token.Type switch
                    {
                        Token.TokenType.Keyword => "Keyword",
                        Token.TokenType.Identifier => "Identifier",
                        _ => token.Type.ToString()
                    };

                    Errors.Add($"Unexpected Token, expected Keyword, found {foundType}: {token.Value}");

                    tokens.RemoveAt(0);
                    continue;
                }

                if (token.Value != "COUNTRY")
                {
                    Errors.Add($"Unpassendes Schlüsselwort: {token.Value}");

                    tokens.RemoveAt(0);
                    continue;
                }

                tokens.RemoveAt(0); // Keyword konsumieren

                Expression expression = new CountryExpression();
                expression.Parse(tokens);
                expressions.Add(expression);
            }
        }

        internal override void Run(List<Worldcity> cityList, List<Worldcity> resultList)
        {
            foreach (Expression expression in expressions)
            {
                expression.Run(cityList, resultList);
            }
        }
    }
}
