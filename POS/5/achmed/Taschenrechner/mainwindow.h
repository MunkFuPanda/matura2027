#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void on_pushButton_clear_clicked();
    void on_pushButton_back_clicked();
    void on_pushButton_eq_clicked();

private:
    void appendText(const QString &text);
    QString adaptFormula(const QString &raw) const;  // an exprtk-Syntax anpassen
    Ui::MainWindow *ui;
};

#endif // MAINWINDOW_H
