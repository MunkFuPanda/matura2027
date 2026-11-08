#include "mainwindow.h"
#include "ui_mainwindow.h"

// Hinweis aus der Angabe: vor dem Include der exprtk.hpp
#define exprtk_disable_enhanced_features
#include "exprtk.hpp"

#include <QLineEdit>
#include <QPushButton>
#include <QMessageBox>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    // Alle Zahlen-/Operator-/Klammerbuttons hingen sich nach dem
    // Auto-Connect-Prinzip selbst an den Display-Text an.
    struct ButtonText { QPushButton *button; QString text; };
    const QList<ButtonText> map = {
        { ui->pushButton_0, "0" },   { ui->pushButton_1, "1" },
        { ui->pushButton_2, "2" },   { ui->pushButton_3, "3" },
        { ui->pushButton_4, "4" },   { ui->pushButton_5, "5" },
        { ui->pushButton_6, "6" },   { ui->pushButton_7, "7" },
        { ui->pushButton_8, "8" },   { ui->pushButton_9, "9" },
        { ui->pushButton_dot, "." }, { ui->pushButton_plus, "+" },
        { ui->pushButton_minus, "-" },{ ui->pushButton_times, "*" },
        { ui->pushButton_divide, "/" },{ ui->pushButton_open, "(" },
        { ui->pushButton_close, ")" }
    };
    for (const ButtonText &bt : map) {
        const QString t = bt.text;
        connect(bt.button, &QPushButton::clicked, this, [this, t]() { appendText(t); });
    }

    ui->lineEdit_display->setReadOnly(true);   // Eingabe nur per Buttons
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::appendText(const QString &text)
{
    ui->lineEdit_display->setText(ui->lineEdit_display->text() + text);
}

void MainWindow::on_pushButton_clear_clicked()
{
    ui->lineEdit_display->clear();
}

void MainWindow::on_pushButton_back_clicked()
{
    QString t = ui->lineEdit_display->text();
    t.chop(1);
    ui->lineEdit_display->setText(t);
}

// Anpassungen an die Syntax der Bibliothek:
//  - Dezimaltrennzeichen ',' wird zu '.'
//  - implizite Multiplikationen einfuegen: 3( -> 3*(, )( -> )*(, )3 -> )*3
QString MainWindow::adaptFormula(const QString &raw) const
{
    QString s = raw.trimmed();
    s.replace(',', '.');

    QString out;
    for (int i = 0; i < s.size(); ++i) {
        const QChar c = s.at(i);
        if (i > 0) {
            const QChar p = s.at(i - 1);
            const bool prevOperand = p.isDigit() || p == ')';
            const bool startsOperand = c.isDigit() || c == '(';
            if (prevOperand && startsOperand)
                out += '*';
        }
        out += c;
    }
    return out;
}

void MainWindow::on_pushButton_eq_clicked()
{
    const QString formula = adaptFormula(ui->lineEdit_display->text());

    if (formula.isEmpty())
        return;

    try {
        exprtk::expression<double> expression;
        exprtk::parser<double> parser;

        if (!parser.compile(formula.toStdString(), expression)) {
            QMessageBox::critical(this, "Taschenrechner",
                QString("Ungueltige Formel:\n%1")
                    .arg(QString::fromStdString(parser.error())));
            return;
        }

        const double result = expression.value();
        ui->lineEdit_display->setText(QString::number(result, 'g', 15));
    } catch (std::exception &ex) {
        QMessageBox::critical(this, "Taschenrechner",
            QString("Fehler bei der Auswertung:\n%1").arg(ex.what()));
    }
}
