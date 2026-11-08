#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QLineEdit>
#include <QPushButton>
#include <QLabel>
#include <QMessageBox>
#include <QStack>       // QT-Container fuer den Stack
#include <QStringList>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    // Berechnung auch per Enter-Taste ausloesen
    connect(ui->lineEdit_input, &QLineEdit::returnPressed,
            this, &MainWindow::on_pushButton_calc_clicked);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_pushButton_calc_clicked()
{
    calculate();
}

void MainWindow::calculate()
{
    const QString input = ui->lineEdit_input->text().simplified();

    if (input.isEmpty()) {
        QMessageBox::warning(this, "UPN-Rechner", "Bitte zuerst einen Ausdruck eingeben!");
        return;
    }

    // Elemente sind durch Leerstellen getrennt
    const QStringList tokens = input.split(' ', Qt::SkipEmptyParts);

    QStack<double> stack;

    for (const QString &token : tokens) {
        bool isNumber = false;
        const double value = token.toDouble(&isNumber);

        if (isNumber) {
            // Operand auf den Stack pushen
            stack.push(value);
            continue;
        }

        // Operator: beide Operanden vom Stack holen
        if (token == "+" || token == "-" || token == "*" || token == "/") {
            if (stack.size() < 2) {
                QMessageBox::critical(this, "UPN-Rechner",
                    QString("Fehler: zu wenige Operanden fuer '%1'!").arg(token));
                ui->label_ergebnis->setText("Fehler");
                return;
            }
            const double b = stack.pop();   // zweiter Operand
            const double a = stack.pop();   // erster Operand
            double result = 0.0;

            if (token == "+")      result = a + b;
            else if (token == "-") result = a - b;
            else if (token == "*") result = a * b;
            else { // "/"
                if (b == 0.0) {
                    QMessageBox::critical(this, "UPN-Rechner",
                                          "Fehler: Division durch 0!");
                    ui->label_ergebnis->setText("Fehler");
                    return;
                }
                result = a / b;
            }
            stack.push(result);
        } else {
            QMessageBox::critical(this, "UPN-Rechner",
                QString("Fehler: '%1' ist weder Zahl noch Operator!").arg(token));
            ui->label_ergebnis->setText("Fehler");
            return;
        }
    }

    if (stack.size() != 1) {
        QMessageBox::critical(this, "UPN-Rechner",
            "Fehler: Ausdruck unvollstaendig - es bleibt mehr als ein Wert am Stack!");
        ui->label_ergebnis->setText("Fehler");
        return;
    }

    // Ergebnis im Label anzeigen ('g' schneidet ueberfluessige Nullen ab)
    ui->label_ergebnis->setText(QString::number(stack.top(), 'g', 12));
}
