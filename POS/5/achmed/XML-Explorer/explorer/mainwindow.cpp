#include "mainwindow.h"
#include "ui_mainwindow.h"

#include "tinyxml2.h"

#include <QTreeWidgetItem>
#include <QTreeWidget>
#include <QPlainTextEdit>
#include <QComboBox>
#include <QLineEdit>
#include <QPushButton>
#include <QFileDialog>
#include <QMessageBox>
#include <QDir>
#include <QFileInfo>

using namespace tinyxml2;

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
    , doc(new XMLDocument())
    , dirty(false)
    , updatingCombo(false)
{
    ui->setupUi(this);

    ui->edit_details->setReadOnly(true);
    ui->lineEdit_value->setEnabled(false);
    ui->pushButton_apply->setEnabled(false);
    ui->pushButton_save->setEnabled(false);

    connect(ui->tree_elements, &QTreeWidget::itemSelectionChanged,
            this, &MainWindow::onTreeSelectionChanged);
}

MainWindow::~MainWindow()
{
    delete doc;
    delete ui;
}

// ---------------- Datei laden / speichern ----------------

void MainWindow::on_pushButton_open_clicked()
{
    const QString path = QFileDialog::getOpenFileName(
        this, "XML-Datei oeffnen", QDir::homePath(),
        "XML-Dateien (*.xml);;Alle Dateien (*)");

    if (!path.isEmpty())
        openFile(path);
}

void MainWindow::openFile(const QString &path)
{
    const XMLError err = doc->LoadFile(path.toUtf8().constData());
    if (err != XML_SUCCESS) {
        QMessageBox::critical(this, "XML Explorer",
            QString("Datei konnte nicht gelesen werden:\n%1")
                .arg(QString::fromUtf8(doc->ErrorStr())));
        return;
    }

    currentFile = path;
    dirty = false;
    ui->pushButton_save->setEnabled(true);
    ui->tree_elements->clear();

    XMLElement *root = doc->RootElement();
    if (!root) {
        QMessageBox::warning(this, "XML Explorer", "Die Datei enthaelt kein XML-Element.");
        return;
    }

    // Wurzelelement direkt oben ins TreeWidget (ohne unsichtbaren Elternknoten)
    addElementItem(nullptr, root);
    ui->tree_elements->expandAll();
    ui->statusbar->showMessage(path);
    setWindowTitle(QString("XML Explorer - %1").arg(QFileInfo(path).fileName()));
}

void MainWindow::on_pushButton_save_clicked()
{
    if (currentFile.isEmpty())
        return;

    const XMLError err = doc->SaveFile(currentFile.toUtf8().constData());
    if (err != XML_SUCCESS) {
        QMessageBox::critical(this, "XML Explorer",
            QString("Speichern fehlgeschlagen:\n%1")
                .arg(QString::fromUtf8(doc->ErrorStr())));
        return;
    }
    dirty = false;
    ui->statusbar->showMessage("Gespeichert: " + currentFile);
}

// ---------------- Tree aufbauen ----------------

void MainWindow::addElementItem(QTreeWidgetItem *parent, XMLElement *element)
{
    QTreeWidgetItem *item = new QTreeWidgetItem;
    if (parent)
        parent->addChild(item);
    else
        ui->tree_elements->addTopLevelItem(item);

    item->setText(0, elementText(element));
    item->setData(0, Qt::UserRole,
                  reinterpret_cast<qulonglong>(element));

    for (XMLElement *child = element->FirstChildElement();
         child; child = child->NextSiblingElement())
        addElementItem(item, child);
}

QString MainWindow::elementText(XMLElement *element)
{
    // Attribute kurz mitanzeigen, z.B. <person id="1">
    QString attrs;
    for (const XMLAttribute *a = element->FirstAttribute();
         a; a = a->Next())
        attrs += QString(" %1=\"%2\"").arg(QString::fromUtf8(a->Name()),
                                           QString::fromUtf8(a->Value()));

    return QString("<%1%2>").arg(QString::fromUtf8(element->Name()), attrs);
}

XMLElement *MainWindow::elementOf(QTreeWidgetItem *item)
{
    if (!item)
        return nullptr;
    return reinterpret_cast<XMLElement*>(item->data(0, Qt::UserRole).toULongLong());
}

// ---------------- Auswahl: Attribute + Inhalt anzeigen ----------------

void MainWindow::onTreeSelectionChanged()
{
    const QList<QTreeWidgetItem*> sel = ui->tree_elements->selectedItems();
    QTreeWidgetItem *item = sel.isEmpty() ? nullptr : sel.first();
    selectedItem = item;

    XMLElement *element = elementOf(item);
    if (!element) {
        ui->edit_details->clear();
        ui->comboBox_attribute->clear();
        ui->lineEdit_value->clear();
        ui->lineEdit_value->setEnabled(false);
        ui->pushButton_apply->setEnabled(false);
        return;
    }

    showDetails(element);

    // ComboBox mit den Attributen des Elements fuellen (Erweiterung)
    updatingCombo = true;
    ui->comboBox_attribute->blockSignals(true);
    ui->comboBox_attribute->clear();
    for (const XMLAttribute *a = element->FirstAttribute();
         a; a = a->Next())
        ui->comboBox_attribute->addItem(QString::fromUtf8(a->Name()));
    ui->comboBox_attribute->blockSignals(false);
    updatingCombo = false;

    const bool hasAttrs = ui->comboBox_attribute->count() > 0;
    ui->lineEdit_value->setEnabled(hasAttrs);
    ui->pushButton_apply->setEnabled(hasAttrs);
    if (hasAttrs)
        on_comboBox_attribute_currentIndexChanged(ui->comboBox_attribute->currentIndex());
    else
        ui->lineEdit_value->clear();
}

void MainWindow::showDetails(XMLElement *element)
{
    QString details = QString("Tag: %1\n\nAttribute:\n")
                          .arg(QString::fromUtf8(element->Name()));

    bool hasAttrs = false;
    for (const XMLAttribute *a = element->FirstAttribute();
         a; a = a->Next()) {
        details += QString("  %1 = %2\n").arg(QString::fromUtf8(a->Name()),
                                              QString::fromUtf8(a->Value()));
        hasAttrs = true;
    }
    if (!hasAttrs)
        details += "  (keine)\n";

    // Inhalt (Textkind) des Elements, falls vorhanden
    const char *content = element->GetText();
    details += QString("\nInhalt:\n  %1")
                   .arg(content ? QString::fromUtf8(content) : "(leer)");

    ui->edit_details->setPlainText(details);
}

// ---------------- Attribut bearbeiten (Erweiterung) ----------------

void MainWindow::on_comboBox_attribute_currentIndexChanged(int index)
{
    if (updatingCombo)
        return;

    XMLElement *element = elementOf(selectedItem);
    if (!element)
        return;

    const QString name = ui->comboBox_attribute->itemText(index);
    const char *value = element->Attribute(name.toUtf8().constData());
    ui->lineEdit_value->setText(value ? QString::fromUtf8(value) : QString());
}

void MainWindow::on_pushButton_apply_clicked()
{
    XMLElement *element = elementOf(selectedItem);
    if (!element)
        return;

    const QString name = ui->comboBox_attribute->currentText();
    if (name.isEmpty())
        return;

    element->SetAttribute(name.toUtf8().constData(),
                          ui->lineEdit_value->text().toUtf8().constData());
    dirty = true;

    // Anzeige aktualisieren
    selectedItem->setText(0, elementText(element));
    showDetails(element);
    ui->statusbar->showMessage("Attribut geaendert - nicht vergessen zu speichern!");
}
