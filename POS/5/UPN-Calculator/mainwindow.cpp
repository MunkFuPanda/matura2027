#include "mainwindow.h"
#include "ui_mainwindow.h"

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

void MainWindow::on_pushButton_pressed()
{
    QStringList operators = QStringList({"+", "-", "*", "/"});
    QString input = ui->lineEdit->text();
    QStringList pieces = input.split(" ");
    // early ending incase of invalid input
    if (!operators.contains(pieces.last())) {
        ui->lineEdit->setText("Invalid Expression");
        return;
    }
    // calculation loop
    for (auto piece : pieces) {
        if (!operators.contains(piece)) {
            stack.push(piece.toInt());
        } else {
            if (stack.isEmpty()) {
                ui->lineEdit->setText("Invalid Expression");
                return;
            }
            int right = stack.pop();
            if (stack.isEmpty()) {
                ui->lineEdit->setText("Invalid Expression");
                return;
            }
            int left = stack.pop();
            int total = compute(right, left, piece);
            if (total == INT_MAX) {
                ui->lineEdit->setText("Divion by 0");
                return;
            }
            stack.push(total);
        }
    }
    if (stack.size() == 1) {
        ui->lineEdit->setText(QString::number(stack.pop()));
    }
}

// helper function
int MainWindow::compute(int right, int left, QString op) {
    if (op == "+") {
        return left + right;
    } else if (op == "-") {
        return left - right;
    } else if (op == "*") {
        return left * right;
    } else if (op == "/") {
        if (right == 0) return INT_MAX;
        return left / right;
    }
    // find proper error return value here, if needed
    return 0;
}