#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    buttons->addButton(ui->pushButton_0);
    buttons->addButton(ui->pushButton_1);
    buttons->addButton(ui->pushButton_2);
    buttons->addButton(ui->pushButton_3);
    buttons->addButton(ui->pushButton_4);
    buttons->addButton(ui->pushButton_5);
    buttons->addButton(ui->pushButton_6);
    buttons->addButton(ui->pushButton_7);
    buttons->addButton(ui->pushButton_8);
    buttons->addButton(ui->pushButton_9);
    buttons->addButton(ui->pushButtonCB);
    buttons->addButton(ui->pushButtonOB);
    buttons->addButton(ui->pushButtonPlus);
    buttons->addButton(ui->pushButtonMinus);
    buttons->addButton(ui->pushButtonMulti);
    buttons->addButton(ui->pushButtonDiv);
    buttons->addButton(ui->pushButtonEq);
    buttons->addButton(ui->pushButtonC);
    buttons->addButton(ui->pushButtonBP);
    buttons->addButton(ui->pushButtonPi);
    buttons->addButton(ui->pushButtonE);
    buttons->addButton(ui->pushButtonComma);
    connect(buttons,
            &QButtonGroup::idClicked,
            this,
            &MainWindow::buttonWasClicked);

}

MainWindow::~MainWindow()
{
    delete ui;
    delete buttons;
}

void MainWindow::buttonWasClicked(int buttonID) {
    if (ui->lineEdit->text().contains("=")) ui->lineEdit->setText("");

    if (buttons->button(buttonID)->text() != "Backspace"
        && buttons->button(buttonID)->text() != "C") ui->lineEdit->setText(ui->lineEdit->text() + buttons->button(buttonID)->text());
    else {
        if (buttons->button(buttonID)->text() == "C") ui->lineEdit->setText("");
        else if (ui->lineEdit->text() != "" && ui->lineEdit->text().length() > 1 && ui->lineEdit->text().last(2) == "pi") {
            ui->lineEdit->setText(ui->lineEdit->text().removeLast());
            ui->lineEdit->setText(ui->lineEdit->text().removeLast());
        }
        else ui->lineEdit->setText(ui->lineEdit->text().removeLast());
    }
    if (buttons->button(buttonID)->text() == "=") equals();
}

void MainWindow::equals() {
    Math_S math;
    double result = math.calculate(ui->lineEdit->text().removeLast());
    ui->lineEdit->setText(ui->lineEdit->text() + QString::number(result, 'f', 4));
}