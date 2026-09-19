#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "stack.h"
#include <sstream>
#include <stdexcept>
#include <string>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_pushButton_clicked()
{
    try {
        const int result = calculateUpn(ui->lineEdit->text().toStdString());
        ui->label->setText(QString::number(result));
    } catch (const std::exception& error) {
        ui->label->setText(QString("Fehler: %1").arg(QString::fromUtf8(error.what())));
    }
}

int MainWindow::calculateUpn(const std::string& input)
{
    Stack stack;
    std::istringstream tokens(input);
    std::string token;
    bool hasToken = false;

    while (tokens >> token) {
        hasToken = true;

        // Erst versuchen, das Token als ganze Zahl zu lesen.
        try {
            std::size_t charactersRead = 0;
            const int number = std::stoi(token, &charactersRead);

            if (charactersRead == token.size()) {
                stack.push(number);
                continue;
            }
        } catch (const std::invalid_argument&) {
            // Das Token ist keine Zahl und wird unten als Operator geprüft.
        } catch (const std::out_of_range&) {
            throw std::runtime_error("Die Zahl ist zu gross oder zu klein");
        }

        if (token.size() != 1 || std::string("+-*/").find(token[0]) == std::string::npos)
            throw std::runtime_error("Unbekanntes Token: " + token);

        if (stack.anzahl < 2)
            throw std::runtime_error("Zu wenige Zahlen fuer den Operator");

        const int right = stack.pop();
        const int left = stack.pop();
        int result = 0;

        switch (token[0]) {
        case '+':
            result = left + right;
            break;
        case '-':
            result = left - right;
            break;
        case '*':
            result = left * right;
            break;
        case '/':
            if (right == 0)
                throw std::runtime_error("Division durch 0");

            result = left / right;
            break;
        default:
            // Durch die Operatorprüfung oben nicht erreichbar.
            throw std::runtime_error("Unbekannter Operator");
        }

        stack.push(result);
    }

    if (!hasToken)
        throw std::runtime_error("Keine Eingabe");

    if (stack.anzahl != 1)
        throw std::runtime_error("Die Eingabe ist keine gueltige UPN-Rechnung");

    return stack.pop();
}