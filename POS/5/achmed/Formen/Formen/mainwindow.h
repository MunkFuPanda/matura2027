#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QList>
#include <QLibrary>
#include "form.h"
#include "factory.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow();
    void paintEvent(QPaintEvent *event) override;

private slots:
    void on_pushButton_clicked();
    void on_pushButton_clear_clicked();
    void on_form_select_currentTextChanged(const QString &arg1);

private:
    void load_plugins();

    Ui::MainWindow *ui;
    QList<Form*> formen;           // die erzeugten Formen
    Factory *factory;
    QList<QLibrary*> libs;         // Plugins offen halten bis zum Ende
};

#endif // MAINWINDOW_H
