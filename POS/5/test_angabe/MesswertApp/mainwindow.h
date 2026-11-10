#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QTreeWidget>
#include <vector>

QT_BEGIN_NAMESPACE
namespace Ui {
class MainWindow;
}
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow() override;

private slots:
    void on_pushButton_clicked();

private:
    Ui::MainWindow *ui;
    void newElement(const QString &messart, double wert, const QString &datentyp);
    std::vector<double> tempList;
    std::vector<double> distList;
    QTreeWidgetItem* temperaturMaximum = nullptr;
    QTreeWidgetItem* distanzMaximum = nullptr;
};
#endif // MAINWINDOW_H
