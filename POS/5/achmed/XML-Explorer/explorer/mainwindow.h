#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

namespace tinyxml2 {
    class XMLDocument;
    class XMLElement;
}

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
class QTreeWidgetItem;
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void on_pushButton_open_clicked();
    void on_pushButton_save_clicked();
    void on_pushButton_apply_clicked();
    void on_comboBox_attribute_currentIndexChanged(int index);
    void onTreeSelectionChanged();

private:
    void openFile(const QString &path);
    void addElementItem(QTreeWidgetItem *parent, tinyxml2::XMLElement *element);
    void showDetails(tinyxml2::XMLElement *element);
    static QString elementText(tinyxml2::XMLElement *element);
    static tinyxml2::XMLElement *elementOf(QTreeWidgetItem *item);

    Ui::MainWindow *ui;
    tinyxml2::XMLDocument *doc;
    QString currentFile;
    QTreeWidgetItem *selectedItem = nullptr;  // aktuell markierter Tree-Knoten
    bool dirty;
    bool updatingCombo;   // verhindert Signalraeckerkursion beim Auffuellen
};

#endif // MAINWINDOW_H
