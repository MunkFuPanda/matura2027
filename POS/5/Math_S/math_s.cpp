#include "math_s.h"
#include "exprtk.hpp"

Math_S::Math_S() {}

typedef exprtk::symbol_table<double> symbol_table_t;
typedef exprtk::expression<double> expression_t;
typedef exprtk::parser<double> parser_t;

double Math_S::calculate(QString expression) {
    std::string temp = expression.toStdString();

    symbol_table_t symbol_table;
    symbol_table.add_constants(); // e nicht enthalten in hier
    symbol_table.add_constant("e", exprtk::details::numeric::constant::e);
    expression_t expression2;
    expression2.register_symbol_table(symbol_table);

    parser_t parser;
    if (!parser.compile(temp, expression2)) {
        throw std::runtime_error("exprtk: " + parser.error() + " in \"" + temp + "\"");
    }
    return expression2.value();
}
