using DataModels;
using LinqToDB.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4_4C
{
    internal class CountryExpression : Expression
    {
        public List<Expression> body = [];
        private String country;
        internal override void Parse(List<Token> tokens)
        {
            if (tokens.Count > 0 && tokens.First().Type == Token.TokenType.Identifier)
            {
                country = tokens.First().Value;
                tokens.RemoveAt(0);

                if (tokens.Count > 0)
                {
                    if (tokens.First().Type == Token.TokenType.Open)
                    {
                        tokens.RemoveAt(0);
                        while (tokens.Count > 0 && tokens.First().Type != Token.TokenType.Close)
                        {
                            if (tokens.First().Type != Token.TokenType.Keyword)
                            {
                                string foundType = tokens.First().Type switch
                                {
                                    Token.TokenType.Keyword => "Keyword",
                                    Token.TokenType.Identifier => "Identifier",
                                    _ => tokens.First().Type.ToString()
                                };

                                Errors.Add($"Unexpected Token, expected Keyword, found {foundType}: {tokens.First().Value}");

                                tokens.RemoveAt(0);
                                continue;
                            }

                            if (!ExpressionFactories.TryGetValue(tokens.First().Value, out var factory))
                            {
                                Errors.Add($"Unbekanntes Schlüsselwort: {tokens.First().Value}");

                                tokens.RemoveAt(0);
                                continue;
                            }

                            tokens.RemoveAt(0); // Keyword konsumieren

                            Expression expression = factory();
                            expression.Parse(tokens);
                            body.Add(expression);
                        }
                        if (tokens.Count > 0 && tokens.First().Type == Token.TokenType.Close)
                        {
                            tokens.RemoveAt(0);
                        }
                        else
                        {
                            Errors.Add($"Fehlende schließende Klammer für Country");
                        }
                    }
                    else
                    {
                        Errors.Add($"Erwartete öffnende Klammer für Country, nicht {tokens.First().Value}");
                        tokens.RemoveAt(0);
                    }
                }
            } else
            {
                Errors.Add($"Erwartetes Land für COUNTRY, nicht {tokens.First().Value}");
                tokens.RemoveAt(0);
            }
        }

        internal override void Run(List<Worldcity> cityList, List<Worldcity> resultList)
        {
            var CountryCityList = cityList.FindAll(c => c.Country.ToLower().Equals(country.ToLower()));

            foreach (Expression expression in body)
            {
                expression.Run(CountryCityList, resultList);
            }
        }
    }
}
