#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QMessageBox>
#include <QPushButton>
#include <QIcon>
#include <random>
#include <vector>


MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    // Spieler 1
    ui->comboBox->addItem("Spieler");
    ui->comboBox->addItem("Computer");

    // Spieler 2
    ui->comboBox_2->addItem("Spieler");
    ui->comboBox_2->addItem("Computer");
}


MainWindow::~MainWindow()
{
    delete ui;
}


void MainWindow::on_pushButton_clicked()
{
    // Array zurücksetzen
    for (int i = 0; i < 9; i++) {
        arr[i] = 0;
    }

    // Spielfeld zurücksetzen
    for (int i = 0; i < 9; i++) {
        getButton(i)->setIcon(QIcon(":/images/empty.png"));
    }

    started = true;

    // Zufällig bestimmen, wer beginnt
    std::random_device rd;
    std::mt19937 gen(rd());
    std::uniform_int_distribution<int> dist(1, 2);

    int randomNumber = dist(gen);

    if (randomNumber == 1) {
        counter = true;
    }
    else {
        counter = false;
    }

    QMessageBox msgBox(this);

    msgBox.setText(
        "Spieler " + QString::number(randomNumber) + " beginnt!"
        );

    msgBox.setStandardButtons(QMessageBox::Ok);
    msgBox.setDefaultButton(QMessageBox::Ok);

    msgBox.exec();

    // Falls der Computer beginnt
    if (isComputerTurn()) {
        computerMove();
    }
}

void MainWindow::on_pushButton_2_clicked()
{
    if (!started || isComputerTurn()) {
        return;
    }

    makeMove(0);
}

void MainWindow::on_pushButton_3_clicked()
{
    if (!started || isComputerTurn()) {
        return;
    }

    makeMove(1);
}

void MainWindow::on_pushButton_4_clicked()
{
    if (!started || isComputerTurn()) {
        return;
    }

    makeMove(2);
}

void MainWindow::on_pushButton_5_clicked()
{
    if (!started || isComputerTurn()) {
        return;
    }

    makeMove(3);
}

void MainWindow::on_pushButton_6_clicked()
{
    if (!started || isComputerTurn()) {
        return;
    }

    makeMove(4);
}

void MainWindow::on_pushButton_7_clicked()
{
    if (!started || isComputerTurn()) {
        return;
    }

    makeMove(5);
}

void MainWindow::on_pushButton_8_clicked()
{
    if (!started || isComputerTurn()) {
        return;
    }

    makeMove(6);
}

void MainWindow::on_pushButton_9_clicked()
{
    if (!started || isComputerTurn()) {
        return;
    }

    makeMove(7);
}

void MainWindow::on_pushButton_10_clicked()
{
    if (!started || isComputerTurn()) {
        return;
    }

    makeMove(8);
}

void MainWindow::makeMove(int index)
{
    // Spiel läuft nicht oder Feld bereits belegt
    if (!started || arr[index] != 0) {
        return;
    }

    // Spieler 1
    if (counter) {
        arr[index] = 1;

        getButton(index)->setIcon(
            QIcon(":/images/cross.png")
            );
    }

    // Spieler 2
    else {
        arr[index] = 2;

        getButton(index)->setIcon(
            QIcon(":/images/star.png")
            );
    }

    // Spieler wechseln
    counter = !counter;

    int winner = checkWin(arr);

    if (winner != 0) {

        QMessageBox::information(
            this,
            "Gewonnen",
            QString("Spieler %1 hat gewonnen!").arg(winner)
            );

        started = false;

        return;
    }

    bool freeField = false;

    for (int i = 0; i < 9; i++) {

        if (arr[i] == 0) {
            freeField = true;
            break;
        }
    }

    if (!freeField) {

        QMessageBox::information(
            this,
            "Unentschieden",
            "Das Spiel endet unentschieden!"
            );

        started = false;

        return;
    }

    if (isComputerTurn()) {
        computerMove();
    }
}

void MainWindow::computerMove()
{
    if (!started) {
        return;
    }

    std::vector<int> freeFields;

    // Alle freien Felder suchen
    for (int i = 0; i < 9; i++) {

        if (arr[i] == 0) {
            freeFields.push_back(i);
        }
    }

    if (freeFields.empty()) {
        return;
    }


    // Zufälliges freies Feld auswählen
    std::random_device rd;
    std::mt19937 gen(rd());

    std::uniform_int_distribution<int> dist(
        0,
        static_cast<int>(freeFields.size()) - 1
        );

    int randomIndex = dist(gen);

    int field = freeFields[randomIndex];


    // Zug durchführen
    makeMove(field);
}

bool MainWindow::isComputerTurn()
{
    // counter == true -> Spieler 1
    if (counter) {

        // Index 1 = Computer
        return ui->comboBox->currentIndex() == 1;
    }

    // counter == false -> Spieler 2
    else {

        return ui->comboBox_2->currentIndex() == 1;
    }
}

QPushButton *MainWindow::getButton(int index)
{
    switch (index) {

    case 0:
        return ui->pushButton_2;

    case 1:
        return ui->pushButton_3;

    case 2:
        return ui->pushButton_4;

    case 3:
        return ui->pushButton_5;

    case 4:
        return ui->pushButton_6;

    case 5:
        return ui->pushButton_7;

    case 6:
        return ui->pushButton_8;

    case 7:
        return ui->pushButton_9;

    case 8:
        return ui->pushButton_10;

    default:
        return nullptr;
    }
}

int MainWindow::checkWin(int *arr)
{
    if (
        arr[0] != 0 &&
        arr[0] == arr[1] &&
        arr[1] == arr[2]
        ) {
        return arr[0];
    }


    if (
        arr[3] != 0 &&
        arr[3] == arr[4] &&
        arr[4] == arr[5]
        ) {
        return arr[3];
    }


    if (
        arr[6] != 0 &&
        arr[6] == arr[7] &&
        arr[7] == arr[8]
        ) {
        return arr[6];
    }

    if (
        arr[0] != 0 &&
        arr[0] == arr[3] &&
        arr[3] == arr[6]
        ) {
        return arr[0];
    }


    if (
        arr[1] != 0 &&
        arr[1] == arr[4] &&
        arr[4] == arr[7]
        ) {
        return arr[1];
    }


    if (
        arr[2] != 0 &&
        arr[2] == arr[5] &&
        arr[5] == arr[8]
        ) {
        return arr[2];
    }

    if (
        arr[0] != 0 &&
        arr[0] == arr[4] &&
        arr[4] == arr[8]
        ) {
        return arr[0];
    }


    if (
        arr[2] != 0 &&
        arr[2] == arr[4] &&
        arr[4] == arr[6]
        ) {
        return arr[2];
    }


    // Kein Gewinner
    return 0;
}