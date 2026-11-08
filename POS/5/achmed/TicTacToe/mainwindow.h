#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QVector>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
class QPushButton;
QT_END_NAMESPACE

// 0 = leer, 1 = Stern (Spieler 1), 2 = Kreuz (Spieler 2)
class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void on_pushButton_start_clicked();
    void fieldClicked(int index);
    void computerMove();

private:
    void applySymbol(int index, int player);
    bool applyMove(int index);
    bool winLine(int player) const;
    bool boardFull() const;
    bool isComputerTurn() const;
    int  bestMove() const;
    int  triesToWin(int index, int player) const;

    Ui::MainWindow *ui;
    QVector<QPushButton*> fields;   // 9 Buttons des Spielfelds
    int board[9];                   // Inhalt der Felder
    int current;                    // 0 = Spieler 1 (Stern), 1 = Spieler 2 (Kreuz)
    bool running;
};

#endif // MAINWINDOW_H
