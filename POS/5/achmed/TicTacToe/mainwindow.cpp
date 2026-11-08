#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QPushButton>
#include <QComboBox>
#include <QLabel>
#include <QMessageBox>
#include <QRandomGenerator>
#include <QTimer>

// Reihenfolge der Gewinnlinien (Zeilen, Spalten, Diagonalen)
static const int LINES[8][3] = {
    {0,1,2}, {3,4,5}, {6,7,8},   // Zeilen
    {0,3,6}, {1,4,7}, {2,5,8},   // Spalten
    {0,4,8}, {2,4,6}             // Diagonalen
};

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
    , current(0)
    , running(false)
{
    ui->setupUi(this);

    for (int i = 0; i < 9; ++i) {
        QPushButton *b = ui->centralwidget->findChild<QPushButton*>(
                             QString("pushButton_f%1").arg(i + 1));
        fields.append(b);
        b->setIconSize(QSize(72, 72));
        b->setMinimumSize(90, 90);
        connect(b, &QPushButton::clicked, this, [this, i]() { fieldClicked(i); });
    }

    for (int i = 0; i < 9; ++i) {
        board[i] = 0;
        fields[i]->setIcon(QIcon(":/images/empty.png"));
    }
}

MainWindow::~MainWindow()
{
    delete ui;
}

// ---------------- Spielstart ----------------

void MainWindow::on_pushButton_start_clicked()
{
    for (int i = 0; i < 9; ++i) {
        board[i] = 0;
        applySymbol(i, 0);
    }

    running = true;
    current = QRandomGenerator::global()->bounded(2);   // wer beginnt, ist zufaellig

    const QString starter = (current == 0) ? "Stern" : "Kreuz";
    QMessageBox::information(this, "Tic Tac Toe",
                             QString("%1 beginnt!").arg(starter));

    if (running && isComputerTurn())
        QTimer::singleShot(500, this, &MainWindow::computerMove);
}

// ---------------- Eingabe / Ablauf ----------------

void MainWindow::fieldClicked(int index)
{
    if (!running || isComputerTurn())
        return;

    if (!applyMove(index))     // Feld schon besetzt -> ignorieren
        return;

    if (running && isComputerTurn())
        QTimer::singleShot(500, this, &MainWindow::computerMove);
}

// Setzt den Zug von  *current*, wenn das Feld frei ist.
bool MainWindow::applyMove(int index)
{
    if (board[index] != 0)
        return false;

    board[index] = current + 1;              // 1 = Stern, 2 = Kreuz
    applySymbol(index, board[index]);

    const int player = board[index];
    if (winLine(player)) {
        running = false;
        const QString winner = (player == 1) ? "Stern" : "Kreuz";
        QMessageBox::information(this, "Tic Tac Toe",
                                 QString("Gewonnen hat: %1!").arg(winner));
    } else if (boardFull()) {
        running = false;
        QMessageBox::information(this, "Tic Tac Toe", "Unentschieden!");
    } else {
        current = 1 - current;
    }
    return true;
}

void MainWindow::applySymbol(int index, int player)
{
    if (player == 1)
        fields[index]->setIcon(QIcon(":/images/star.png"));
    else if (player == 2)
        fields[index]->setIcon(QIcon(":/images/cross.png"));
    else
        fields[index]->setIcon(QIcon(":/images/empty.png"));
}

// ---------------- Gewinnpruefung ----------------

bool MainWindow::winLine(int player) const
{
    for (const auto &line : LINES)
        if (board[line[0]] == player && board[line[1]] == player && board[line[2]] == player)
            return true;
    return false;
}

bool MainWindow::boardFull() const
{
    for (int i = 0; i < 9; ++i)
        if (board[i] == 0)
            return false;
    return true;
}

bool MainWindow::isComputerTurn() const
{
    // ComboBox: Index 0 = Mensch, Index 1 = PC
    if (current == 0)
        return ui->comboBox_player1->currentIndex() == 1;
    return ui->comboBox_player2->currentIndex() == 1;
}

// ---------------- Computergegner (Erweiterung) ----------------
// Strategie: Gewinnen -> Blocken -> Mitte -> Ecke -> Zufall

void MainWindow::computerMove()
{
    if (!running || !isComputerTurn())
        return;

    const int me = current + 1;               // eigene Symbolnummer
    const int opp = (me == 1) ? 2 : 1;

    // 1) eigener Gewinnzug
    for (int i = 0; i < 9; ++i)
        if (board[i] == 0 && triesToWin(i, me)) { applyMove(i); return; }

    // 2) Gegenzug blocken
    for (int i = 0; i < 9; ++i)
        if (board[i] == 0 && triesToWin(i, opp)) { applyMove(i); return; }

    // 3) Mitte, dann Ecken
    if (board[4] == 0) { applyMove(4); return; }
    const int corners[4] = { 0, 2, 6, 8 };
    QVector<int> freeCorners;
    for (int c : corners)
        if (board[c] == 0) freeCorners.append(c);
    if (!freeCorners.isEmpty()) {
        applyMove(freeCorners.at(QRandomGenerator::global()->bounded(freeCorners.size())));
        return;
    }

    // 4) beliebiges freies Feld
    QVector<int> freeFields;
    for (int i = 0; i < 9; ++i)
        if (board[i] == 0) freeFields.append(i);
    if (!freeFields.isEmpty())
        applyMove(freeFields.at(QRandomGenerator::global()->bounded(freeFields.size())));
}

// Wuerde Zug auf  index  fuer  player  eine Linie voll machen?
int MainWindow::triesToWin(int index, int player) const
{
    for (const auto &line : LINES) {
        int count = 0, empty = -1;
        for (int k = 0; k < 3; ++k) {
            const int f = board[line[k]];
            if (f == player) ++count;
            else if (f == 0 && empty == -1) empty = line[k];
            else { count = -10; break; }    // Feld vom Gegner -> nievoll
        }
        if (count == 2 && empty == index)
            return 1;
    }
    return 0;
}
