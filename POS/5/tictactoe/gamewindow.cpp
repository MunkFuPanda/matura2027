#include "gamewindow.h"
#include "ui_gamewindow.h"

#include <QRandomGenerator>
#include <QMessageBox>
#include <QIcon>

GameWindow::GameWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::GameWindow)
{
    ui->setupUi(this);

    buttons[0] = ui->button1;
    buttons[1] = ui->button2;
    buttons[2] = ui->button3;
    buttons[3] = ui->button4;
    buttons[4] = ui->button5;
    buttons[5] = ui->button6;
    buttons[6] = ui->button7;
    buttons[7] = ui->button8;
    buttons[8] = ui->button9;

    for (int i = 0; i < 9; i++)
    {
        board[i] = ' ';
        buttons[i]->setEnabled(true);
        buttons[i]->setIcon(QIcon());
    }

    currentPlayer = QRandomGenerator::global()->bounded(2);

    QString player =
        (currentPlayer == 0) ? "Spieler 1" : "Spieler 2";

    QMessageBox::information(
        this,
        "Spiel gestartet",
        player + " beginnt!"
        );
}

GameWindow::~GameWindow()
{
    delete ui;
}

bool GameWindow::checkWinner()
{
    int wins[8][3] = {
        {0, 1, 2},
        {3, 4, 5},
        {6, 7, 8},
        {0, 3, 6},
        {1, 4, 7},
        {2, 5, 8},
        {0, 4, 8},
        {2, 4, 6}
    };

    for (int i = 0; i < 8; i++)
    {
        if (board[wins[i][0]] != ' ' &&
            board[wins[i][0]] == board[wins[i][1]] &&
            board[wins[i][1]] == board[wins[i][2]])
        {
            return true;
        }
    }

    return false;
}

bool GameWindow::checkDraw()
{
    for (int i = 0; i < 9; i++)
    {
        if (board[i] == ' ')
            return false;
    }

    return true;
}

void GameWindow::makeMove(int position)
{
    if (board[position] != ' ')
        return;

    if (currentPlayer == 0)
    {
        board[position] = 'X';
        buttons[position]->setIcon(
            QIcon(":/images/cross.png")
            );
    }
    else
    {
        board[position] = 'O';
        buttons[position]->setIcon(
            QIcon(":/images/star.png")
            );
    }

    buttons[position]->setEnabled(false);

    if (checkWinner())
    {
        QString winner =
            (currentPlayer == 0)
                ? "Spieler 1"
                : "Spieler 2";

        QMessageBox::information(
            this,
            "Spielende",
            winner + " hat gewonnen!"
            );

        disableBoard();
        return;
    }

    if (checkDraw())
    {
        QMessageBox::information(
            this,
            "Spielende",
            "Unentschieden!"
            );

        return;
    }

    currentPlayer = 1 - currentPlayer;
}

void GameWindow::disableBoard()
{
    for (int i = 0; i < 9; i++)
    {
        buttons[i]->setEnabled(false);
    }
}

void GameWindow::on_button1_clicked()
{
    makeMove(0);
}

void GameWindow::on_button2_clicked()
{
    makeMove(1);
}

void GameWindow::on_button3_clicked()
{
    makeMove(2);
}

void GameWindow::on_button4_clicked()
{
    makeMove(3);
}

void GameWindow::on_button5_clicked()
{
    makeMove(4);
}

void GameWindow::on_button6_clicked()
{
    makeMove(5);
}

void GameWindow::on_button7_clicked()
{
    makeMove(6);
}

void GameWindow::on_button8_clicked()
{
    makeMove(7);
}

void GameWindow::on_button9_clicked()
{
    makeMove(8);
}
